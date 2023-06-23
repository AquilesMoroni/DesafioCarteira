using CarteiraDigital.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarteiraDigital.Helper
{
    public interface ISessao
    {
        void CriarSessaoUsuario(LoginViewModel login);

        void FinalizarSessaoUsuario();

        LoginViewModel BuscarSessaoUsuario();  
    }
}
