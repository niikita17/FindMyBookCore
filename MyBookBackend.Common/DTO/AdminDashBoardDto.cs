namespace MyBookBackend.Common.DTO
{
    public class AdminDashBoardDto
    {
        public int TotalBooks { get; set; }

        public int TotalUsers { get; set; }

        public int TotalCategories { get; set; }

        public List<BookResponseDto>
            RecentBooks
        { get; set; }

        public List<UserResponseDto>
            RecentUsers
        { get; set; }

    }
}
