using DPManagement.Domain.Entities;
using DPManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DPManagement.API.Data.Seed;

public static class BancoIngestion
{
    public static async Task SeedBancosAsync(DPManagementDbContext context, string sqlPath)
    {
        if (await context.Bancos.AnyAsync()) return;

        if (!File.Exists(sqlPath))
        {
            Console.WriteLine($"[Banco Seed] Arquivo não encontrado em: {sqlPath}");
            return;
        }

        var bancos = new List<Banco>();
        var lines = await File.ReadAllLinesAsync(sqlPath);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("INSERT INTO Banks", StringComparison.OrdinalIgnoreCase))
                continue;

            var valuesPartIndex = line.IndexOf("VALUES(", StringComparison.OrdinalIgnoreCase);
            if (valuesPartIndex == -1) continue;

            var valuesPart = line.Substring(valuesPartIndex + 7).TrimEnd(';', ')');
            var values = ParseSqlValues(valuesPart);

            if (values.Count >= 5)
            {
                var codigo = values[0];
                var nome = values[3];
                var nomeCurto = values[4];

                if (codigo.Equals("NULL", StringComparison.OrdinalIgnoreCase)) codigo = string.Empty;
                if (nome.Equals("NULL", StringComparison.OrdinalIgnoreCase)) nome = string.Empty;
                if (nomeCurto.Equals("NULL", StringComparison.OrdinalIgnoreCase)) nomeCurto = string.Empty;

                if (!string.IsNullOrEmpty(codigo))
                {
                    bancos.Add(new Banco
                    {
                        Id = Guid.NewGuid(),
                        CodigoBanco = codigo,
                        Nome = nome,
                        NomeCurto = nomeCurto
                    });
                }
            }
        }

        if (bancos.Any())
        {
            Console.WriteLine($"[Banco Seed] Importando {bancos.Count} registros...");
            await context.Bancos.AddRangeAsync(bancos);
            await context.SaveChangesAsync();
            Console.WriteLine("[Banco Seed] Importação concluída.");
        }
    }

    private static List<string> ParseSqlValues(string valuesString)
    {
        var values = new List<string>();
        bool inString = false;
        var current = new StringBuilder();

        for (int i = 0; i < valuesString.Length; i++)
        {
            char c = valuesString[i];
            if (c == '\'')
            {
                if (inString && i + 1 < valuesString.Length && valuesString[i + 1] == '\'')
                {
                    current.Append('\'');
                    i++; // Skip escaped quote
                }
                else
                {
                    inString = !inString;
                }
            }
            else if (c == ',' && !inString)
            {
                values.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }
        values.Add(current.ToString());
        return values;
    }
}
