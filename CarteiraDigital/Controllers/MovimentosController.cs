using CarteiraDigital.Models;
using CarteiraDigital.Repositorios;
using CarteiraDigital.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CarteiraDigital.Controllers
{
    public class MovimentosController : Controller
    {
        private readonly MovimentosRepository movimentosRepository;
        private readonly PessoaRepository pessoaRepository;

        public MovimentosController(NHibernate.ISession session) { 
                movimentosRepository = new MovimentosRepository(session);
                pessoaRepository = new PessoaRepository(session);
        } 

        [HttpGet]
        public IActionResult GeraMovimentos(int id)
        {
            MovimentoViewModel movimento = new MovimentoViewModel();
            movimento.PessoaId = id;
            
            return View(movimento);
        }

        public async Task<bool> Deposito(MovimentoViewModel movimento)
        {
            //CONVERSÃO PARA MOVIMENTO ENTRADA
            MovimentoEntrada entrada = new MovimentoEntrada();
            entrada.Descricao = movimento.Descricao;
            entrada.Valor = movimento.Valor;
            entrada.PessoaId = await pessoaRepository.FindByID(movimento.PessoaId);

            //ATUALIZA SALDO PESSOA
            entrada.PessoaId.Saldo =  entrada.PessoaId.Saldo + entrada.Valor;

            //SALVA NO BANCO AS ALTERAÇõES E ADICIONA UM MOVIMENTO
            await movimentosRepository.Add(entrada);
            await pessoaRepository.Update(entrada.PessoaId);

            return true;
        }

        public async Task<bool> Saque(MovimentoViewModel movimento)
        {
            //CONVERSÃO PARA MOVIMENTO SAIDA
            MovimentoSaida saida = new MovimentoSaida();
            saida.Descricao = movimento.Descricao;
            saida.Valor = movimento.Valor;
            saida.PessoaId = await pessoaRepository.FindByID(movimento.PessoaId);

            if (movimento.Valor > saida.PessoaId.Saldo + saida.PessoaId.Limite)
            {
                return false;  
            }
            else
            {
                saida.PessoaId.Saldo = saida.PessoaId.Saldo - movimento.Valor;

                await movimentosRepository.Add(saida);
                await pessoaRepository.Update(saida.PessoaId);
            }

            return true;
        }

        [HttpPost]
        public async Task<IActionResult> GeraMovimentos(MovimentoViewModel movimento)
        {
            if (movimento.TipoMovimento == 1)
            {
                await Deposito(movimento);
            }
            else
            {
                if (!await Saque(movimento))
                {
                    ModelState.AddModelError(string.Empty, "Você não possui Saldo e nem Limite suficientes para realizar o Saque.");
                    return View(movimento);
                }
            }

             return RedirectToAction("GeraMovimentos", movimento);
        } 
    }
} 