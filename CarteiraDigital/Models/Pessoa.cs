using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarteiraDigital.Models
{
    public class Pessoa
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório!")]
        [StringLength(40)]
        public virtual string Nome { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório!")]
        [EmailAddress(ErrorMessage = "O campo E-mail está em formato inválido!")]
        public virtual string Email { get; set; }

        [Required(ErrorMessage = "O campo Salário é obrigatório!")]
        [Range(0, double.MaxValue, ErrorMessage = "O campo Salário deve ser maior ou igual a zero!")]
        [DataType(DataType.Currency)]
        public virtual decimal Salario { get; set; }

        [Required(ErrorMessage = "O campo Limite é obrigatório!")]
        [Range(0, double.MaxValue, ErrorMessage = "O campo Limite deve ser maior ou igual a zero!")]
        public virtual decimal Limite { get; set; }

        [Required(ErrorMessage = "O campo Mínimo é obrigatório!")]
        [Range(0, double.MaxValue, ErrorMessage = "O campo Mínimo deve ser maior ou igual a zero!")]
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
