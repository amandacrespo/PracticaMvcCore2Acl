using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2Acl.Data;
using PracticaMvcCore2Acl.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();

builder.Services.AddControllersWithViews(options => options.EnableEndpointRouting = false)
    .AddSessionStateTempDataProvider();

builder.Services.AddSession();

string connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<LibrosContext>
    (options => options.UseSqlServer(connectionString));

builder.Services.AddTransient<RepositoryLibros>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseRouting();

app.UseAuthorization();

//app.MapStaticAssets();
app.UseSession();
app.UseStaticFiles();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Libros}/{action=Index}/{id?}")
//    .WithStaticAssets();

app.UseMvc(
    routes =>
    {
        routes.MapRoute(
            name: "default",
            template: "{controller=Libros}/{action=Index}/{id?}"
        );
    });

app.Run();
