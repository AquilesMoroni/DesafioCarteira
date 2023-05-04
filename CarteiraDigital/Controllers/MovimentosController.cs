using CarteiraDigital.Models;
using CarteiraDigital.Repositorios;
using CarteiraDigital.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Mapping;
using System.Linq;
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

            //ATUALIZA SALDO PESSOA
            saida.PessoaId.Saldo = saida.PessoaId.Saldo - saida.Valor;

            //SALVA NO BANCO AS ALTERAÇõES E ADICIONA UM MOVIMENTO
            await movimentosRepository.Add(saida);
            await pessoaRepository.Update(saida.PessoaId); 

            return true;
        }

        [HttpPost]
        public async Task<IActionResult> GeraMovimentos(MovimentoViewModel movimento)
        {
            if (movimento.TipoMovimento == true)
            {
                await Deposito(movimento);
            }
            else
            {
                var pessoa = await pessoaRepository.FindByID(movimento.PessoaId);

                if (movimento.Valor > pessoa.Saldo) 
                {
                    ModelState.AddModelError("Valor", "Saldo insuficiente para realizar o saque.");
                    return View(movimento);
                }
                else
                {
                    await Saque(movimento);
                }
            }

            return RedirectToAction("GeraMovimentos", movimento);
        } 
    }
} 