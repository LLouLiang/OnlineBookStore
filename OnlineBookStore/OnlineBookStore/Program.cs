var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    // Add the "Authorization" header to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

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

// Configure ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<OnlineBookStoreDbContext>()
    .AddDefaultTokenProviders();

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

builder.Services.AddScoped<ICartItemService, CartItemService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();

builder.Services.AddScoped<ISqlQueryContext, SqlQueryContext>();

// jwt configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Add OpenTelemetry tracing
builder.Services.AddOpenTelemetry().WithTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OnlineBookStore.WebAPI"))
        .AddAspNetCoreInstrumentation()  // Instrument ASP.NET Core requests
        .AddHttpClientInstrumentation()  // Instrument outgoing HTTP requests
        .AddConsoleExporter()            // Export traces to the console
        .AddProcessor(new BatchActivityExportProcessor(new SQLiteTraceExporter(
            builder.Services.BuildServiceProvider().GetRequiredService<OnlineBookStoreDbContext>())));
});

var app = builder.Build();

// Create scope to access services
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = scope.ServiceProvider.GetRequiredService<OnlineBookStoreDbContext>();

    // Open the connection to the in-memory SQLite database
    dbContext.Database.OpenConnection();

    // Ensure the schema is created
    dbContext.Database.EnsureCreated();

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Seed the database with some provision data
    await SeedDataAsync(dbContext, userManager, roleManager);

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
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.UseHttpsRedirection();

app.MapControllers();

app.Run();

static async Task SeedDataAsync(OnlineBookStoreDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
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

    if (!dbContext.ShoppingCarts.Any())
    {
        dbContext.ShoppingCarts.AddRange(
            new ShoppingCart { Owner = "llyu", CreatedByADName = "llyu", CreateDate = DateTime.UtcNow, Enabled = true }
        );
        dbContext.SaveChanges();
    }

    if (!dbContext.CartItems.Any())
    {
        dbContext.CartItems.AddRange(
            new CartItem { ShoppingCartId = 1, BookId = 1, Quantity = 1, CreatedByADName = "llyu", CreateDate = DateTime.UtcNow, Enabled = true }
        );
        dbContext.SaveChanges();
    }

    // Seed Roles
    if (!roleManager.Roles.Any())
    {
        await roleManager.CreateAsync(new IdentityRole("Administrator"));
        await roleManager.CreateAsync(new IdentityRole("Moderator"));
        await roleManager.CreateAsync(new IdentityRole("User"));
    }

    // Seed Default User
    var defaultUser = new ApplicationUser
    {
        UserName = "admin",
        Email = "admin@example.com",
        FirstName = "Admin",
        LastName = "User",
        EmailConfirmed = true,
        PhoneNumberConfirmed = true
    };

    if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
    {
        var result = await userManager.CreateAsync(defaultUser, "Admin@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(defaultUser, "Administrator");
        }
    }

    if (!dbContext.Traces.Any())
    {
        dbContext.Traces.AddRange(
            new OnlineBookStore.Models.Trace { TraceId = "trace1", SpanId = "span1", ParentSpanId = "Parent span", OperationName = "Test Operation 1", Status = "Status", StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddSeconds(1) },
            new OnlineBookStore.Models.Trace { TraceId = "trace2", SpanId = "span2", ParentSpanId = "Parent span", OperationName = "Test Operation 2", Status = "Status", StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddSeconds(2) }
        );
        dbContext.SaveChanges();
    }
}