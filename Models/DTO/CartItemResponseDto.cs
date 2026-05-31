namespace MyBook_Backend.Models.DTO
{
    public class CartItemResponseDto
    {
        public int Id {  get; set; }
        public int BookId { get; set; }

        public string Title { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public string ImageUrl { get; set; }
    }
}
