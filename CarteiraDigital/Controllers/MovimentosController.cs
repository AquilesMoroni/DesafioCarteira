using Microsoft.AspNetCore.Mvc;

namespace CarteiraDigital.Controllers
{
    public class MovimentosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GeraMovimentos()
        {
            return View();
        } 
    }
} 