using Microsoft.EntityFrameworkCore;
using MyBook_Backend.Data;
using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Repository.IRepository;

public class AdminRepository
    : IAdminRepository
{
    private readonly ApplicationDbContext
        _dbContext;

    public AdminRepository(
        ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int>
        GetTotalBooks()
    {
        return await _dbContext.Books.CountAsync();
    }

    public async Task<int>
        GetTotalUsers()
    {
        return await _dbContext.Users
            .CountAsync();
    }

    public async Task<int>
        GetTotalCategories()
    {
        return await _dbContext.Categories
            .CountAsync();
    }

    public async Task<List<Book>>
        GetRecentBooks()
    {
        return await _dbContext.Books
            .OrderByDescending(
                b => b.Id)
            .Take(5)
            .ToListAsync();
    }

    public async Task<List<User>>
        GetRecentUsers()
    {
        return await _dbContext.Users
            .OrderByDescending(
                u => u.Id)
            .Take(5)
            .ToListAsync();
    }
}