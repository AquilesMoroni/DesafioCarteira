using CarteiraDigital.Models;
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

        public Pessoa BuscarSessaoUsuario()
        {
            string sessaoUsuario = _httpContext.HttpContext.Session.GetString("sessaoUsuarioLogado");

            if (string.IsNullOrEmpty(sessaoUsuario)) return null;

            return JsonConvert.DeserializeObject<Pessoa>(sessaoUsuario);
        }

        public void CriarSessaoUsuario(Pessoa pessoa)
        {
            string valor = JsonConvert.SerializeObject(pessoa); 

            _httpContext.HttpContext.Session.SetString("sessaoUsuarioLogado", valor); 
        }

        public void RemoverSessaoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("sessaoUsuarioLogado"); 
        }
    }
}
