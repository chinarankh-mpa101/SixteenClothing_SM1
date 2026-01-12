using Microsoft.AspNetCore.Mvc;
using SixteenClothing.Models;
using System.Diagnostics;

namespace SixteenClothing.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

  
    }
}
