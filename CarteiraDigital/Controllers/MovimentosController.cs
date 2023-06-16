using CarteiraDigital.Models;
using CarteiraDigital.Repositorios;
using CarteiraDigital.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarteiraDigital.Controllers
{
    public class MovimentosController : Controller
    {
        private readonly MovimentosRepository movimentosRepository;
        private readonly PessoaRepository pessoaRepository;

        public MovimentosController(NHibernate.ISession session)
        {
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

            if (saida.PessoaId.Limite == null)
            {
                saida.PessoaId.Limite = 0;
            }
            if (movimento.Valor > saida.PessoaId.Saldo + saida.PessoaId.Limite)
            {
                return false;
            }
            else
            {
                saida.PessoaId.Saldo = saida.PessoaId.Saldo - movimento.Valor;
                await movimentosRepository.Add(saida);
                await pessoaRepository.Update(saida.PessoaId);
                return true;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GeraMovimentos(MovimentoViewModel movimento)
        {
            if (movimento.Tipo == "1" && await Deposito(movimento))
            {
                ViewBag.Script = "<script>Swal.fire({icon: 'success', title: 'Sucesso', text: 'Depósito Realizado com Sucesso!', position: 'bottom-center', timer: 2000, showConfirmButton: false});</script>";
                return View(movimento);
            }
            else
            {
                MovimentoSaida saida = new MovimentoSaida();
                saida.Descricao = movimento.Descricao;
                saida.Valor = movimento.Valor;
                saida.PessoaId = await pessoaRepository.FindByID(movimento.PessoaId);

                if (await Saque(movimento))
                {
                    if (saida.PessoaId.Minimo < saida.PessoaId.Saldo)
                    {
                        ViewBag.Script = "<script>Swal.fire({icon: 'success', title: 'Sucesso', text: 'Saque Realizado com Sucesso!', position: 'bottom-center', timer: 2000, showConfirmButton: false});</script>"; 
                        return View(movimento);
                    }
                }

                if (!await Saque(movimento))
                {
                    ViewBag.Script = "<script>Swal.fire({icon: 'error', title: 'Atenção', text: 'Você não possui saldo e nem limite suficientes para realizar o saque', position: 'bottom-center', timer: 2500, showConfirmButton: false});</script>";
                    return View(movimento);
                }

                ViewBag.Script = "<script>Swal.fire({icon: 'warning', title: 'Atenção', text: 'Saque Realizado com Sucesso. Contudo, seu Saldo ficou abaixo do Mínimo!', position: 'bottom-center', timer: 2500, showConfirmButton: false});</script>";
                return View(movimento);
            }
        }

        public async Task<ActionResult> ExtratoAsync(int id)
        {
            IList<MovimentoViewModel> movimentos = movimentosRepository.FindAll(id);
            ViewBag.mov = movimentos;
            ViewBag.id = id;
            Pessoa p = await pessoaRepository.FindByID(id);
            ViewBag.Nome = p.Nome;
            ViewBag.Saldo = p.Saldo;
            ViewBag.SaldoAnterior = 0.0;
            return View();
        }

        public ActionResult FiltroExtrato(Filtro filtro, int id)
        {
            List<MovimentoViewModel> movimentos = movimentosRepository.FindAll(id);
            IList<MovimentoViewModel> movimentosFiltrados = new List<MovimentoViewModel>();
            ViewBag.id = id;

            DateTime ultimos7dias = DateTime.Now.AddDays(-7);
            DateTime ultimos15dias = DateTime.Now.AddDays(-15);
            DateTime ultimos30dias = DateTime.Now.AddDays(-30);
            DateTime? dataInicio = null;
            DateTime? dataFim = null;

            switch (filtro.tipoPeriodo)
            {
                case 0:
                    dataInicio = ultimos7dias.Date;
                    dataFim = DateTime.Now.Date;
                    break;
                case 1:
                    dataInicio = ultimos15dias.Date;
                    dataFim = DateTime.Now.Date;
                    break;
                case 2:
                    dataInicio = ultimos30dias.Date;
                    dataFim = DateTime.Now.Date;
                    break;
                case 3:
                    dataInicio = filtro.dataInicio;
                    dataFim = filtro.dataFim;
                    break;
            }

            decimal valorTotalEntradas = 0;
            decimal valorTotalSaidas = 0;

            foreach (var item in movimentos)
            {
                if ((filtro.tipoMovimento == 0 && item.Tipo == "Entrada") ||
                    (filtro.tipoMovimento == 1 && item.Tipo == "Saída") ||
                    (filtro.tipoMovimento == 2))
                {
                    if (item.Data.Date >= dataInicio && item.Data.Date <= dataFim)
                    {
                        movimentosFiltrados.Add(item);

                        if (item.Tipo == "Entrada")
                        {
                            valorTotalEntradas = valorTotalEntradas + item.Valor;
                        }
                        else if (item.Tipo == "Saída")
                        {
                            valorTotalSaidas = valorTotalSaidas + item.Valor;
                        }
                    }
                }
            }

            decimal saldoAtual = 0;

            if (filtro.tipoMovimento == 0)
            {
                saldoAtual = valorTotalEntradas;
            }
            else if (filtro.tipoMovimento == 1)
            {
                saldoAtual = -valorTotalSaidas;
            }
            else
            {
                decimal valorTotalanteriorFiltrado = 0;

                foreach (var mov in movimentos)
                {
                    if (mov.Data < dataInicio)
                    {
                        decimal valorMovimento = mov.Tipo == "Entrada" ? mov.Valor : -mov.Valor;
                        valorTotalanteriorFiltrado = valorTotalanteriorFiltrado + valorMovimento;
                    }
                }

                saldoAtual = valorTotalEntradas - valorTotalSaidas + valorTotalanteriorFiltrado;
                ViewBag.SaldoAnterior = valorTotalanteriorFiltrado;
            }

            ViewBag.Saldo = saldoAtual;
            ViewBag.mov = movimentosFiltrados;

            if (filtro.tipoMovimento == 0 || filtro.tipoMovimento == 1)
            {
                ViewBag.SaldoAnterior = 0.0;
                ViewBag.MostrarViewBag = false;
            }
            else
            {
                ViewBag.MostrarViewBag = true;
            }

            return View("Extrato");
        }
    }
} 