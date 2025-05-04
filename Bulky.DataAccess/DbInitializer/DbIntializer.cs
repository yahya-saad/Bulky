using BulkyBook.DataAccess.Data;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.DbInitializer;
public class DbIntializer : IDbinitializer
{
    private readonly ApplicationDbContext _db;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public DbIntializer(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _roleManager = roleManager;
        _userManager = userManager;
    }
    public async Task Initialize()
    {
        // migrate the database
        try
        {
            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
            throw;
        }

        // seed roles
        if (!_roleManager.RoleExistsAsync(AppConstants.Roles.Role_Customer).GetAwaiter().GetResult())
        {
            await _roleManager.CreateAsync(new IdentityRole(AppConstants.Roles.Role_Customer));
            await _roleManager.CreateAsync(new IdentityRole(AppConstants.Roles.Role_Admin));
            await _roleManager.CreateAsync(new IdentityRole(AppConstants.Roles.Role_Company));
            await _roleManager.CreateAsync(new IdentityRole(AppConstants.Roles.Role_Employee));


            // create admin user
            var admin = new ApplicationUser
            {
                UserName = "Admin",
                Email = "admin@bulky.com",
                Name = "Admin",
                PhoneNumber = "1234567890",
                StreetAddress = "123 Admin St",
                City = "Admin City",
                State = "Admin State",
                PostalCode = "12345",
                EmailConfirmed = true
            };
            await _userManager.CreateAsync(admin, "Admin@1234");
            await _userManager.AddToRoleAsync(admin, AppConstants.Roles.Role_Admin);
        }
    }
}