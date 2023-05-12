using CarteiraDigital.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarteiraDigital.ViewModel
{
    public class MovimentoViewModel
    {
        public int PessoaId { get; set; }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public int TipoMovimento { get; set; }
    }
} 