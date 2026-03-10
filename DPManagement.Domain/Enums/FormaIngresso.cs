using System.ComponentModel;

namespace DPManagement.Domain.Enums;

public enum FormaIngresso
{
    [Description("Concurso Público")]
    ConcursoPublico = 1,
    
    [Description("Processo Seletivo Simplificado")]
    ProcessoSeletivoSimplificado = 2,
    
    [Description("Livre Nomeação / Exoneração")]
    LivreNomeacaoExoneracao = 3,
    
    [Description("Mandato")]
    Mandato = 4
}
