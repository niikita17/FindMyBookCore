using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using MyBookBackend.Common.Constants;
using MyBookBackend.Common.DTO;
using MyBookBackend.Common.Interfaces;
using MyBookBackend.Domain.Data;
using MyBookBackend.Domain.DomainModels;
using MyBookBackend.Repository.IRepository;
using MyBookBackend.Service.IServices;

namespace MyBookBackend.Service
{
    public class BookService:IBookService
    {
        public readonly ApplicationDbContext _dbContext;
        private readonly IBookRepository _bookRepository;
        private readonly ICacheService _cacheService;
        private readonly IAuditService _auditService;
        private readonly IAuthService _authService;
        private readonly ILogger<IBookService> _logger;

        public BookService(ApplicationDbContext dbcontext, IBookRepository bookRepository, ICacheService cache, IAuditService auditService,IAuthService authService, ILogger<IBookService> logger)
        {
            _logger = logger;
            _dbContext = dbcontext;
            _bookRepository = bookRepository;
            _cacheService = cache;
            _auditService = auditService;
            _authService = authService;
        }
        public async Task<Result<BookResponseDto>> Create(CreateProductDto book)
        {
           
            var allowedExtensions = new[]
            {
                ".jpg",
                ".jpeg",
                ".png"
            };
            var extension = Path.GetExtension(book.Image.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                return Result<BookResponseDto>.Failure("Somting went wrong");
            }
            
           

            var newBook = await _bookRepository.Create(book);
            
            if (newBook == null)
            {
                return Result<BookResponseDto>.Failure("Somting went wrong");
            }


            var res = new BookResponseDto
            { Id = newBook.Id,
                Title = newBook.Title,
                Description = newBook.Description,
                Price = newBook.Price,
                StockQuantity=newBook.StockQuantity,
                CategoryId =newBook.CategoryId,
                CategoryName = newBook.Category.Name,
                ImageUrl = newBook.ImageUrl
            };

            await _auditService.Log(
     userId: _authService.getLoggedInUserId,
     action: "Create",
     entityName: "Book",
     entityId: newBook.Id,
     description:
         $"Created book {newBook.Title}");

            //remove cache 
            _cacheService.RemoveByPrefix("books");


            return Result<BookResponseDto>.Success(res, "Book Added sucessfully");
        }

        public async Task<Result<Book>> Delete(int Id)
        {
            if(Id == 0 || Id ==null)
            {
                return Result<Book>.Failure("Id is null");
            }

            Book book=await _bookRepository.Delete(Id);
         
            if (book == null)
            {
                return Result<Book>.Failure("Id is null");
            }

            await _auditService.Log(
    userId: _authService.getLoggedInUserId,
    action: "Delete",
    entityName: "Book",
    entityId: book.Id,
    description:
        $"Deleted book {book.Title}");
            _cacheService.Remove(CacheKeys.BookById(Id));

            _cacheService.RemoveByPrefix("books");
            return Result<Book>.Success(book);

        }

        public async Task<Result<BookResponseDto>> EditbyId(int id, CreateProductDto book)
        {

            Book editedBook = await _bookRepository.Edit(id, book);
            if (editedBook == null)
                return Result<BookResponseDto>.Failure("Id is null");
        
            var res = new BookResponseDto
            {
                Id = editedBook.Id,
                Title = editedBook.Title,
                Description = editedBook.Description,
                Price = editedBook.Price,
                StockQuantity = editedBook.StockQuantity,
                CategoryId =editedBook.CategoryId,
                CategoryName = editedBook.Category.Name,
                ImageUrl = editedBook.ImageUrl
            };

            await _auditService.Log(
    userId: _authService.getLoggedInUserId,
    action: "Update",
    entityName: "Book",
    entityId: editedBook.Id,
    description:
        $"Updated book {editedBook.Title}");


            _cacheService.RemoveByPrefix("books");

            _cacheService.Remove(CacheKeys.BookById(id));
            return Result<BookResponseDto>.Success(res, "book edited sucesssfully");


        }
        public async Task<Result<BookResponseDto>>  Get(int id)
        {

            var cacheKey = CacheKeys.BookById(id);

            if (_cacheService.TryGetValue(
                    cacheKey,
                    out Result<BookResponseDto>? cachedBook))
            {
                _logger.LogInformation("Book returned from cache.");

                return cachedBook!;
            }

            var book =
                await _bookRepository.Get(id);

            if (book == null)
            {
                return Result<BookResponseDto>
                    .Failure("Book not found");
            }
        



            var response = new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                StockQuantity = book.StockQuantity,
                ImageUrl = book.ImageUrl,
                CategoryId=book.CategoryId,
                CategoryName = book.Category?.Name
            };

            var result = Result<BookResponseDto>.Success(response);

            _cacheService.Set(
                cacheKey,
                result,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(5)
                });

            return result;
        }

        public async Task<Result<PagedResponseDto<BookResponseDto>>> GetAll(string? search,int? categoryId,int page = 1,int pageSize = 10,string? sortBy = null)
        {
            var cacheKey = CacheKeys.Books(
                search,
                categoryId,
                page,
                pageSize,
                sortBy);

            if (_cacheService.TryGetValue(
      cacheKey,
      out Result<PagedResponseDto<BookResponseDto>>? cachedResult))
            {
                return cachedResult!;
            }

            var query =  _bookRepository.GetAll();


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.Title.Contains(search));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(p =>
                    p.CategoryId == categoryId);
            }
            switch (sortBy)
            {
                case "price_asc":
                    query = query.OrderBy(p => p.Price);
                    break;

                case "price_desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;

                case "title":
                    query = query.OrderBy(p => p.Title);
                    break;

                default:
                    query = query.OrderByDescending(p => p.Id);
                    break;
            }
            var totalCount =  query.Count();

            var items =  query
       .Skip((page - 1) * pageSize)
       .Take(pageSize)
       .Select(b => new BookResponseDto
       {
           Id = b.Id,
           Title = b.Title,
           Price = b.Price,
           ImageUrl = b.ImageUrl,
           CategoryId=b.CategoryId,
           StockQuantity = b.StockQuantity,
           CategoryName = b.Category.Name,
           Description=b.Description
       })
       .ToList(); 
      
            var data = new PagedResponseDto<BookResponseDto>
            {      
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(
                     totalCount / (double)pageSize)
            };

            var result = Result<PagedResponseDto<BookResponseDto>>
                .Success(data);

            _cacheService.Set(
                cacheKey,
                result,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            return result;
        }

      
      
    }
}
