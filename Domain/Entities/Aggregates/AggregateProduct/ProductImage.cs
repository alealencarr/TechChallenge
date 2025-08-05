namespace Domain.Entities.Aggregates.AggregateProduct
{
    public class ProductImage
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public byte[] Blob { get; set; } = default!;
        public Guid Id { get; set; }
        public ProductImage(Guid productId, byte[] blob, string name, string imagePath, string mimeType, string fileName)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Blob = blob;
            Name = name;
            ImagePath = imagePath;
            MimeType = mimeType;
            FileName = fileName;
        }

        public ProductImage(Guid id, Guid productId, byte[] blob, string name, string imagePath, string mimeType, string fileName)
        {
            Id = id;
            ProductId = productId;
            Blob = blob;
            Name = name;
            ImagePath = imagePath;
            MimeType = mimeType;
            FileName = fileName;
        }


        public ProductImage( byte[] blob, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("É necessário informar um nome para a imagem do produto.");

            Id = Guid.NewGuid();            
            Blob = blob;
            Name = name;            
            MimeType = $"data:image/png;base64,{Convert.ToBase64String(blob)}";
            FileName = $"imagem-{name}-{Id.ToString()}.png";
        }
        protected ProductImage() { }
    }
}
