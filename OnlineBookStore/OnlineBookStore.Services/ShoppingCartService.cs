using AutoMapper;
using OnlineBookStore.Interfaces;
using OnlineBookStore.Interfaces.Repository;
using OnlineBookStore.Models;

namespace OnlineBookStore.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartItemService _cartItemService;
        public ShoppingCartService(IMapper mapper, IUnitOfWork unitOfWork, ICartItemService cartItemService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cartItemService = cartItemService;
        }

        public async Task<IServiceResponse<ShoppingCartDTO>> InsertShoppingCart(ShoppingCartDTO shoppingCartDto)
        {
            try
            {
                var ShoppingCartEntity = _mapper.Map<ShoppingCart>(shoppingCartDto);

                var Id = await _unitOfWork.IShoppingCartRepository.InsertAsync(ShoppingCartEntity);

                await _unitOfWork.CompleteAsync();

                shoppingCartDto.Id = Id;

                return new ServiceResponse<ShoppingCartDTO>(true, "Shopping cart added successfully", "0001", "Shopping cart added successfully", shoppingCartDto);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ShoppingCartDTO>(false, ex.Message, "0002", ex.Message, null);
            }
        }

        public async Task<IServiceResponse<ShoppingCartDTO>> GetShoppingCartById(long id)
        {
            try
            {
                var shoppingCartDB = await _unitOfWork.IShoppingCartRepository.GetByIdAsync(id);

                await _unitOfWork.CompleteAsync();

                var shoppingCartDto = _mapper.Map<ShoppingCartDTO>(shoppingCartDB);

                var cartItems = (await _cartItemService.GetCartItemsByShoppingCartId(id).ConfigureAwait(false)).ResponseObject;

                shoppingCartDto.CartItems = cartItems;

                return new ServiceResponse<ShoppingCartDTO>(true, "Fetch shopping cart by Id successfully", "0001", "Fetch shopping cart by Id successfully", shoppingCartDto);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ShoppingCartDTO>(false, ex.Message, "0002", ex.Message, null);
            }
        }

        public async Task<IServiceResponse<ShoppingCartDTO>> UpdateShoppingCart(ShoppingCartDTO shoppingCartDto)
        {
            try
            {
                var shoppingCartEntity = _mapper.Map<ShoppingCart>(shoppingCartDto);

                _unitOfWork.IShoppingCartRepository.UpdateAsync(shoppingCartEntity);

                await _unitOfWork.CompleteAsync();

                return new ServiceResponse<ShoppingCartDTO>(true, "Update shopping cart successfully", "0001", "Update shopping cart successfully", shoppingCartDto);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ShoppingCartDTO>(false, ex.Message, "0002", ex.Message, null);
            }
        }

        public async Task<IServiceResponse<ShoppingCartDTO>> AddBookToShoppingCart(long shoppingCartId, long bookId, int quantity)
        {
            var shoppingCartResponse = await GetShoppingCartById(shoppingCartId).ConfigureAwait(false);
            if (shoppingCartResponse == null)
            {
                return new ServiceResponse<ShoppingCartDTO>(false, "Shopping cart does not exist, add item to shopping cart failed", "0001", "Shopping cart does not exist, add item to shopping cart failed", null);
            }
            CartItemDTO cartItemDto = new()
            {
                ShoppingCartId = shoppingCartId,
                BookId = bookId,
                Quantity = quantity
            };
            await _cartItemService.AddCartItem(cartItemDto).ConfigureAwait(false);
            var shoppingCartItems = (await _cartItemService.GetCartItemsByShoppingCartId(shoppingCartId).ConfigureAwait(false)).ResponseObject;
            var shoppingCartDto = shoppingCartResponse.ResponseObject;
            shoppingCartDto.CartItems = shoppingCartItems;
            return new ServiceResponse<ShoppingCartDTO>(true, "Add book into shopping cart successfully", "0001", "Add book into shopping cart successfully", shoppingCartDto);
        }
    }
}
