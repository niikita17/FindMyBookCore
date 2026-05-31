using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Models.DTO;

namespace MyBook_Backend.Repository.IRepository
{
    public interface ICartRepository
    {
        Task<Cart> AddToCart( int userId, AddToCartDto dto);
        Task<Cart> GetCartByUserID(int userId );
        Task<bool> RemoveCartItem(int cartItemId);
    }
}
