using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarteiraDigital.Models
{
    public class Pessoa
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório!")]
        [StringLength(40)]
        [RegularExpression(@"^(?:[A-Za-zÀ-ÿ]{2,} ?)+$", ErrorMessage = "Nome Inválido")]
        public virtual string Nome { get; set; } 

        [Required(ErrorMessage = "O campo E-mail é obrigatório!")]
        [StringLength(40)]
        [EmailAddress(ErrorMessage = "O campo E-mail está em formato inválido!")] 
        public virtual string Email { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O campo Salário deve ser maior ou igual a zero!")] 
        public virtual decimal? Salario { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O campo Limite deve ser maior ou igual a zero!")]
        public virtual decimal? Limite { get; set; }

        [Required(ErrorMessage = "O campo Mínimo é obrigatório!")] 
        public virtual decimal? Minimo { get; set; }

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