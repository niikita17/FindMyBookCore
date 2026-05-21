using Microsoft.AspNetCore.Http;
namespace MyBook_Backend.Models.DTO
{
    public class CreateProductDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public string ImgaeUrl { get; set; }
        public int CategoryId { get; set; }
      public IFormFile Image { get; set; }


    }
}
