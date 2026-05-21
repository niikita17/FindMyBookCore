using Microsoft.EntityFrameworkCore;
using MyBook_Backend.Data;
using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Models.DTO;
using MyBook_Backend.Repository.IRepository;

namespace MyBook_Backend.Repository
{
    public class BookRepository : IBookRepository
    {
        public readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

      

        public BookRepository(ApplicationDbContext dbcontext, IWebHostEnvironment environment)
        {
            _environment = environment;
            _dbContext = dbcontext;

        }

        public async Task<string> uploadImage( IFormFile image, string oldfile=null)

        {
          
            var extension = Path.GetExtension(image.FileName).ToLower();
          
            var fileName = Guid.NewGuid().ToString()
                         + extension;

            var folderPath = Path.Combine(
             _environment.WebRootPath,
             "images",
             "books");
            if(oldfile!=null)
            {
                string oldfilePath = Path.Combine(_environment.WebRootPath, "images", fileName);

                if (System.IO.File.Exists(oldfilePath))
                {
                    System.IO.File.Delete(oldfilePath);
                }
            }


            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return fileName;
        }


        public async Task<Book> Create(CreateProductDto book)
        {


            var fileName = uploadImage(book.Image);

            var newbook = new Book
            {
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                CategoryId = book.CategoryId,
                ImageUrl = $"images/products/{fileName}"
            };

           
            await _dbContext.Books.AddAsync(newbook);
            await _dbContext.SaveChangesAsync();

            return newbook;
        }

        public async Task<Book> Delete(int Id)
        {
            Book book = await _dbContext.Books.FindAsync(Id);
            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();
            return book;

        }

        public async Task<Book> Edit(int Id, CreateProductDto model)
        {

            Book existingBook =await  _dbContext.Books.FindAsync(Id);
            string fileName = "";
            if (model.Image == null)
            {
                 fileName = await uploadImage(model.Image);
            }
             fileName =await  uploadImage(model.Image, existingBook.ImageUrl);
            if (fileName == null)
                throw new ArgumentNullException();

            existingBook.CategoryId = model.CategoryId;
            existingBook.Title = model.Title;
            existingBook.Price = model.Price;
            existingBook.Description = model.Description;
            existingBook.ImageUrl = $"images/products/{fileName}";
            await _dbContext.Books.AddAsync(existingBook);
            await _dbContext.SaveChangesAsync();

            return existingBook;

        }

     

        public async Task<Book> Get(int Id)
        {
            Book book = await _dbContext.Books.FindAsync(Id);
            return book;
        }

        public IQueryable<Book> GetAll()
        {
            return _dbContext.Books
                .Include(b => b.Category)
                .AsQueryable();
        }

     

    }
}
