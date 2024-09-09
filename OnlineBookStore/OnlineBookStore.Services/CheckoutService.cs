using OnlineBookStore.Interfaces;

namespace OnlineBookStore.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IBookService _bookService;

        public CheckoutService(IShoppingCartService shoppingCartService, IBookService bookService) 
        {
            _shoppingCartService = shoppingCartService;
            _bookService = bookService;
        }
        public async Task<IServiceResponse<string>> CalculateTotalAsync(long shoppingCartId)
        {
            var shoppingCartResponse = await _shoppingCartService.GetShoppingCartById(shoppingCartId).ConfigureAwait(false);
            if(shoppingCartResponse.ResponseObject == null)
            {
                return new ServiceResponse<string>(false, "Shopping cart does not exist", "0002", "Shopping cart does not exist", null);
            }
            var totalAmount = decimal.Zero;
            var cartItemDtos = shoppingCartResponse.ResponseObject.CartItems;
            if(cartItemDtos == null)
            {
                return new ServiceResponse<string>(true, "Get shopping cart items calculated successfully", "0001", "Get shopping cart items calculated successfully", totalAmount.ToString());
            }
            var bookIds = cartItemDtos.Select(ci => ci.Id).ToList();
            var bookDtos = (await _bookService.GetBooksByIds(bookIds).ConfigureAwait(false)).ResponseObject;
            
            foreach(var cartItemDto in cartItemDtos)
            {
                var priceRaw = bookDtos?.FirstOrDefault(b => b.Id == cartItemDto.BookId)?.Price;
                var quantity = cartItemDto.Quantity;
                var price = string.IsNullOrEmpty(priceRaw) ? decimal.Zero : Convert.ToDecimal(priceRaw);
                
                totalAmount += (quantity * price);
            }
            return new ServiceResponse<string>(true, "Get shopping cart items calculated successfully", "0001", "Get shopping cart items calculated successfully", decimal.Round(totalAmount, 2).ToString());
        }
    }
}
