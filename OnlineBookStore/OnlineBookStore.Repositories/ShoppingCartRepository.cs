using OnlineBookStore.Interfaces.Repository;
using OnlineBookStore.Models;
using OnlineBookStore.Repositories.Data;
using OnlineBookStore.Services.DB;

namespace OnlineBookStore.Repositories
{
    public class ShoppingCartRepository: BaseRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(OnlineBookStoreDbContext context) : base(context)
        {
        }
    }
    
}
