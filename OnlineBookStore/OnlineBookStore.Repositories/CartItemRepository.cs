using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Interfaces.Repository;
using OnlineBookStore.Models;
using OnlineBookStore.Repositories.Data;
using OnlineBookStore.Services.DB;

namespace OnlineBookStore.Repositories
{
    public class CartItemRepository : BaseRepository<CartItem>, ICartItemRepository
    {
        private readonly OnlineBookStoreDbContext _context;
        private readonly ISqlQueryContext _sqlQueryContext;
        public CartItemRepository(OnlineBookStoreDbContext context, ISqlQueryContext sqlQueryContext) : base(context)
        {
            _context = context;
            _sqlQueryContext = sqlQueryContext;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByShoppingCartIdAsync(long shoppingCartId)
        {
            var query = _sqlQueryContext.GetCartItemsByShoppingCartId();
            var param = new SqliteParameter("@ShoppingCartId", shoppingCartId);
            return await _context.CartItems.FromSqlRaw(query, param).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByBookIdAndShoppingCartId(long bookId, long shoppingCartId)
        {
            var query = _sqlQueryContext.GetCartItemsByBookIdAndShoppingCartId();
            var paramBookId = new SqliteParameter("@BookId", bookId);
            var paramShoppingCartId = new SqliteParameter("@ShoppingCartId", shoppingCartId);
            return await _context.CartItems.FromSqlRaw(query, paramBookId, paramShoppingCartId).AsNoTracking().ToListAsync();
        }
    }
}
