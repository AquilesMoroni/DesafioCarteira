using CarteiraDigital.ViewModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarteiraDigital.Helper
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _httpContext;

        public Sessao(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext; 
        }

        public LoginViewModel BuscarSessaoUsuario()
        {
            string sessaoUsuario = _httpContext.HttpContext.Session.GetString("sessaoUsuarioLogado");

            if (string.IsNullOrEmpty(sessaoUsuario)) return null;

            return JsonConvert.DeserializeObject<LoginViewModel>(sessaoUsuario); 
        }

        public void CriarSessaoUsuario(LoginViewModel login)
        {
            string valor = JsonConvert.SerializeObject(login);

            _httpContext.HttpContext.Session.SetString("sessaoUsuarioLogado", valor); 
        }

        public void FinalizarSessaoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("sessaoUsuarioLogado"); 
        }
    }
}
