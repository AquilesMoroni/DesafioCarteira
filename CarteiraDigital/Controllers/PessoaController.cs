﻿using CarteiraDigital.Models;
using CarteiraDigital.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarteiraDigital.Controllers
{
    public class PessoaController : Controller
    {
        private readonly PessoaRepository pessoaRepository;

        public PessoaController(NHibernate.ISession session)
        {
            pessoaRepository = new PessoaRepository(session); 
        }

        [HttpGet]
        public ActionResult Index()
        {
            IList<Pessoa> Pessoas = pessoaRepository.FindAll().ToList(); 
            return View(Pessoas.Reverse<Pessoa>());
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
                ViewBag.Script = @"
                <script>
                Swal.fire({
                    icon: 'success',
                    title: 'Sucesso',
                    text: 'Cadastro Realizado com Sucesso!',
                    position: 'bottom-center',
                    timer: 2000,
                    showConfirmButton: false
                    });

                 setTimeout(function () {
                    const inputs = document.querySelectorAll('input[data-span-id]');
                    inputs.forEach(function (input) {
                        input.value = '';
                    });

                    const spans = document.querySelectorAll('[data-span-id]');
                    spans.forEach(function (span) {
                        span.textContent = '';
                    });
                 }, 1000);

                 document.addEventListener('DOMContentLoaded', function () {
                    const inputs = document.querySelectorAll('input[data-span-id]');
                    inputs.forEach(function (input) {
                        input.addEventListener('click', function () {
                            const spanId = input.getAttribute('data-span-id');
                            const span = document.getElementById(spanId);
                            if (span) {
                                span.textContent = '';
                            }
                        });

                        input.addEventListener('focus', function () {
                            const spanId = input.getAttribute('data-span-id');
                            const span = document.getElementById(spanId);
                            if (span) {
                                span.textContent = '';
                            }
                        });
                    });
                }); 
              </script>";
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