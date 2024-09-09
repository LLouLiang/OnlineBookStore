using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class CartItem : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public long ShoppingCartId { get; set; }
        public long BookId { get; set; }
        public int Quantity { get; set; }
    }
}
