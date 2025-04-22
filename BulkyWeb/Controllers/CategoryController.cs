namespace BulkyWeb.Controllers;

public class CategoryController(ApplicationDbContext db) : Controller
{
    public IActionResult Index()
    {
        var categories = db.Categories.ToList();
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

        db.Add(obj);
        db.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {

        var category = db.Categories.Find(id);

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

        db.Update(obj);
        db.SaveChanges();

        return RedirectToAction(nameof(Index));
    }


    public IActionResult Delete(int id)
    {
        var category = db.Categories.Find(id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int id)
    {
        var category = db.Categories.Find(id);
        if (category == null) return NotFound();

        db.Remove(category);
        db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }



}
