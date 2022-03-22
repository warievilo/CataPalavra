using Buscador.Interfaces;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;

namespace Buscador.Services;

public class BuscadorService : IBuscadorService
{
    public async Task<IEnumerable<string>> Buscar(string? mascara, string? letrasIgnoradas, string? letrasObrigatorias)
    {
        var palavras = new List<string>();

        if (string.IsNullOrEmpty(mascara))
            return palavras;

        var ra = GetRegexPrimaryFilter(mascara, letrasIgnoradas);
        var rb = GetRegexSecondaryFilter(letrasObrigatorias);
        
        //string[] dicionario = File.ReadAllLines(@".\dicio\pt-br-sem-acentos\br-sem-acentos.txt");
        string[] dicionario = File.ReadAllLines(@".\dicio\pt-br-master\dicio");
        
        foreach (string palavra in dicionario)
            if (ra.IsMatch(palavra) && rb.IsMatch(palavra))
                palavras.Add(palavra);

        return palavras;
    }

    private Regex GetRegexPrimaryFilter(string mascara, string? letrasIgnoradas) 
    {
        string filtroLetrasIgnoradas = string.Empty;
        
        if (!string.IsNullOrEmpty(letrasIgnoradas) && letrasIgnoradas.Length > 0)
            filtroLetrasIgnoradas = $"(?![{ letrasIgnoradas.ToLower() }])";

        string filtroLetras = string.Empty;

        foreach (char c in mascara.ToLower().ToCharArray())
        {
            var filtroPalavras = ".";

            if (c != '*')
                filtroPalavras = $"[{ c }]";

            if (string.IsNullOrEmpty(filtroLetrasIgnoradas))
                filtroLetras += filtroPalavras;
            else
                filtroLetras += $"{filtroLetrasIgnoradas}{filtroPalavras}";        
        }

        filtroLetras = @$"\b{ filtroLetras }\b";

        return new Regex(filtroLetras, RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }

    private Regex GetRegexSecondaryFilter(string? letrasObrigatorias) 
    {
        //(?=\w*c)(?=\w*y)\w+
        var filtro = @"\w+";

        if (!string.IsNullOrEmpty(letrasObrigatorias))
            foreach (var c in letrasObrigatorias.ToLower().ToCharArray())
                filtro = $@"(?=\w*{ c }){ filtro }";

        return new Regex(filtro, RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
