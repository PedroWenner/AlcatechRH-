using System;

namespace DPManagement.Application.DTOs
{
    public class NaturezaRubricaDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime Inicio { get; set; }
        public DateTime? Termino { get; set; }
    }
}
