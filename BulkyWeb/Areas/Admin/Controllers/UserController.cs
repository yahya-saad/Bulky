using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppConstants.Roles.Role_Admin)]
public class UserController : Controller
{
    private readonly ApplicationDbContext dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        this.dbContext = dbContext;
        _userManager = userManager;
    }


    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RoleManagement(string userId)
    {
        var roleId = dbContext.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;

        var viewModel = new RoleManagementVM()
        {
            User = dbContext.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == userId),
            Roles = dbContext.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name,
                Selected = r.Id == roleId
            }).ToList(),
            Companies = dbContext.Companies.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList(),
            Role = dbContext.Roles.FirstOrDefault(r => r.Id == roleId).Name
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RoleManagement(RoleManagementVM roleManagementVM)
    {
        var userId = roleManagementVM.User.Id;

        // Get the user
        var applicationUser = await dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
        if (applicationUser == null)
        {
            return NotFound();
        }

        var currentRoleId = dbContext.UserRoles.FirstOrDefault(ur => ur.UserId == userId)?.RoleId;

        var oldRole = dbContext.Roles.FirstOrDefault(r => r.Id == currentRoleId)?.Name;
        var newRole = roleManagementVM.Role;

        // If role is changing
        if (oldRole != newRole)
        {
            if (oldRole != null)
            {
                await _userManager.RemoveFromRoleAsync(applicationUser, oldRole);
            }

            await _userManager.AddToRoleAsync(applicationUser, newRole);
        }

        // Handle CompanyId logic for "Company" role
        if (newRole == AppConstants.Roles.Role_Company)
        {
            applicationUser.CompanyId = roleManagementVM.User.CompanyId;
        }
        else if (oldRole == AppConstants.Roles.Role_Company && newRole != AppConstants.Roles.Role_Company)
        {
            applicationUser.CompanyId = null;
        }

        await dbContext.SaveChangesAsync();

        TempData["success"] = "User role updated successfully.";
        return RedirectToAction("Index");
    }




    #region API
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        var users = await dbContext.ApplicationUsers.Where(u => u.Id != userId).Include(u => u.Company).ToListAsync();
        var userRoles = await dbContext.UserRoles.ToListAsync();
        var roles = await dbContext.Roles.ToListAsync();

        var response = new List<UserVM>();

        foreach (var user in users)
        {
            // Get the role name
            var roleId = userRoles.FirstOrDefault(ur => ur.UserId == user.Id)?.RoleId;
            var roleName = roles.FirstOrDefault(r => r.Id == roleId)?.Name ?? "";

            // Set default company name if null
            var companyName = user.Company?.Name ?? "";

            response.Add(new UserVM
            {
                Id = user.Id,
                Name = user?.UserName ?? "",
                Email = user?.Email ?? "",
                PhoneNumber = user?.PhoneNumber ?? "",
                Company = companyName,
                Role = roleName,
                IsLocked = user.LockoutEnd != null
            });
        }

        return Json(new { data = response });
    }


    [HttpPost]
    public async Task<IActionResult> ToggleLock([FromBody] string id)
    {
        var user = await dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return Json(new { success = false, message = "Error while locking/unlocking" });

        user.LockoutEnd = user.LockoutEnd is null ? DateTime.Now.AddYears(1000) : null; // ربنا يبارك فعمر اليوزر
        await dbContext.SaveChangesAsync();
        return Json(new { success = true, message = "Operation successful" });

    }
    #endregion
}