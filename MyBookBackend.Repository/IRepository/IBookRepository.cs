using MyBookBackend.Domain.DomainModels;
using MyBookBackend.Common.DTO;
using Microsoft.AspNetCore.Http;

namespace MyBookBackend.Repository.IRepository
{
    public interface IBookRepository
    {
        public Task<Book> Create(CreateProductDto book);
      public   IQueryable<Book> GetAll();



	
        public Task<Book> Edit(int Id, CreateProductDto model);

        public Task<Book> Delete(int Id);
        public Task<Book> Get(int Id);


        public Task<CloudinaryUploadResultDto> uploadImage(IFormFile Image, string oldFile);
        public  Task DeleteImageFromCloudinary(string publicId);
    }



}

