using Microsoft.AspNetCore.Http;

namespace MyBookBackend.Common.DTO
{
    public class EditProductDto
    {   public int Id{get; set;}
		public string Title { get; set; }
		public string Description { get; set; }
		public int Price { get; set; }
        public int StockQuantity { get; set; }

        public string? ImageUrl { get; set; }
		public int CategoryId { get; set; }
		public IFormFile Image { get; set; }
	}
}
