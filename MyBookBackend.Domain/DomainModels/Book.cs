namespace MyBookBackend.Domain.DomainModels
{
    public class Book
    {
        public int  Id { get; set; }
        public string Title { get; set; }
         public string Description { get; set; }

        public int StockQuantity { get; set; } = 1;
        public int Price { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string ImageUrl { get; set; } = null;
        public string? CloudinaryPublicId { get; set; }


    }
}
