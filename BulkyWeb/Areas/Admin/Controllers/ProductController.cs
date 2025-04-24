using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork uow;

    public ProductController(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }


    public IActionResult Index()
    {
        var products = uow.Product.GetAll();
        return View(products);
    }

    public IActionResult Create()
    {
        var categoryList = uow.Category
            .GetAll()
            .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

        var productVM = new ProductVM
        {
            Product = new(),
            CategoryList = categoryList,
        };

        return View(productVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ProductVM obj)
    {
        if (!ModelState.IsValid)
        {
            obj.CategoryList = uow.Category
                .GetAll()
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();

            return View(obj);
        }

        uow.Product.Add(obj.Product);
        uow.Save();

        TempData["success"] = "Product created successfully";

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {

        var product = uow.Product.Get(c => c.Id == id);

        if (product is null)
            return NotFound();

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Product obj)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        uow.Product.Update(obj);
        uow.Save();

        TempData["success"] = "Product updated successfully";

        return RedirectToAction(nameof(Index));
    }


    public IActionResult Delete(int id)
    {
        var product = uow.Product.Get(c => c.Id == id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int id)
    {
        var product = uow.Product.Get(c => c.Id == id);
        if (product == null) return NotFound();

        uow.Product.Remove(product);
        uow.Save();

        TempData["success"] = "Product deleted successfully";

        return RedirectToAction(nameof(Index));
    }


}
