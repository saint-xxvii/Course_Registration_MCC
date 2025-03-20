
using Microsoft.EntityFrameworkCore;
using sample_1.Data; // Replace with your actual namespace
using Microsoft.AspNetCore.Authentication.Cookies;
using sample_1.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
// Adding email service
builder.Services.AddSingleton<EmailService>();

// Add the database context and configure SQL Server connection.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add authentication using cookies.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        //ithelam options uh
        options.LoginPath = "/Login"; // Redirect to login page if not authenticated
        options.LogoutPath = "/Logout"; // Redirect to logout page
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie expiration time
        options.AccessDeniedPath = "/AccessDenied"; // Redirect to access denied page
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // Enforce HTTP Strict Transport Security.
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add authentication middleware.
app.UseAuthentication(); // Ensure this comes before UseAuthorization.

// Add authorization middleware.
app.UseAuthorization();
app.MapGet("/", (HttpContext context) =>
{
    if (!context.User.Identity.IsAuthenticated)
    {
        return Results.Redirect("/Login"); // Redirect to the login page if not authenticated
    }

    return Results.Redirect("/Index"); // Redirect to the home page if authenticated
});
app.MapRazorPages();



app.Run();
