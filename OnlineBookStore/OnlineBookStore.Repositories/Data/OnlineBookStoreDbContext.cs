using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Models;

namespace OnlineBookStore.Repositories.Data
{
    public class OnlineBookStoreDbContext : DbContext
    {
        public OnlineBookStoreDbContext(DbContextOptions<OnlineBookStoreDbContext> option) : base(option)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                var currentUserADName = GetCurrentADUserName();

                if (entry.State == EntityState.Added)
                {
                    entity.CreateDate = DateTime.UtcNow;
                    entity.CreatedByADName = currentUserADName;
                    entity.Enabled = true;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.ModifyDate = DateTime.UtcNow;
                    entity.ModifyByADName = currentUserADName;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private string GetCurrentADUserName()
        {
            return "user_ad_name";
        }
    }
}
