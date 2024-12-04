using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniqloMVC.DataAccess;
using UniqloMVC.Extensions;
using UniqloMVC.Models;
using UniqloMVC.ViewModels.Product;
using UniqloMVC.ViewModels.Slider;

namespace UniqloMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(IWebHostEnvironment _env, UniqloDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (vm.CoverFile != null)
            {
                if (!vm.CoverFile.IsValidType("image"))
                    ModelState.AddModelError("CoverFile", "File type must be an image");
                if (!vm.CoverFile.IsValidSize(300))
                    ModelState.AddModelError("CoverFile", "File type must be less than 300kb");

            }

            if (!ModelState.IsValid) return View();
            Product product = vm;
            product.CoverImage = await vm.CoverFile!.UploadAsync(_env.WebRootPath, "imgs", "products");

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return View();
        }

        public async Task<IActionResult> Update(int? id)
        {

            if (!id.HasValue) return BadRequest();

            var data = await _context.Products.FindAsync(id);

            if (data is null) return NotFound();

            ProductUpdateVM vm = new();

            vm.Name = data.Name;
            vm.Description = data.Description;
            vm.CostPrice = data.CostPrice;
            vm.SellPrice = data.SellPrice;
            vm.Discount = data.Discount;
            vm.Quantity = data.Quantity;
            vm.CategoryId = data.CategoryId;
            vm.CoverFileUrl = data.CoverImage;

            //Turkan copy eleme bu setri yaz!!!
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Products.FindAsync(id);

            if (data is null) return NotFound();


            if (vm.CoverFile != null)
            {
                if (!vm.CoverFile.IsValidType("image"))
                    ModelState.AddModelError("File", "File must be image!");
                if (!vm.CoverFile.IsValidSize(5 * 1024))
                    ModelState.AddModelError("File", "File length must be less than 2mg");

                string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), _env.WebRootPath, "imgs", "products", data.CoverImage);

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                string newFileName = await vm.CoverFile.UploadAsync(_env.WebRootPath, "imgs", "products");
                data.CoverImage = newFileName;
            }


            if (!ModelState.IsValid)
            {

                ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
                return View(vm);
            }

            data.Name = vm.Name;
            data.Description = vm.Description;
            data.Discount = vm.Discount;
            data.Quantity = vm.Quantity;
            data.CostPrice = vm.CostPrice;
            data.SellPrice = vm.SellPrice;
            data.CategoryId = vm.CategoryId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Products.FindAsync(id);

            if (data is null) return NotFound();

            string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs", "products", data.CoverImage);

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

            _context.Products.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
