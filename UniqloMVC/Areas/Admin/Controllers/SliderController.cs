﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniqloMVC.DataAccess;
using UniqloMVC.Models;
using UniqloMVC.ViewModels.Slider;

namespace UniqloMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController(UniqloDbContext _context,IWebHostEnvironment _env) : Controller
    {       
        public async Task<IActionResult> Index()
        {
          
            return View(await _context.Sliders.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Create(SliderCreateVM vm)
        {
            if(!vm.File.ContentType.StartsWith("image"))
             ModelState.AddModelError("File", "File must be image!");
            if(vm.File.Length>2*1024*1024)
             ModelState.AddModelError("File", "File length must be less than 2mg");
            if (!ModelState.IsValid) return View();

            string newFileName=Path.GetRandomFileName()+Path.GetExtension(vm.File.FileName);

            using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "sliders", newFileName)))
            {
                await vm.File.CopyToAsync(stream);
            }

            Slider slider = new Slider
            {
                ImageUrl=newFileName,
                Title=vm.Title,
                Subtitle=vm.Subtitle,
                Link=vm.Link,
            };
            
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            return View();
        }
    }
}
