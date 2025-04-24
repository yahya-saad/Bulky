namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork uow;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }


    public IActionResult Index()
    {
        var categories = uow.Category.GetAll();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category obj)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        uow.Category.Add(obj);
        uow.Save();

        TempData["success"] = "Category created successfully";

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {

        var category = uow.Category.Get(c => c.Id == id);

        if (category is null)
            return NotFound();

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category obj)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        uow.Category.Update(obj);
        uow.Save();

        TempData["success"] = "Category updated successfully";

        return RedirectToAction(nameof(Index));
    }


    public IActionResult Delete(int id)
    {
        var category = uow.Category.Get(c => c.Id == id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int id)
    {
        var category = uow.Category.Get(c => c.Id == id);
        if (category == null) return NotFound();

        uow.Category.Remove(category);
        uow.Save();

        TempData["success"] = "Category deleted successfully";

        return RedirectToAction(nameof(Index));
    }



}
