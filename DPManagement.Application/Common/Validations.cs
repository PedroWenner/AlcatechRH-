using System.Text.RegularExpressions;

namespace DPManagement.Application.Common;

public static class Validations
{
    public static bool ValidarCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        cpf = Regex.Replace(cpf, @"[^\d]", "");

        if (cpf.Length != 11) return false;

        if (new string(cpf[0], 11) == cpf) return false;

        var tempCpf = cpf.Substring(0, 9);
        var sum = 0;

        for (int i = 0; i < 9; i++)
            sum += int.Parse(tempCpf[i].ToString()) * (10 - i);

        var remainder = sum % 11;
        var digit = remainder < 2 ? 0 : 11 - remainder;

        tempCpf += digit;
        sum = 0;

        for (int i = 0; i < 10; i++)
            sum += int.Parse(tempCpf[i].ToString()) * (11 - i);

        remainder = sum % 11;
        digit = remainder < 2 ? 0 : 11 - remainder;

        tempCpf += digit;

        return cpf.EndsWith(tempCpf.Substring(9));
    }
}
