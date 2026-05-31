using Microsoft.AspNetCore.Mvc;
using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Models.DTO;

namespace MyBook_Backend.Services.IServices
{
    public interface ICartService
    {
        Task<Result<string>>AddToCart( int userId,AddToCartDto dto);

        public Task<Result<List<CartItemResponseDto>>> GetCartByUser(int id);

        public  Task<Result<bool>> Remove(int id);
      
    }
}
