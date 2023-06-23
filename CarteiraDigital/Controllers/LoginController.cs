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

        public IActionResult Login()
        {
            //Se o usuário estiver logado, redirecione para a home:
            if (_sessao.BuscarSessaoUsuario() != null) return RedirectToAction("Index", "Home"); 

            return View();
        }

        public IActionResult Sair()
        {
            _sessao.FinalizarSessaoUsuario();

            return RedirectToAction("Login", "Login"); 
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Pessoa pessoa = pessoaRepository.FindByEmail(login.Email);

                    if (pessoa != null && login.Email == pessoa.Email && login.Senha == pessoa.Senha)
                    {
                        _sessao.CriarSessaoUsuario(login);
                        return RedirectToAction("Index", "Home", new { pessoa });   
                    } 

                    ViewBag.MensagemErro = "Ops, Usuário Não Encontrado! Tente Novamente!";
                }

                return View("Login");
            }
            catch (Exception erro)
            {
                ViewBag.MensagemErro = $"Ops, não foi possível efetuar o Login. Por favor, verifique o seu Email e Senha e tente novamente! {erro.Message}";
                return RedirectToAction("Login");
            }
        }
    }
} 