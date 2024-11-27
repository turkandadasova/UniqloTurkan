using Microsoft.AspNetCore.Mvc;
using UniqloMVC.DataAccess;

namespace UniqloMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController(UniqloDbContext _context) : Controller
    {       
        public IActionResult Index()
        {
            _context.Sliders.ToList();
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
