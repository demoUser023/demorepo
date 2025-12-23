using ECommerce.Models;
using ECommerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly IProductRepository _repo;
    private readonly IWebHostEnvironment _env;

    public HomeController(IProductRepository repo, IWebHostEnvironment env)
    {
        _repo = repo;
        _env = env;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Products()
    {
        var products = await _repo.GetAllProducts();

        var vm = products.Select(p => new ProductViewModel
        {
            PId = p.PId,
            PName = p.PName,
            Price = p.Price,
            PImage = p.PImage,
            Product_Desc = p.Product_Desc
        }).ToList();

        return View(vm);
    }
    // GET
    public IActionResult CreateProduct()
    {
        return View(new CreateProductViewModel());
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductViewModel vm)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (vm.PImageFile == null || vm.PImageFile.Length == 0)
            {
                ModelState.AddModelError("PImageFile", "Please upload an image.");
                return View(vm);
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(vm.PImageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await vm.PImageFile.CopyToAsync(stream);

            var product = new Product
            {
                PName = vm.PName,
                Price = vm.Price,
                Product_Desc = vm.Product_Desc,
                PImage = "/images/" + uniqueName
            };

            await _repo.CreateProduct(product);
            TempData["Success"] = "product Added";

            return RedirectToAction("Products");
        }
        catch (Exception ex)
        {
            TempData["debug"] = ex.Message;
            return View(vm);
        }
    }



    // public async Task<IActionResult> DeleteProduct(int id)
    // {
    //     if (id <= 0)
    //         return BadRequest("Invalid product id.");

    //     var deleted = await _repo.DeleteProduct(id);

    //     if (!deleted)
    //         return NotFound("Product not found.");

    //     return RedirectToAction("Products");
    // }

    public async Task<IActionResult> DeleteAllProducts()
    {
        var deleted = await _repo.Deleteallproducts();

        if (!deleted)
            return NotFound("Product not found.");

        return RedirectToAction("Products");
    }

    public async Task<IActionResult> EditProduct(int id)
    {
        if (id <= 0) return BadRequest("Invalid Product Id");

        var product = await _repo.GetAProduct(id);
        if (product == null) return NotFound("Product Not found");

        // Map entity to ViewModel
        var vm = new ProductViewModel
        {
            PId = product.PId,
            PName = product.PName,
            Price = product.Price,
            Product_Desc = product.Product_Desc
        };

        return View(vm); // Pass ViewModel to the view
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(ProductViewModel vm)
    {
    
        ModelState.Remove("PImage");
        ModelState.Remove("PImageFile");


        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine("MODEL ERROR: " + error.ErrorMessage);
            }
            TempData["Error"] = "Cannot edit the data" + vm.PId;
            return View(vm);
        }
        var productedit = new Product
        {
            PId = vm.PId,
            PName = vm.PName,
            Price = vm.Price,
            Product_Desc = vm.Product_Desc

        };
        if (await _repo.EditProduct(productedit))
        {

            return RedirectToAction("Products");

        }
        else
        {
            return NotFound("NO such product");
        }

    }

}
