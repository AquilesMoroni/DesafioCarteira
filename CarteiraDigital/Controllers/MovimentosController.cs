using CarteiraDigital.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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