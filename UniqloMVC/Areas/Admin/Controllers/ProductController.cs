using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniqloMVC.DataAccess;
using UniqloMVC.Extensions;
using UniqloMVC.Models;
using UniqloMVC.ViewModels.Product;

namespace UniqloMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(IWebHostEnvironment _env,UniqloDbContext _context) : Controller
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
            if(vm.CoverFile != null)
            {
             if (!vm.CoverFile.IsValidType("image"))
                ModelState.AddModelError("CoverFile", "File type must be an image");
             if(!vm.CoverFile.IsValidSize(300))
                    ModelState.AddModelError("CoverFile", "File type must be less than 300kb");

            }

            if (!ModelState.IsValid) return View();
            Product product = vm;
            product.CoverImage=await vm.CoverFile!.UploadAsync(_env.WebRootPath,"imgs","products");
           
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return View();
        }
    }
}
