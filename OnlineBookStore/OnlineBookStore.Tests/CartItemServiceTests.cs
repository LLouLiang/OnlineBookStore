using AutoMapper;
using Moq;
using OnlineBookStore.Interfaces.Repository;
using OnlineBookStore.Models;
using OnlineBookStore.Services;

namespace OnlineBookStore.Tests
{
    public class CartItemServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICartItemRepository> _cartItemRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CartItemService _cartItemService;

        public CartItemServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cartItemRepositoryMock = new Mock<ICartItemRepository>();
            _mapperMock = new Mock<IMapper>();

            // Set up the unit of work to return the mocked repository
            _unitOfWorkMock.Setup(uow => uow.ICartItemRepository).Returns(_cartItemRepositoryMock.Object);

            // Create an instance of CartItemService with the mocks
            _cartItemService = new CartItemService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetCartItemById_ShouldReturnSuccess_WhenCartItemExists()
        {
            // Arrange
            var cartItem = new CartItem { Id = 1 };
            _cartItemRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(cartItem);
            _mapperMock.Setup(m => m.Map<CartItemDTO>(cartItem)).Returns(new CartItemDTO { Id = 1 });

            // Act
            var result = await _cartItemService.GetCartItemById(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Fetch cart item by Id successfully", result.Message);
            _cartItemRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _mapperMock.Verify(m => m.Map<CartItemDTO>(cartItem), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCartItemById_ShouldReturnFailure_WhenExceptionIsThrown()
        {
            // Arrange
            _cartItemRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<long>())).ThrowsAsync(new Exception("Get failed"));

            // Act
            var result = await _cartItemService.GetCartItemById(1);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Get failed", result.Message);
            _cartItemRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task GetCartItemsByShoppingCartId_ShouldReturnSuccess_WhenItemsExist()
        {
            // Arrange
            var cartItems = new List<CartItem> { new CartItem { Id = 1, ShoppingCartId = 1 } };
            _cartItemRepositoryMock.Setup(repo => repo.GetCartItemsByShoppingCartIdAsync(1)).ReturnsAsync(cartItems);
            _mapperMock.Setup(m => m.Map<IEnumerable<CartItemDTO>>(cartItems))
                       .Returns(new List<CartItemDTO> { new CartItemDTO { Id = 1, ShoppingCartId = 1 } });

            // Act
            var result = await _cartItemService.GetCartItemsByShoppingCartId(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Fetch cart items by shopping cart Id successfully", result.Message);
            _cartItemRepositoryMock.Verify(repo => repo.GetCartItemsByShoppingCartIdAsync(1), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<CartItemDTO>>(cartItems), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task InsertCartItem_ShouldReturnSuccess_WhenCartItemIsAdded()
        {
            // Arrange
            var cartItemDto = new CartItemDTO { ShoppingCartId = 1 };
            var cartItem = new CartItem { ShoppingCartId = 1 };
            _mapperMock.Setup(m => m.Map<CartItem>(cartItemDto)).Returns(cartItem);
            _cartItemRepositoryMock.Setup(repo => repo.InsertAsync(cartItem)).ReturnsAsync(1);

            // Act
            var result = await _cartItemService.InsertCartItem(cartItemDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Cart item added successfully", result.Message);
            _mapperMock.Verify(m => m.Map<CartItem>(cartItemDto), Times.Once);
            _cartItemRepositoryMock.Verify(repo => repo.InsertAsync(cartItem), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateCartItem_ShouldReturnSuccess_WhenCartItemIsUpdated()
        {
            // Arrange
            var cartItemDto = new CartItemDTO { Id = 1, Quantity = 2 };
            var cartItem = new CartItem { Id = 1, Quantity = 2 };
            _mapperMock.Setup(m => m.Map<CartItem>(cartItemDto)).Returns(cartItem);

            // Act
            var result = await _cartItemService.UpdateCartItem(cartItemDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Update cart item successfully", result.Message);
            _cartItemRepositoryMock.Verify(repo => repo.UpdateAsync(cartItem), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddCartItem_ShouldInsertNewItem_WhenItemDoesNotExist()
        {
            // Arrange
            var cartItemDto = new CartItemDTO { BookId = 1, ShoppingCartId = 1, Quantity = 1, CreatedByADName = "llyu", CreateDate = DateTime.UtcNow, Enabled = true };
            _cartItemRepositoryMock.Setup(repo => repo.GetCartItemsByBookIdAndShoppingCartId(1, 1))
                                   .ReturnsAsync(Enumerable.Empty<CartItem>());
            _mapperMock.Setup(m => m.Map<CartItem>(cartItemDto)).Returns(new CartItem());
            _cartItemRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<CartItem>())).ReturnsAsync(1);

            // Act
            var result = await _cartItemService.AddCartItem(cartItemDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Cart item added successfully", result.Message);
            _cartItemRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<CartItem>()), Times.Once);
            _cartItemRepositoryMock.Verify(repo => repo.GetCartItemsByBookIdAndShoppingCartId(1, 1), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.AtLeastOnce);
        }

        [Fact]
        public async Task AddCartItem_ShouldUpdateQuantity_WhenItemAlreadyExists()
        {
            // Arrange
            var cartItemDto = new CartItemDTO { BookId = 1, ShoppingCartId = 1, Quantity = 1, CreatedByADName = "llyu", CreateDate = DateTime.UtcNow, Enabled = true };
            var existingCartItem = new CartItem { Id = 1, BookId = 1, ShoppingCartId = 1, Quantity = 1, CreatedByADName = "llyu", CreateDate = DateTime.UtcNow, Enabled = true };
            _cartItemRepositoryMock.Setup(repo => repo.GetCartItemsByBookIdAndShoppingCartId(1, 1))
                                   .ReturnsAsync(new List<CartItem> { existingCartItem });
            _mapperMock.Setup(m => m.Map<CartItemDTO>(existingCartItem))
                       .Returns(new CartItemDTO { Id = 1, BookId = 1, ShoppingCartId = 1, Quantity = 1 });

            // Act
            var result = await _cartItemService.AddCartItem(cartItemDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Update cart item successfully", result.Message);
            _cartItemRepositoryMock.Verify(repo => repo.GetCartItemsByBookIdAndShoppingCartId(1, 1), Times.Once);
            _cartItemRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<CartItem>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.AtLeastOnce);
        }
    }
}
