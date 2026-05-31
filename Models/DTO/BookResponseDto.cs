using MyBook_Backend.Models.DomainModels;

namespace MyBook_Backend.Models.DTO
{
    public class BookResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int StockQuantity { get; set; }
        public int Price { get; set; }
        public int CategoryId {  get; set; }

        public string CategoryName { get; set; }
       

        public string ImageUrl { get; set; } = null;
    }
}
