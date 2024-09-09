using OnlineBookStore.Interfaces.Repository;
using OnlineBookStore.Models;
using OnlineBookStore.Repositories.Data;
using OnlineBookStore.Services.DB;

namespace OnlineBookStore.Repositories
{
    public class ShoppingCartRepository: BaseRepository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly OnlineBookStoreDbContext _context;
        private readonly ISqlQueryContext _sqlQueryContext;
        public ShoppingCartRepository(OnlineBookStoreDbContext context, ISqlQueryContext sqlQueryContext) : base(context)
        {
            _context = context;
            _sqlQueryContext = sqlQueryContext;
        }

    }

}
