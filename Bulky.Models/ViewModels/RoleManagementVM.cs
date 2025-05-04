using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Models.ViewModels;
public class RoleManagementVM
{

    public ApplicationUser User { get; set; }
    public List<SelectListItem> Roles { get; set; }
    public List<SelectListItem> Companies { get; set; }
    public int? CompanyId { get; set; }
    public string Role { get; set; }

}
