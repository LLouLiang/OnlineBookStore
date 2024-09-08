using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class ShoppingCart : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
