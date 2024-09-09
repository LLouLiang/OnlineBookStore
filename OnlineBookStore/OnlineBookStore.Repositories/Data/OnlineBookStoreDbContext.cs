using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Models;

namespace OnlineBookStore.Repositories.Data
{
    public class OnlineBookStoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public OnlineBookStoreDbContext(DbContextOptions<OnlineBookStoreDbContext> option) : base(option)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Trace> Traces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // configured the identity entities

            // Example for IdentityUserLogin:
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });

            // Example for IdentityUserRole:
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });

            // Example for IdentityUserToken:
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        }

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
            // TO-DO user adname or equvilant from token
            return "modified_user_adname";
        }
    }
}
