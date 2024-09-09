using OnlineBookStore.DTOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    public class ShoppingCartDTO : BaseDTO
    {
        public long Id { get; set; }
        public string Owner { get; set; }
        [NotMapped]
        public IEnumerable<CartItemDTO> CartItems { get; set; } 
    }
}
