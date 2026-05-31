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
                string oldfilePath =
    Path.Combine(
        _environment.WebRootPath,
        oldfile
    );

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


            var fileName = await uploadImage(book.Image);

            var newbook = new Book
            {
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                CategoryId = book.CategoryId,
                StockQuantity = book.StockQuantity,
                ImageUrl = $"images/books/{fileName}"
            };

           
            await _dbContext.Books.AddAsync(newbook);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Entry(newbook)
       .Reference(b => b.Category)
       .LoadAsync();

            return newbook;
        }

        public async Task<Book> Delete(int Id)
        {
            Book book = await _dbContext.Books
                    .FindAsync(Id);

            if (book == null)
            {
                return null;
            }

            // DELETE IMAGE
            if (!string.IsNullOrEmpty(book.ImageUrl))
            {
                string imagePath =
                    Path.Combine(
                        _environment.WebRootPath,
                        book.ImageUrl
                    );

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _dbContext.Books.Remove(book);

            await _dbContext.SaveChangesAsync();

            return book;
        }

        public async Task<Book> Edit(int Id, CreateProductDto model)
        {

            Book existingBook = await _dbContext.Books
    .Include(b => b.Category)
    .FirstOrDefaultAsync(b => b.Id == Id);
            string fileName = "";
            if (model.Image != null)
            {
                fileName = await uploadImage(model.Image, existingBook.ImageUrl);
                 existingBook.ImageUrl = $"images/books/{fileName}";

            }
            
       

            existingBook.CategoryId = model.CategoryId;
            existingBook.Title = model.Title;
            existingBook.Price = model.Price;
            existingBook.StockQuantity = model.StockQuantity;
            existingBook.Description = model.Description;
           

            await _dbContext.SaveChangesAsync();

        

            return existingBook;
        }



        public async Task<Book?> Get(int id)
        {
            return await _dbContext.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public IQueryable<Book> GetAll()
        {
            return _dbContext.Books
                .Include(b => b.Category)
                .AsQueryable();
        }

     

    }
}
