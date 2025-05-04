namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppConstants.Roles.Role_Admin)]
public class CompanyController(IUnitOfWork uow) : Controller
{
    public IActionResult Index()
    {
        var companies = uow.Company.GetAll();
        return View(companies);
    }

    public IActionResult Upsert(int? id)
    {

        if (id == null || id == 0)
        {
            // Create Company
            return View(new Company());
        }
        else
        {
            // Update Company
            var company = uow.Company.Get(c => c.Id == id)!;

            if (company == null)
                return NotFound();

            return View(company);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Company obj)
    {
        if (!ModelState.IsValid)
        {
            return View(obj);
        }


        if (obj.Id == 0)
        {
            uow.Company.Add(obj);
            TempData["success"] = "Company created successfully";
        }
        else
        {
            uow.Company.Update(obj);
            TempData["success"] = "Company updated successfully";
        }

        uow.Save();
        return RedirectToAction(nameof(Index));
    }

    #region APIs
    [HttpGet]
    public IActionResult GetAll()
    {
        var companyList = uow.Company.GetAll();
        return Json(new { data = companyList });
    }

    public IActionResult Delete(int id)
    {
        var company = uow.Company.Get(c => c.Id == id);
        if (company == null) return NotFound();

        uow.Company.Remove(company);
        uow.Save();

        return NoContent();
    }

    #endregion

}
