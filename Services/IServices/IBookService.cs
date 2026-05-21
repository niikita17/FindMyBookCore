using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Models.DTO;

namespace MyBook_Backend.Services.IServices
{
    public interface IBookService
    {
        public Task<Result<BookResponseDto>> Create(CreateProductDto book);
        public Task<Result<PagedResponseDto<BookResponseDto>>> GetAll(string? search, int? categoryId, int page = 1, int pageSize = 10, string? sortBy = null);
    
 
        public Task<Result<BookResponseDto>> EditbyId(int Id, CreateProductDto book);

        public Task<Result<Book>> Delete(int Id);
        public Task<Result<BookResponseDto>> Get(int Id);

    }
}
