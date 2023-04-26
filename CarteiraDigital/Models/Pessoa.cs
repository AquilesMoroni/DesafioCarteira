using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarteiraDigital.Models
{
    public class Pessoa
    {
        public virtual int Id { get; set; }

        public virtual string Nome { get; set; }

        public virtual string Email { get; set; }

        public virtual decimal Salario { get; set; }

        public virtual decimal Limite { get; set; }

        public virtual decimal Minimo { get; set; }

        public virtual decimal Saldo { get; set; } 

        public virtual IList<MovimentoEntrada> Entradas { get; set; }
        
        public virtual IList<MovimentoSaida> Saidas { get; set; }

        //Construtor:
        public Pessoa()
        {
            Saldo = 0; 
        } 
    }
}
