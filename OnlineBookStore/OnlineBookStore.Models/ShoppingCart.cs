using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class ShoppingCart : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string Owner { get; set; }
    }
}
