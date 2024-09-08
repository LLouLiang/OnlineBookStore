using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;
var environment = builder.Environment;

// Add services to the container.
// Open the SQLite in-memory connection
var connection = new SqliteConnection("DataSource=:memory:");
connection.Open();

// Register the DbContext with the open SQLite connection
builder.Services.AddDbContext<OnlineBookStoreDbContext>(options =>
{
    options.UseSqlite(connection);
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Register Services
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();

builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

builder.Services.AddScoped<ICheckoutService, CheckoutService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

builder.Services.AddScoped<ISqlQueryContext, SqlQueryContext>();

var app = builder.Build();

// Create scope to access services
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OnlineBookStoreDbContext>();

    // Open the connection to the in-memory SQLite database
    dbContext.Database.OpenConnection();

    // Ensure the schema is created
    dbContext.Database.EnsureCreated();

    // Seed the database with some provision data
    SeedData(dbContext);

    Console.WriteLine("Database seeded successfully.");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.UseAuthentication();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.UseHttpsRedirection();

app.MapControllers();

app.Run();

static void SeedData(OnlineBookStoreDbContext dbContext)
{
    // Add some sample data to the Books table
    if (!dbContext.Books.Any())
    {
        dbContext.Books.AddRange(
            new Book { Title = "Book 1", Author = "Author 1", Price = "9.99", Category = "Fiction", CreatedByADName = "llyu", CreateDate = DateTime.UtcNow, Enabled = true },
            new Book { Title = "Book 2", Author = "Author 2", Price = "14.99", Category = "Science", CreatedByADName = "llyu", CreateDate = DateTime.UtcNow, Enabled = true },
            new Book { Title = "Book 3", Author = "Author 3", Price = "19.99", Category = "Technology", CreatedByADName = "llyu", CreateDate = DateTime.UtcNow, Enabled = true }
        );
        dbContext.SaveChanges();
    }

    // You can seed other entities similarly
}