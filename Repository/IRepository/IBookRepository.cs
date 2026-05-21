using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Models.DTO;

namespace MyBook_Backend.Repository.IRepository
{
    public interface IBookRepository
    {
        public Task<Book> Create(CreateProductDto book);
      public   IQueryable<Book> GetAll();



	
        public Task<Book> Edit(int Id, CreateProductDto model);

        public Task<Book> Delete(int Id);
        public Task<Book> Get(int Id);


        public Task<string> uploadImage(IFormFile Image, string oldFile);

    }



}

