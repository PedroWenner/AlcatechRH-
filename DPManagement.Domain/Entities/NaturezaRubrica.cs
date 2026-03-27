using System;

namespace DPManagement.Domain.Entities
{
    public class NaturezaRubrica
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime Inicio { get; set; }
        public DateTime? Termino { get; set; }
    }
}
