using Microsoft.EntityFrameworkCore;
using MyBookBackend.Domain.Data;
using MyBookBackend.Domain.DomainModels;
using MyBookBackend.Common.DTO;
using MyBookBackend.Repository.IRepository;

namespace MyBookBackend.Repository
{

    
    public class CartRepository:ICartRepository

    {

        public readonly ApplicationDbContext _dbContext;
        public readonly IBookRepository _bookRepository;
        public CartRepository(ApplicationDbContext dbContext, IBookRepository bookRepository)
        {
            _dbContext = dbContext;
            _bookRepository = bookRepository;
        }
        public async Task<Cart> AddToCart(int userId,AddToCartDto dto)
        {
            


            Cart cart = await _dbContext.Carts .Include(c => c.CartItems) .FirstOrDefaultAsync( c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                await _dbContext.Carts
                    .AddAsync(cart);

                await _dbContext
                    .SaveChangesAsync();
            }

            CartItem existingItem =
                cart.CartItems
                    .FirstOrDefault(
                        x => x.BookId
                            == dto.BookId
                    );

            if (existingItem != null)
            {
                existingItem.Quantity
                    += dto.Quantity;
            }

            else
            {

                var book = await _bookRepository.Get(dto.BookId);

                if (book.StockQuantity < dto.Quantity)
                {
                    throw new Exception("Out of stock");
                }


                CartItem item =
                    new CartItem
                    {
                        CartId = cart.Id,
                        BookId = dto.BookId,
                        Quantity = dto.Quantity
                    };



                await _dbContext
                    .CartItems
                    .AddAsync(item);
            }

            await _dbContext
                .SaveChangesAsync();

            return cart;
        }


        public async Task<Cart> GetCartByUserID(int userId)
        {

            Cart cart = await _dbContext.Carts.Include(c => c.CartItems).Include(c => c.CartItems)
.ThenInclude(ci => ci.Book).FirstOrDefaultAsync(c => c.UserId == userId);

            return cart;
        }


        public async Task<bool>RemoveCartItem(int cartItemId)
        {
            var item =
                await _dbContext.CartItems
                    .FindAsync(cartItemId);

            if (item == null)
            {
                return false;
            }

            _dbContext.CartItems
                .Remove(item);

            await _dbContext
                .SaveChangesAsync();

            return true;
        }

    }
}
