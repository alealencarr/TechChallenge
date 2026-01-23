namespace Domain.Entities.Aggregates.AggregateProduct
{
    public class Product
    {
        public Product(Guid id, string name, string description, decimal price, Guid categorieId, DateTime createdAt, List<ProductIngredient> ingredients, List<ProductImage> images, bool isLanche)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            CategorieId = categorieId;
            CreatedAt = createdAt;
            IsLanche = isLanche;
            ToAddImages(images);
            ToAddIngredients(ingredients);
        }

        private Product(string name, decimal price, Guid categorieId, string description, List<ProductIngredient> ingredients, List<ProductImage> images, bool isLanche)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("É necessário informar um nome para o Produto.");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("É necessário informar uma descrição para o Produto.");
            if (price == 0)
                throw new ArgumentException("O preço do produto deve ser maior que zero.");

            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Description = description;
            CategorieId = categorieId;
            CreatedAt = DateTime.Now;

            if (isLanche)
            {
                if (ingredients.Count == 0)
                    throw new ArgumentException("É necessário informar pelo menos um ingrediente para criar um produto do tipo Lanche.");

                ingredients = ingredients.Select(x => new ProductIngredient(Id, x.IngredientId, x.Quantity)).ToList();
                ToAddIngredients(ingredients);
            }

            if (images.Count > 0)
            {
                images = images.Select(x => new ProductImage(Id, x.Blob, x.Name, $"produtos/imagens/produto-{Id.ToString()}", x.MimeType, x.FileName)).ToList();
                ToAddImages(images);
            }

            IsLanche = isLanche;

        }
 
        #region Construtor Update Product 
        private Product(Guid id, string name, decimal price, Guid categorieId, string description, List<ProductIngredient> ingredients, List<ProductImage> images, bool isLanche)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            CategorieId = categorieId;

            if (isLanche)
            {
                if (ingredients.Count == 0)
                    throw new ArgumentException("É necessário informar pelo menos um ingrediente para alterar um produto do tipo Lanche.");

                ingredients = ingredients.Select(x => new ProductIngredient(Id, x.IngredientId, x.Quantity)).ToList();
                ToAddIngredients(ingredients);

            }

            if (images.Count > 0)
            {
                images = images.Select(x => new ProductImage(Id, x.Blob, x.Name, $"produtos/imagens/produto-{Id.ToString()}", x.MimeType, x.FileName)).ToList();
                ToAddImages(images);

            }

            IsLanche = isLanche;
        }
        #endregion

        public static Product Create(string name, decimal price, Guid categorieId, string description, List<ProductIngredient> ingredients, List<ProductImage> images, bool isLanche)
        {
            return new Product(name, price, categorieId, description, ingredients, images, isLanche);
        }

        public static Product Update(Guid id, string name, decimal price, Guid categorieId, string description, List<ProductIngredient> ingredients, List<ProductImage> images, bool isLanche)
        {
            return new Product(id, name, price, categorieId, description, ingredients, images, isLanche);
        }

        protected Product() { }
        public DateTime CreatedAt { get; private set; }
        public Guid Id { get; private set; }
        public string Name { get; set; }

        public decimal Price { get; set; }
        public Guid CategorieId { get; set; }
        public bool IsLanche { get; set; }  

        public string Description { get; set; }

        public ICollection<ProductImage> ProductImages { get; private set; } = [];

        public ICollection<ProductIngredient> ProductIngredients { get; private set; } = [];

        public void ToAddIngredients(ICollection<ProductIngredient> produtoIngredientes)
        {
            ProductIngredients = produtoIngredientes;
        }

        public void ToAddImages(ICollection<ProductImage> produtoImagens)
        {
            ProductImages = produtoImagens;
        }
    }
}
