using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarteiraDigital.Models
{
    public class MovimentoSaida : Movimentos
    {
        public virtual int SaidaId { get; set; }

        public virtual DateTime DataSaida { get; set; }

        public virtual string Descricao { get; set; }

        public virtual decimal Valor { get; set; }

        public virtual Pessoa PessoaId { get; set; }
    }
}
