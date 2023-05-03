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

        [Required(ErrorMessage = "A descrição é obrigatória", AllowEmptyStrings = false)]
        [StringLength(35, MinimumLength = 1)]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo valor é obrigatório", AllowEmptyStrings = false)]
        [Range(0, double.MaxValue, ErrorMessage = "Não é possível inserir um número menor ou igual a zero!")]
        public decimal Valor { get; set; }

        public bool TipoMovimento { get; set; }
    }
} 