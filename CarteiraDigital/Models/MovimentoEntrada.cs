using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarteiraDigital.Models
{
    public class MovimentoEntrada
    {
        public virtual int EntradaId { get; set; }

        public virtual DateTime DataEntrada { get; set; }

        public virtual string Descricao { get; set; }

        public virtual decimal Valor { get; set; }

        public virtual Pessoa PessoaId { get; set; }

        public MovimentoEntrada()
        {
            this.DataEntrada = DateTime.Now;
        }
    }
} 