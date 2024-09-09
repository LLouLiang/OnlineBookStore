using OnlineBookStore.Interfaces.Repository;
using OnlineBookStore.Repositories.Data;

namespace OnlineBookStore.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnlineBookStoreDbContext _context;
        public IBookRepository IBookRepository { get; private set; }
        public IShoppingCartRepository IShoppingCartRepository { get; private set; }
        public ICartItemRepository ICartItemRepository { get; private set; }

        public UnitOfWork(OnlineBookStoreDbContext context, IBookRepository bookRepository, IShoppingCartRepository shoppingCartRepository, ICartItemRepository cartItemRepository)
        {
            IBookRepository = bookRepository;
            IShoppingCartRepository = shoppingCartRepository;
            ICartItemRepository = cartItemRepository;
            _context = context;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
