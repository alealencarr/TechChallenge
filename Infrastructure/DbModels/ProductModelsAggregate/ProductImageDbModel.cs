namespace Infrastructure.DbModels.ProductModelsAggregate
{
    public class ProductImageDbModel
    {
        public Guid ProductId { get; set; }
        public ProductDbModel Product { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public byte[] Blob { get; set; } = default!;
        public Guid Id { get; set; }
        public ProductImageDbModel(Guid id, Guid productId, byte[] blob, string name, string imagePath, string mimeType, string fileName)
        {
            Id = id;
            ProductId = productId;
            Blob = blob;
            Name = name;
            ImagePath = imagePath;
            MimeType = mimeType;
            FileName = fileName;
        }
        protected ProductImageDbModel() { }
    }
}
