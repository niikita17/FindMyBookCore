using MyBookBackend.Domain.DomainModels;
using MyBookBackend.Common.DTO;

namespace MyBookBackend.Repository.IRepository
{
    public interface ICartRepository
    {
        Task<Cart> AddToCart( int userId, AddToCartDto dto);
        Task<Cart> GetCartByUserID(int userId );
        Task<bool> RemoveCartItem(int cartItemId);
    }
}
