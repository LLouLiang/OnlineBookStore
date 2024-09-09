using OnlineBookStore.DTOs;

namespace OnlineBookStore.Models
{
    public class BookDTO : BaseDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
    }
}
