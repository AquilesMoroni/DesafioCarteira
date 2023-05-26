using System; 

namespace CarteiraDigital.Models
{
    public class Filtro
    {
        public int tipoPeriodo { get; set; }
        public DateTime? dataInicio { get; set; }
        public DateTime? dataFim { get; set; }
        public int tipoMovimento { get; set; }
    } 
}