using Microsoft.AspNetCore.Mvc;
using UniqloMVC.ViewModels.Auths;

namespace UniqloMVC.Controllers
{
    public class AccountController : Controller
    {
       
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserCreateVM vm)
        {
            if(!ModelState.IsValid)
            return View();
            return View();
        }
    }
}
