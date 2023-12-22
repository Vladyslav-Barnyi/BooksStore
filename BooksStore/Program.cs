using BooksStore.Repositories;
using BooksStore.Repositories.Interfaces;
using BooksStore.Services;
using BooksStore.Services.Interfaces;
using BooksStoreEntities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var service = builder.Services;

service.AddControllers()
    .AddNewtonsoftJson();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("BooksStoreEntities")
    )
);

service.AddScoped<IUserService, UserService>();
service.AddScoped<IAuthorService, AuthorService>();
service.AddScoped<IBookService, BookService>();
service.AddScoped<IOrderService, OrderService>();
service.AddScoped<IShoppingCartService, ShoppingCartService>();
service.AddScoped<IGenreService, GenreService>();
service.AddScoped<IUserRepository, UserRepository>();
service.AddScoped<IAuthorRepository, AuthorRepository>();
service.AddScoped<IGenreRepository, GenreRepository>();
service.AddScoped<IBookRepository, BookRepository>();
service.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
service.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
