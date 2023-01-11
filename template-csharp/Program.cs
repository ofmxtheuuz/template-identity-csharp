using Microsoft.AspNetCore.Identity;
using template_csharp.Database;
using template_csharp.Utils.Services;
using template_csharp.Utils.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// services
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SqlServerDbContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<SqlServerDbContext>();

// d.i.
builder.Services.AddScoped<IAuthService, AuthService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();