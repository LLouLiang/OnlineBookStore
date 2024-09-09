using OnlineBookStore.DTOs;

namespace OnlineBookStore.Models
{
    public class CartItemDTO : BaseDTO
    {
        public long Id { get; set; }
        public long ShoppingCartId { get; set; }
        public long BookId { get; set; }
        public int Quantity { get; set; }
    }
}
