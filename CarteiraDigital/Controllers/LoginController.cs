using CarteiraDigital.Helper;
using CarteiraDigital.Models;
using CarteiraDigital.Repositorios;
using CarteiraDigital.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarteiraDigital.Controllers
{
    public class LoginController : Controller
    {
        private readonly PessoaRepository pessoaRepository;

        private readonly ISessao _sessao;

        public LoginController(NHibernate.ISession session, ISessao sessao)
        {
            pessoaRepository = new PessoaRepository(session);
            _sessao = sessao;
        }

        public IActionResult Index()
        {
            //Se o usuário estiver logado, redirecione para a home:
            if (_sessao.BuscarSessaoUsuario() != null) return RedirectToAction("Index", "Home"); 

            return View();
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoUsuario();
            return RedirectToAction("Index", "Login"); 
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Pessoa usuario = pessoaRepository.FindByEmail(loginModel.Email);

                    if (usuario != null) 
                    {
                        if (loginModel.Email == usuario.Email && loginModel.Senha == usuario.Senha)
                        {
                            _sessao.CriarSessaoUsuario(usuario); 
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

                return View("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
} 