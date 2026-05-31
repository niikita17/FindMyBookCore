using Microsoft.Identity.Client;
using MyBook_Backend.Data;
using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Models.DTO;
using MyBook_Backend.Repository.IRepository;
using MyBook_Backend.Services.IServices;
using Microsoft.Extensions.Caching.Memory;

namespace MyBook_Backend.Services
{
    public class BookService:IBookService
    {
        public readonly ApplicationDbContext _dbContext;
        private readonly IBookRepository _bookRepository;
        private readonly IMemoryCache _cache;
        private readonly IAuditService _auditService;
        private readonly IAuthService _authService;


        public BookService(ApplicationDbContext dbcontext, IBookRepository bookRepository, IMemoryCache cache, IAuditService auditService,IAuthService authService)
        {
           
            _dbContext = dbcontext;
            _bookRepository = bookRepository;
            _cache = cache;
            _auditService = auditService;
            _authService = authService;
        }
        public async Task<Result<BookResponseDto>> Create(CreateProductDto book)
        {
            if (book.Image == null || book.Image.Length <= 0)
            {
                return Result<BookResponseDto>.Failure("Image is null");
            }
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
            _cache.Remove("all_books");
            _cache.Remove("admin_dashboard");
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

            return Result<BookResponseDto>.Success(res, "Book Added sucessfully");
        }

        public async Task<Result<Book>> Delete(int Id)
        {
            if(Id == 0 || Id ==null)
            {
                return Result<Book>.Failure("Id is null");
            }

            Book book=await _bookRepository.Delete(Id);
            _cache.Remove("all_books");
            _cache.Remove("admin_dashboard");
            _cache.Remove($"book_{Id}");
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
            return Result<Book>.Success(book);

        }

        public async Task<Result<BookResponseDto>> EditbyId(int id, CreateProductDto book)
        {

            Book editedBook = await _bookRepository.Edit(id, book);
            if (editedBook == null)
                return Result<BookResponseDto>.Failure("Id is null");
            _cache.Remove("all_books");
            _cache.Remove($"book_{id}");
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
            return Result<BookResponseDto>.Success(res, "book edited sucesssfully");


        }
        public async Task<Result<BookResponseDto>>  Get(int id)
        {

            string cacheKey = $"book_{id}";

            if (_cache.TryGetValue(
                cacheKey,
                out BookResponseDto b))
            {
                return Result<BookResponseDto>
                    .Success(b, "sucess");
            }


            var book =
                await _bookRepository.Get(id);

            if (book == null)
            {
                return Result<BookResponseDto>
                    .Failure("Book not found");
            }
            _cache.Set( cacheKey, book, TimeSpan.FromMinutes(10));


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

            return Result<BookResponseDto>
                .Success(response);
        }

        public async Task<Result<PagedResponseDto<BookResponseDto>>> GetAll(string? search,int? categoryId,int page = 1,int pageSize = 10,string? sortBy = null)
        {

            const string cacheKey = "all_books";


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


            return Result<PagedResponseDto<BookResponseDto>>.Success(data);
        }

      
      
    }
}
