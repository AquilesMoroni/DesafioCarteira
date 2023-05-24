using System;

namespace CarteiraDigital.ViewModel
{
    public class MovimentoViewModel
    {
        public int PessoaId { get; set; }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public string Tipo { get; set; }

        public DateTime Data { get; set; } 
    }
} 