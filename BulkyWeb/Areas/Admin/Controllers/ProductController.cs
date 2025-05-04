using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppConstants.Roles.Role_Admin)]
public class ProductController : Controller
{
    private readonly IUnitOfWork uow;
    private readonly IWebHostEnvironment webHostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        uow = unitOfWork;
        this.webHostEnvironment = webHostEnvironment;
    }


    public IActionResult Index()
    {
        var products = uow.Product.GetAll(includeProperties: "Category");
        return View(products);
    }

    public IActionResult Upsert(int? id)
    {
        var productVM = new ProductVM
        {
            Product = new(),
            CategoryList = uow.Category.GetAll()
            .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList(),
        };

        if (id == null || id == 0)
        {
            // Create Product
            return View(productVM);
        }
        else
        {
            // Update Product
            productVM.Product = uow.Product.Get(c => c.Id == id)!;

            if (productVM.Product == null)
                return NotFound();

            return View(productVM);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM obj)
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


        if (obj.Image != null)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
            string productPath = Path.Combine(webHostEnvironment.WebRootPath, "images", "products");

            // Delete old image if it's an update
            if (!string.IsNullOrEmpty(obj.Product.ImageUrl) && obj.Product.Id != 0)
            {
                string oldImagePath = Path.Combine(productPath, obj.Product.ImageUrl);
                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);

            }

            string savePath = Path.Combine(productPath, fileName);
            using var fileStream = new FileStream(savePath, FileMode.Create);
            obj.Image.CopyTo(fileStream);

            obj.Product.ImageUrl = fileName;
        }

        if (obj.Product.Id == 0)
        {
            uow.Product.Add(obj.Product);
            TempData["success"] = "Product created successfully";
        }
        else
        {
            uow.Product.Update(obj.Product);
            TempData["success"] = "Product updated successfully";
        }

        uow.Save();
        return RedirectToAction(nameof(Index));
    }

    #region APIs
    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = uow.Product.GetAll(includeProperties: "Category");
        return Json(new { data = productList });
    }

    public IActionResult Delete(int id)
    {
        var product = uow.Product.Get(c => c.Id == id);
        if (product == null) return NotFound();

        if (!string.IsNullOrEmpty(product.ImageUrl))
        {
            string productPath = Path.Combine(webHostEnvironment.WebRootPath, "images", "products");
            string oldImagePath = Path.Combine(productPath, product.ImageUrl);
            if (System.IO.File.Exists(oldImagePath))
                System.IO.File.Delete(oldImagePath);

        }

        uow.Product.Remove(product);
        uow.Save();

        return NoContent();
    }

    #endregion

}
