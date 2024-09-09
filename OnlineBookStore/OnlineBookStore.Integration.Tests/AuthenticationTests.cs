using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using OnlineBookStore.DTOs;
using OnlineBookStore.Models;
using OnlineBookStore.Repositories.Data;
using System.Text;
using System.Text.Json;

namespace OnlineBookStore.Integration.Tests
{
    public class AuthenticationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public AuthenticationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace the DbContext with a SQLite in-memory database
                    services.Remove(services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<OnlineBookStoreDbContext>)));

                    services.AddDbContext<OnlineBookStoreDbContext>(options =>
                    {
                        options.UseSqlite("DataSource=:memory:");
                    });

                    // Replace the existing Identity services with new test implementations
                    services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<OnlineBookStoreDbContext>()
                            .AddDefaultTokenProviders();

                    // Build the service provider
                    var sp = services.BuildServiceProvider();

                    // Initialize the database and seed data
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<OnlineBookStoreDbContext>();
                        var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                        db.Database.OpenConnection(); // Keeps the SQLite connection alive
                        db.Database.EnsureCreated(); // Ensures that the database schema is created

                        SeedData(db, userManager, roleManager).Wait();
                    }
                });
            });

            _client = _factory.CreateClient();
        }

        private async Task SeedData(OnlineBookStoreDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed roles
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
            }

            // Seed default user
            var defaultUser = new ApplicationUser
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                FirstName = "Test",
                LastName = "User",
                EmailConfirmed = true
            };

            if (await userManager.FindByNameAsync(defaultUser.UserName) == null)
            {
                var result = await userManager.CreateAsync(defaultUser, "Test@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "User");
                }
            }
        }

        // Test for user registration
        [Fact]
        public async Task RegisterUser_ShouldReturnOk()
        {
            var registerDto = new RegisterDTO
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "NewUser@123",
                FirstName = "New",
                LastName = "User"
            };

            var jsonContent = JsonSerializer.Serialize(registerDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/auth/register", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("User registered successfully", responseString);
        }

        // Test for user login
        [Fact]
        public async Task LoginUser_ShouldReturnJwtToken()
        {
            var loginDto = new LoginDTO
            {
                Username = "testuser",
                Password = "Test@123"
            };

            var jsonContent = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/auth/login", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Token", responseString); // Verify the token is present in the response
        }
    }
}
