using Microsoft.EntityFrameworkCore;
using Operativka.Data;
using Operativka.Data.Seed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Operativka.Areas.Identity.Models;
using Operativka.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var identityConnectionString = builder.Configuration.GetConnectionString("OperativkaIdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'OperativkaIdentityContextConnection' not found.");

builder.Services.AddDbContext<OperativkaIdentityContext>(options =>
    options.UseSqlServer(identityConnectionString));;

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<OperativkaIdentityContext>();;

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<OperativkaIdentityContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();

var dataConnectionString = builder.Configuration.GetConnectionString("OperativkaContext") ?? throw new InvalidOperationException("Connection string 'OperativkaContext' not found.");

builder.Services.AddDbContext<OperativkaContext>(options =>
    options.UseSqlServer(dataConnectionString));;

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    //app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    //Seeds the main db
    try
    {
        var context = services.GetRequiredService<OperativkaContext>();
        context.Database.EnsureCreated();
        DataSeeder.ExecuteSeed(context);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the main DB.");
    }
    
    //Seeds the identity db
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var seeder = new ContextSeeder(builder.Configuration);
        await seeder.SeedRolesAsync(userManager, roleManager);
        await seeder.SeedSuperAdminAsync(userManager, roleManager);
        await seeder.SeedAdminAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the identity DB.");
    }
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
