using Microsoft.EntityFrameworkCore;
using MyBook_Backend.Data;
using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Models.DTO;
using MyBook_Backend.Repository.IRepository;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
namespace MyBook_Backend.Repository
{
    public class BookRepository : IBookRepository
    {
        public readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly Cloudinary _cloudinary;



        public BookRepository(ApplicationDbContext dbcontext, IWebHostEnvironment environment, IConfiguration config)
        {
            _environment = environment;
            _dbContext = dbcontext;
            var account = new Account(
        config["Cloudinary:CloudName"],
        config["Cloudinary:ApiKey"],
        config["Cloudinary:ApiSecret"]);


            _cloudinary = new Cloudinary(account);

        }

        //    public async Task<string> uploadImage( IFormFile image, string oldfile=null)

        //    {

        //        var extension = Path.GetExtension(image.FileName).ToLower();

        //        var fileName = Guid.NewGuid().ToString() 
        //                     + extension;

        //        var folderPath = Path.Combine(
        //         _environment.WebRootPath,
        //         "images",
        //         "books");

        //        if(oldfile!=null)
        //        {
        //            string oldfilePath =
        //Path.Combine(
        //    _environment.WebRootPath,
        //    oldfile
        //);

        //            if (System.IO.File.Exists(oldfilePath))
        //            {
        //                System.IO.File.Delete(oldfilePath);
        //            }
        //        }


        //        if (!Directory.Exists(folderPath))
        //        {
        //            Directory.CreateDirectory(folderPath);
        //        }
        //        var filePath = Path.Combine(folderPath, fileName);
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await image.CopyToAsync(stream);
        //        }
        //        return fileName;
        //    }

        //with cloudnary
        public async Task<CloudinaryUploadResultDto> uploadImage(
    IFormFile image,
    string oldFile = null)
        {
            if (image == null || image.Length == 0)
                return null;

            using var stream =
                image.OpenReadStream();

            var uploadParams =
                new ImageUploadParams
                {
                    File = new FileDescription(
                        image.FileName,
                        stream),

                    Folder = "FindMyBook"
                };

            var uploadResult =
                await _cloudinary.UploadAsync(
                    uploadParams);

            return new CloudinaryUploadResultDto
            {
                Url = uploadResult.SecureUrl.ToString(),
                PublicId = uploadResult.PublicId
            };
        }

       public async Task DeleteImageFromCloudinary(
    string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return;

            var deleteParams =
                new DeletionParams(publicId);

            await _cloudinary.DestroyAsync(
                deleteParams);
        }
        public async Task<Book> Create(CreateProductDto book)
        {


            var uploaded =
     await uploadImage(book.Image);

            var newbook = new Book
            {
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                CategoryId = book.CategoryId,
                StockQuantity = book.StockQuantity,
                ImageUrl = uploaded.Url,
                CloudinaryPublicId =
        uploaded.PublicId
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
            //if (!string.IsNullOrEmpty(book.ImageUrl))
            //{
            //    string imagePath =
            //        Path.Combine(
            //            _environment.WebRootPath,
            //            book.ImageUrl
            //        );

            //    if (System.IO.File.Exists(imagePath))
            //    {
            //        System.IO.File.Delete(imagePath);
            //    }
            //}
            await DeleteImageFromCloudinary(
    book.CloudinaryPublicId);
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
                await DeleteImageFromCloudinary(
                    existingBook.CloudinaryPublicId);

                var uploaded =
                    await uploadImage(model.Image);

                existingBook.ImageUrl =
                    uploaded.Url;

                existingBook.CloudinaryPublicId =
                    uploaded.PublicId;
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
