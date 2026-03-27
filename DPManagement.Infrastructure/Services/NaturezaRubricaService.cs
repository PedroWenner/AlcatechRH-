using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Infrastructure.Persistence;

namespace DPManagement.Infrastructure.Services
{
    public class NaturezaRubricaService : INaturezaRubricaService
    {
        private readonly DPManagementDbContext _context;

        public NaturezaRubricaService(DPManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NaturezaRubricaDto>> GetAllAsync()
        {
            var naturezas = await _context.NaturezaRubricas
                .AsNoTracking()
                .OrderBy(x => x.Codigo)
                .Select(x => new NaturezaRubricaDto
                {
                    Codigo = x.Codigo,
                    Nome = x.Nome,
                    Descricao = x.Descricao,
                    Inicio = x.Inicio,
                    Termino = x.Termino
                })
                .ToListAsync();

            return naturezas;
        }

        public async Task<NaturezaRubricaDto?> GetByCodigoAsync(string codigo)
        {
            var x = await _context.NaturezaRubricas
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Codigo == codigo);

            if (x == null) return null;

            return new NaturezaRubricaDto
            {
                Codigo = x.Codigo,
                Nome = x.Nome,
                Descricao = x.Descricao,
                Inicio = x.Inicio,
                Termino = x.Termino
            };
        }
    }
}
