using Buscador.Interfaces;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Buscador.Services;

public class BuscadorService : IBuscadorService
{
    public IEnumerable<string> Buscar(string? mascara, string? letrasIgnoradas, string? letrasObrigatorias)
    {
        var palavras = new List<string>();

        if (string.IsNullOrEmpty(mascara))
            return palavras;

        var ra = GetRegexPrimaryFilter(mascara, letrasIgnoradas);
        var rb = GetRegexSecondaryFilter(letrasObrigatorias);
        
        var provider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());

        var files = provider.GetDirectoryContents(string.Empty);
        
        using var stream = provider.GetFileInfo("dicio.pt_br_master.dicio").CreateReadStream();
        using var reader = new StreamReader(stream);        
        string[] dicionario = reader.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

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
