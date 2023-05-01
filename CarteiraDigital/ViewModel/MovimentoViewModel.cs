using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarteiraDigital.ViewModel
{
    public class MovimentoViewModel
    {
        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public bool TipoMovimento { get; set; }
    }
} 