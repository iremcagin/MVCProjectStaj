using Microsoft.AspNetCore.Authentication.Cookies;
using myProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache(); // Required for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set timeout as needed
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
    options.Cookie.IsEssential = true; // Make the session cookie essential for the application
});




// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SellerPolicy", policy => policy.RequireRole("Seller"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});





string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<_LoginDatabaseControlModel>(provider =>
    new _LoginDatabaseControlModel(connectionString)); 
builder.Services.AddScoped<_UserDatabaseControlModel>(provider =>
    new _UserDatabaseControlModel(connectionString)); 
builder.Services.AddScoped<_AdminDatabaseControlModel>(provider =>
    new _AdminDatabaseControlModel(connectionString)); 
builder.Services.AddScoped<_SellerDatabaseControlModel>(provider =>
    new _SellerDatabaseControlModel(connectionString)); 







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

// Add session middleware
app.UseSession();


// Kimlik Doðrulama ve Yetkilendirme Middleware'lerini Kullanma
app.UseAuthentication();
app.UseAuthorization();




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Guest}/{action=Index}/{id?}");

app.Run();
