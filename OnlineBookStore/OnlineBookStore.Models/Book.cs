using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class Book : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
    }
}
