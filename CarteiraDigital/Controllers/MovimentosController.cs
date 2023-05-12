﻿using CarteiraDigital.Models;
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
            MovimentoEntrada entrada = new MovimentoEntrada();
            entrada.Descricao = movimento.Descricao;
            entrada.Valor = movimento.Valor;
            entrada.PessoaId = await pessoaRepository.FindByID(movimento.PessoaId);

            entrada.PessoaId.Saldo = entrada.PessoaId.Saldo + entrada.Valor;

            await movimentosRepository.Add(entrada);
            await pessoaRepository.Update(entrada.PessoaId); 

            return true; 
        }

        public async Task<bool> Saque(MovimentoViewModel movimento)
        {
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
            if (movimento.TipoMovimento == 1 && await Deposito(movimento))
            {
                    ViewBag.Script = "<script>Swal.fire({icon: 'success', title: 'Sucesso', text: 'Depósito Realizado com Sucesso!', position: 'bottom-center', timer: 2000, showConfirmButton: false});</script>";
                    return View(movimento); 
            }
            else 
            {
                if (!await Saque(movimento))
                {
                    ViewBag.Script = "<script>Swal.fire({icon: 'error', title: 'Atenção', text: 'Você não possui saldo e nem limite suficientes para realizar o saque', position: 'bottom-center', timer: 2500, showConfirmButton: false});</script>";
                    return View(movimento);
                }

                    ViewBag.Script = "<script>Swal.fire({icon: 'success', title: 'Sucesso', text: 'Saque Realizado com Sucesso!', position: 'bottom-center', timer: 2000, showConfirmButton: false});</script>";
                    return View(movimento);  
            }
        } 
    }
} 