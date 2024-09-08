using OnlineBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Interfaces
{
    public interface IShoppingCartService
    {
        Task AddToCartAsync(int bookId, int quantity);
        Task<IEnumerable<ShoppingCart>> GetCartItemsAsync();
        Task ClearCartAsync();
    }
}
