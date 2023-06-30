using CarteiraDigital.Models;
using CarteiraDigital.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarteiraDigital.Helper
{
    public interface ISessao
    {
        void CriarSessaoUsuario(Pessoa pessoa);

        void RemoverSessaoUsuario();

        Pessoa BuscarSessaoUsuario();  
    }
}
