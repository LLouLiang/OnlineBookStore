using OnlineBookStore.Interfaces;
using OnlineBookStore.Models;

namespace OnlineBookStore.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        public Task AddToCartAsync(int bookId, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task ClearCartAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ShoppingCart>> GetCartItemsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
