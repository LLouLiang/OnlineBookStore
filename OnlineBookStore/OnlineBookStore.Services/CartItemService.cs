using AutoMapper;
using OnlineBookStore.Interfaces;
using OnlineBookStore.Interfaces.Repository;
using OnlineBookStore.Models;

namespace OnlineBookStore.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IServiceResponse<CartItemDTO>> GetCartItemById(long Id)
        {
            try
            {
                var cartItemEntity = await _unitOfWork.ICartItemRepository.GetByIdAsync(Id);

                await _unitOfWork.CompleteAsync();

                var cartItemDto = _mapper.Map<CartItemDTO>(cartItemEntity);

                return new ServiceResponse<CartItemDTO>(true, "Fetch cart item by Id successfully", "0001", "Fetch cart item by Id successfully", cartItemDto);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<CartItemDTO>(false, ex.Message, "0002", ex.Message, null);
            }
        }

        public async Task<IServiceResponse<IEnumerable<CartItemDTO>>> GetCartItemsByShoppingCartId(long shoppingCartId)
        {
            try
            {
                var cartItemEntities = await _unitOfWork.ICartItemRepository.GetCartItemsByShoppingCartIdAsync(shoppingCartId);

                await _unitOfWork.CompleteAsync();

                var cartItemDtos = _mapper.Map<IEnumerable<CartItemDTO>>(cartItemEntities);

                return new ServiceResponse<IEnumerable<CartItemDTO>>(true, "Fetch cart items by shopping cart Id successfully", "0001", "Fetch cart items by shopping cart Id successfully", cartItemDtos);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<CartItemDTO>>(false, ex.Message, "0002", ex.Message, null);
            }
        }

        public async Task<IServiceResponse<CartItemDTO>> InsertCartItem(CartItemDTO cartItemDto)
        {
            try
            {
                var cartItemEntity = _mapper.Map<CartItem>(cartItemDto);

                var Id = await _unitOfWork.ICartItemRepository.InsertAsync(cartItemEntity);

                await _unitOfWork.CompleteAsync();

                cartItemDto.Id = Id;

                return new ServiceResponse<CartItemDTO>(true, "Cart item added successfully", "0001", "Cart item added successfully", cartItemDto);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<CartItemDTO>(false, ex.Message, "0002", ex.Message, null);
            }
        }

        public async Task<IServiceResponse<CartItemDTO>> UpdateCartItem(CartItemDTO cartItemDto)
        {
            try
            {
                var cartItemEntity = _mapper.Map<CartItem>(cartItemDto);

                _unitOfWork.ICartItemRepository.UpdateAsync(cartItemEntity);

                await _unitOfWork.CompleteAsync();

                return new ServiceResponse<CartItemDTO>(true, "Update cart item successfully", "0001", "Update cart item successfully", cartItemDto);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<CartItemDTO>(false, ex.Message, "0002", ex.Message, null);
            }
        }

        public async Task<IServiceResponse<CartItemDTO>> GetCartItemByBookIdAndShoppingCartId(long bookId, long shoppingCartId)
        {
            try
            {
                var cartItemEntities = await _unitOfWork.ICartItemRepository.GetCartItemsByBookIdAndShoppingCartId(bookId, shoppingCartId);

                await _unitOfWork.CompleteAsync();

                var cartItemDto = _mapper.Map<CartItemDTO>(cartItemEntities.FirstOrDefault());

                return new ServiceResponse<CartItemDTO>(true, "Get cart item by book Id and shopping cart Id successfully", "0001", "Get cart item by book Id and shopping cart Id successfully", cartItemDto);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<CartItemDTO>(false, ex.Message, "0002", ex.Message, null);
            }
        }

        public async Task<IServiceResponse<CartItemDTO>> AddCartItem(CartItemDTO cartItemDto)
        {
            try
            {
                var cartItemResponse = await GetCartItemByBookIdAndShoppingCartId(cartItemDto.BookId, cartItemDto.ShoppingCartId).ConfigureAwait(false);
                if(cartItemResponse == null)
                {
                    return await InsertCartItem(cartItemDto);
                }
                cartItemResponse.ResponseObject.Quantity += cartItemDto.Quantity;
                return await UpdateCartItem(cartItemResponse.ResponseObject);

            }
            catch(Exception ex)
            {
                return new ServiceResponse<CartItemDTO>(false, ex.Message, "0002", ex.Message, null);
            }
        }
    }
}
