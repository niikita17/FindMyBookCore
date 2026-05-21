using Microsoft.Identity.Client;
using MyBook_Backend.Data;
using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Models.DTO;
using MyBook_Backend.Repository.IRepository;
using MyBook_Backend.Services.IServices;
using static System.Reflection.Metadata.BlobBuilder;

namespace MyBook_Backend.Services
{
    public class BookService:IBookService
    {
        public readonly ApplicationDbContext _dbContext;
        private readonly IBookRepository _bookRepository;



        public BookService(ApplicationDbContext dbcontext, IBookRepository bookRepository)
        {
           
            _dbContext = dbcontext;
            _bookRepository = bookRepository;   
        }
        public async Task<Result<BookResponseDto>> Create(CreateProductDto book)
        {
            if (book.Image == null || book.Image.Length <= 0)
            {
                return Result<BookResponseDto>.Failure("Image is null");
            }
            var allowedExtensions = new[]
            {
                "jpg",
                ".jpeg",
                ".png"
            };
            var extension = Path.GetExtension(book.Image.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                throw new Exception("Only jpg, jpeg, png allowed");
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
                CategoryName = newBook.Category.Name,
                ImageUrl = newBook.ImageUrl
            };

           
         
           return Result<BookResponseDto>.Success(res, "Book Added sucessfully");
        }

        public async Task<Result<Book>> Delete(int Id)
        {
            if(Id == 0 || Id ==null)
            {
                return Result<Book>.Failure("Id is null");
            }

            Book book=await _bookRepository.Delete(Id);
            if(book == null)
            {
                 throw new Exception("Something went wrong while deleting");
            }
            return Result<Book>.Success(book);

        }

        public async Task<Result<BookResponseDto>> EditbyId(int Id, CreateProductDto book)
        {

            Book editedBook = await _bookRepository.Edit(Id, book);
            if (editedBook == null)
                 throw new Exception("someting went wrong");

            var res = new BookResponseDto
            {
                Id = editedBook.Id,
                Title = editedBook.Title,
                Description = editedBook.Description,
                Price = editedBook.Price,
                CategoryName = editedBook.Category.Name,
                ImageUrl = editedBook.ImageUrl
            };
            return Result<BookResponseDto>.Success(res, "book edited sucesssfully");


        }
       

        public async Task<Result<BookResponseDto>> Get(int Id)
        {
           var book=await _bookRepository.Get(Id);
            var result = new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                CategoryName = book.Category.Name,
                ImageUrl = book.ImageUrl
            };

            return Result<BookResponseDto>.Success(result, "sucess");
        }

        public async Task<Result<PagedResponseDto<BookResponseDto>>> GetAll(string? search,int? categoryId,int page = 1,int pageSize = 10,string? sortBy = null)
        {
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
