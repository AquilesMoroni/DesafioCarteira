using CarteiraDigital.Models;
using CarteiraDigital.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CarteiraDigital.Controllers
{
    public class PessoaController : Controller
    {
        private readonly PessoaRepository pessoaRepository;

        public PessoaController(NHibernate.ISession session) =>
                  pessoaRepository = new PessoaRepository(session);

        [HttpGet]
        public ActionResult Index()
        {
            return View(pessoaRepository.FindAll().ToList());  
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                await pessoaRepository.Add(pessoa);
                ViewBag.Script = "<script>Swal.fire({icon: 'success', title: 'Sucesso', text: 'Cadastro Realizado com Sucesso!', position: 'bottom-center', timer: 2000, showConfirmButton: false});</script>";
            } 

            return View(pessoa); 
        }

        public async Task<IActionResult> Deletar(int id)
        {
            await pessoaRepository.Remove(id);
            return RedirectToAction(nameof(Index)); 
        }

        [HttpGet]
        public async Task<IActionResult> Atualizar(int id)
        {
            Pessoa pessoa = await pessoaRepository.FindByID(id);
            return View(pessoa);
        }

        [HttpPost]
        public async Task<IActionResult> Atualizar(Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                await pessoaRepository.Update(pessoa);
                ViewBag.Script = "<script>Swal.fire({icon: 'success', title: 'Sucesso', text: 'Dados Atualizados com Sucesso!', position: 'bottom-center', timer: 2000, showConfirmButton: false});</script>";
            } 

            return View(pessoa); 
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            Pessoa pessoa = await pessoaRepository.FindByID(id);
            return View(pessoa); 
        } 
    }
} 