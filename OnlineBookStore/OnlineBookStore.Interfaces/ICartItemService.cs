using OnlineBookStore.Models;

namespace OnlineBookStore.Interfaces
{
    public interface ICartItemService
    {
        Task<IServiceResponse<CartItemDTO>> InsertCartItem(CartItemDTO cartItemDto);

        Task<IServiceResponse<CartItemDTO>> GetCartItemById(long Id);

        Task<IServiceResponse<IEnumerable<CartItemDTO>>> GetCartItemsByShoppingCartId(long shoppingCartId);

        Task<IServiceResponse<CartItemDTO>> UpdateCartItem(CartItemDTO cartItemDto);

        Task<IServiceResponse<CartItemDTO>> AddCartItem(CartItemDTO cartItemDto);

        Task<IServiceResponse<CartItemDTO>> GetCartItemByBookIdAndShoppingCartId(long bookId, long shoppingCartId);
    }
}
