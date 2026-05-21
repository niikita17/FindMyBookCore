using Microsoft.EntityFrameworkCore;
using MyBook_Backend.Models.DomainModels;

namespace MyBook_Backend.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base (options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Book> Books { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "Admin" },
                new Role { Id = 2, RoleName = "Customer" }
            );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id=1,
                    Name = "Admin user1",
                    Password = "Admin123",
                    Email = "admin@gmail.com",
                    RoleId = 1,
                    MobileNo = "123456789"
                },
                   new User
                   {
                       Id=2,
                       Name = "Customer user1",
                       Password = "Customer123",
                       Email = "Customer@gmail.com",
                       RoleId = 2,
                       MobileNo = "123456789"
                   });

            modelBuilder.Entity<Category>().HasData(
    new Category { Id = 1, Name = "Story" },
    new Category { Id = 2, Name = "History" },
    new Category { Id = 3, Name = "Horror" }
);

            modelBuilder.Entity<Book>().HasData(

                   // ================= STORY =================
                   new Book
                   {
                       Id = 1,
                       Title = "The Alchemist",
                       Description = "A journey of dreams and destiny",
                       Price = 399,
                       CategoryId = 1,
                       ImageUrl = "images/books/alchemist.jpg"
                   },

                   new Book
                   {
                       Id = 2,
                       Title = "Harry Potter",
                       Description = "Fantasy story of a young wizard",
                       Price = 599,
                       CategoryId = 1,
                       ImageUrl = "images/books/harrypotter.jpg"
                   }, new Book
                   {
                       Id = 3,
                       Title = "The Little Prince",
                       Description = "Classic philosophical story",
                       Price = 299,
                       CategoryId = 1,
                       ImageUrl = "images/books/littleprince.jpg"
                   },

        // ================= HISTORY =================
        new Book
        {
            Id = 4,
            Title = "Sapiens",
            Description = "History of humankind",
            Price = 699,
            CategoryId = 2,
            ImageUrl = "images/books/sapiens.jpg"
        },
          new Book
          {
              Id = 5,
              Title = "Guns, Germs, and Steel",
              Description = "Historical development of civilizations",
              Price = 799,
              CategoryId = 2,
              ImageUrl = "images/books/gunsgermssteel.jpg"
          },

        new Book
        {
            Id = 6,
            Title = "The Silk Roads",
            Description = "A new history of the world",
            Price = 650,
            CategoryId = 2,
            ImageUrl = "images/books/silkroads.jpg"
        },
         new Book
         {
             Id = 7,
             Title = "Dracula",
             Description = "Classic vampire horror novel",
             Price = 450,
             CategoryId = 3,
             ImageUrl = "images/books/dracula.jpg"
         },

        new Book
        {
            Id = 8,
            Title = "It",
            Description = "Terrifying horror by Stephen King",
            Price = 899,
            CategoryId = 3,
            ImageUrl = "images/books/it.jpg"
        },

        new Book
        {
            Id = 9,
            Title = "The Shining",
            Description = "Psychological horror thriller",
            Price = 750,
            CategoryId = 3,
            ImageUrl = "images/books/shining.jpg"
        });
    }
    }
}
