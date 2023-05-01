using CarteiraDigital.Models;
using CarteiraDigital.Repositorios;
using CarteiraDigital.ViewModel;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Mapping;
using System.Linq;
using System.Threading.Tasks;

namespace CarteiraDigital.Controllers
{
    public class MovimentosController : Controller
    {

        private readonly MovimentosRepository movimentosRepository;

        public MovimentosController(NHibernate.ISession session) =>
                            movimentosRepository = new MovimentosRepository(session);

        [HttpGet]
        public IActionResult GeraMovimentos()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GeraMovimentos(MovimentoEntrada entrada)
        {
            if (ModelState.IsValid)
            {
                await movimentosRepository.Add(entrada); 
                return RedirectToAction(nameof(Index));
            }

            return View(entrada);
        } 
    }
} 