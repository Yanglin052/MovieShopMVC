using ApplicationCore.Contracts.Repository;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Entities;
using Infrastructure.Data;
using Infrastructure.Repository;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MovieShopMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ICastService, CastService>();
builder.Services.AddScoped<ICastRepository, CastRepository>();
builder.Services.AddScoped<IRepository<Genre>, Repository<Genre>>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICurrentLogedInUser, CurrentLogedInUser>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRepository<Favorite>, Repository<Favorite>>();
builder.Services.AddScoped<IRepository<Purchase>, Repository<Purchase>>();
builder.Services.AddScoped<IRepository<Review>, Repository<Review>>();


// Inject HttpContext for IHttpContextAccessor interface
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<MovieShopDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieShopDbConnection"));
});

// set cookie based authentication information
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(
         options =>
         {
             options.Cookie.Name = "MovieShopAuthCookie";
             options.ExpireTimeSpan = TimeSpan.FromHours(2);
             options.LoginPath = "/account/login";
         }                                                                                                                                                           
      );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
