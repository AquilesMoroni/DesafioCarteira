using System; 

namespace CarteiraDigital.Models
{
    public class Filtro
    {
        public DateTime dataInicial { get; set; }

        public DateTime dataFinal { get; set; }

        public int tipoMovimento { get; set; }
    }
} 