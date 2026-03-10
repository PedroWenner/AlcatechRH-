using DPManagement.Infrastructure.Persistence;
using DPManagement.Domain.Entities;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.API.Data.Seed;

public static class CboIngestion
{
    public static async Task SeedCbosAsync(DPManagementDbContext context, string csvPath)
    {
        if (await context.Cbos.AnyAsync()) return;

        if (!File.Exists(csvPath))
        {
            Console.WriteLine($"[CBO Seed] Arquivo não encontrado em: {csvPath}");
            return;
        }

        var cbos = new List<Cbo>();
        
        // CBO CSV is CODIGO;TITULO and likely Windows-1252
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var encoding = Encoding.GetEncoding(1252);

        using (var reader = new StreamReader(csvPath, encoding))
        {
            // Skip header
            await reader.ReadLineAsync();

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(';');
                if (parts.Length < 2) continue;

                cbos.Add(new Cbo
                {
                    Id = Guid.NewGuid(),
                    Codigo = parts[0].Trim(),
                    Titulo = parts[1].Trim()
                });
            }
        }

        if (cbos.Any())
        {
            Console.WriteLine($"[CBO Seed] Importando {cbos.Count} registros...");
            await context.Cbos.AddRangeAsync(cbos);
            await context.SaveChangesAsync();
            Console.WriteLine("[CBO Seed] Importação concluída.");
        }
    }
}
