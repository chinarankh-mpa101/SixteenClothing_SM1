using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixteenClothing.Contexts;
using SixteenClothing.Models;
using SixteenClothing.ViewModels.ProductViewModels;

namespace SixteenClothing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _folderPath;

        public ProductController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");
        }


        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Select(x => new ProductGetVM
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImagePath = x.ImagePath,
                Price = x.Price,
                Rating = x.Rating,
                CategoryName = x.Category.Name
            }).ToListAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToListAsync();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            var categories = await _context.Categories.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToListAsync();
            ViewBag.Categories = categories;

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
            if (!isExistCategory)
            {
                ModelState.AddModelError("CategoryId", "This category does not exist");
                return View(vm);
            }
            if (vm.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Image's maximum size must be 2 mb");
                return View(vm);
            }
            if (!vm.Image.ContentType.ToLower().Contains("image"))
            {
                ModelState.AddModelError("Image", "You can upload file in only image format");
                return View(vm);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + vm.Image.FileName;
            //string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");
            string path = Path.Combine(_folderPath, uniqueFileName);

            using FileStream stream = new(path, FileMode.Create);
            await vm.Image.CopyToAsync(stream);
            Product product = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                Rating = vm.Rating,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
                ImagePath = uniqueFileName
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
                return NotFound();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            string deletedImagePath = Path.Combine(_folderPath, product.ImagePath);
            if (System.IO.File.Exists(deletedImagePath))
                System.IO.File.Delete(deletedImagePath);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
                return NotFound();

            ProductUpdateVM vm = new()
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Rating = product.Rating
            };
            var categories = await _context.Categories.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToListAsync();
            ViewBag.Categories = categories;
            return View(vm);

        }


        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateVM vm)
        {
            var categories = await _context.Categories.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToListAsync();
            ViewBag.Categories = categories;
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
            if (!isExistCategory)
            {
                ModelState.AddModelError("CategoryId", "This category does not exist");
                return View(vm);
            }

            if (vm.Image.Length > 2 * 1024 * 1024 )
            {
                ModelState.AddModelError("Image", "Image's maximum size must be 2 mb");
                return View(vm);
            }
            if (!vm.Image.ContentType.ToLower().Contains("image"))
            {
                ModelState.AddModelError("Image", "You can upload file in only image format");
                return View(vm);
            }

            var existProduct = await _context.Products.FindAsync(vm.Id);
            if (existProduct is null)
                return BadRequest();
            existProduct.Name = vm.Name;
            existProduct.Description = vm.Description;
            existProduct.Rating = vm.Rating;
            existProduct.CategoryId = vm.CategoryId;
            existProduct.Price = vm.Price;

            if (vm.Image is not null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + vm.Image.FileName;
                //string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");
                string newImagepath = Path.Combine(_folderPath, uniqueFileName);

                using FileStream stream = new(newImagepath, FileMode.Create);
                await vm.Image.CopyToAsync(stream);

                string oldImagePath = Path.Combine(_folderPath, existProduct.ImagePath);
                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
                existProduct.ImagePath = uniqueFileName;
            }
            _context.Products.Update(existProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    

    }
}
