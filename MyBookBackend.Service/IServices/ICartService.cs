using Microsoft.AspNetCore.Mvc;
using MyBookBackend.Domain.DomainModels;
using MyBookBackend.Common.DTO;

namespace MyBookBackend.Service.IServices
{
    public interface ICartService
    {
        Task<Result<string>>AddToCart( int userId,AddToCartDto dto);

        public Task<Result<List<CartItemResponseDto>>> GetCartByUser(int id);

        public  Task<Result<bool>> Remove(int id);
      
    }
}
