using Microsoft.AspNetCore.Mvc;
using MyBookBackend.Domain.DomainModels;
using MyBookBackend.Common.DTO;
using MyBookBackend.Repository.IRepository;
using MyBookBackend.Service.IServices;

namespace MyBookBackend.Service
{
    public class CartService:ICartService
    {

        public readonly IBookRepository _bookRepository;
        public readonly ICartRepository _cartRepository;

        public CartService(IBookRepository bookRepository, ICartRepository cartRepository)
        {
            _bookRepository = bookRepository;
            _cartRepository= cartRepository;
        }


       public async Task<Result<string>>AddToCart(   int userId,    AddToCartDto dto)

        {


            Book book =
                await _bookRepository.Get(dto.BookId);

            if (book == null)
            {
                return Result<string>
           .Failure(
              "Book not found");
            }


            if (book.StockQuantity
                < dto.Quantity)
            {
                throw new Exception(
                    "Not enough stock"
                );
            }

           
            var cart =
                await _cartRepository
                    .AddToCart(
                        userId,
                        dto
                    );

            if (cart == null)
            {
                return Result<string>
                    .Failure(
                        "Book not found"
                    );
            }

            return Result<string>
                .Success(
                    "Added",
                    "Book added to cart"
                );
        }

        public async Task<Result<List<CartItemResponseDto>>> GetCartByUser(int id)
        {
            if (id == null)
                return Result<List<CartItemResponseDto>>.Failure("id id null");

            var cart = await _cartRepository.GetCartByUserID(id);
            if (cart == null)
            {
                return new Result<List<CartItemResponseDto>>();
            }
            var result =
                 cart.CartItems.Select(x =>
            new CartItemResponseDto
            
            {
                Id=x.Id,
            BookId = x.BookId,
            Title = x.Book.Title,
            Quantity = x.Quantity,
            Price = x.Book.Price,
            ImageUrl = x.Book.ImageUrl
        }).ToList();

            return Result<List<CartItemResponseDto>>.Success(result, "cart items");


        }


        public async Task<Result<bool>> Remove(int id)
        {
            if (id == null)
                return null;
            var res = await _cartRepository.RemoveCartItem(id);

            if(res)
                return Result<bool>.Success(true, "cart removed sucessfully");

            return Result<bool>.Failure( "cart could not be removed ");

        }
    }
}
