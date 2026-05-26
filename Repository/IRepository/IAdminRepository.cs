using MyBook_Backend.Models.DomainModels;

namespace MyBook_Backend.Repository.IRepository
{
    public interface IAdminRepository
    {
        Task<int> GetTotalBooks();

        Task<int> GetTotalUsers();

        Task<int> GetTotalCategories();

        Task<List<Book>>
            GetRecentBooks();

        Task<List<User>>
            GetRecentUsers();
    }
}
