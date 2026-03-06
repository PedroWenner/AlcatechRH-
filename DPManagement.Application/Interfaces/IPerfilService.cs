using DPManagement.Domain.Entities;

namespace DPManagement.Application.Interfaces;

public interface IPerfilService
{
    Task<IEnumerable<Perfil>> ListarTodosAsync();
    Task<Perfil?> ObterPorIdAsync(Guid id);
}
