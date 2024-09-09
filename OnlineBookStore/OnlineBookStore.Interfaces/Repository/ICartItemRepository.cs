using OnlineBookStore.Models;

namespace OnlineBookStore.Interfaces.Repository
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetCartItemsByShoppingCartIdAsync(long shoppingCartId);

        Task<IEnumerable<CartItem>> GetCartItemsByBookIdAndShoppingCartId(long bookId, long shoppingCartId);
    }
}
