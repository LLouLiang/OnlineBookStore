using OnlineBookStore.Models;

namespace OnlineBookStore.Interfaces
{
    public interface IShoppingCartService
    {
        Task<IServiceResponse<ShoppingCartDTO>> AddBookToShoppingCart(long shoppingCartId, long bookId, int quantity);

        Task<IServiceResponse<ShoppingCartDTO>> InsertShoppingCart(ShoppingCartDTO shoppingCartDto);

        Task<IServiceResponse<ShoppingCartDTO>> UpdateShoppingCart(ShoppingCartDTO shoppingCartDto);

        Task<IServiceResponse<ShoppingCartDTO>> GetShoppingCartById(long id);
    }
}
