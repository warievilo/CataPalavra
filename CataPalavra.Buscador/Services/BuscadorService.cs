using CataPalavra.Buscador.Interfaces;
using System.Text.RegularExpressions;

namespace CataPalavra.Buscador.Services;

public class BuscadorService : IBuscadorService
{
    private readonly IDicionarioService _dicionarioService;

    public BuscadorService(IDicionarioService dicionarioService)
    {
        _dicionarioService = dicionarioService;
    }

    public IEnumerable<string> Search(string? word, string? ignoredLetters, string? includedLetters)
    {
        var foundWords = new List<string>();

        if (string.IsNullOrEmpty(word))
            return foundWords;

        var regexForWord = GetRegexForWord(word);
        var regexForIgnoredLetters = GetRegexForIgnoredLetters(word, ignoredLetters);
        var regexForIncludedLetters = GetRegexForIncludedLetters(word, includedLetters);
        
        var dictionary = _dicionarioService.GetDicionario();

        foreach (var w in dictionary)
            if (regexForWord.IsMatch(w) && regexForIgnoredLetters.IsMatch(w) && regexForIncludedLetters.IsMatch(w))
                foundWords.Add(w);

        return foundWords;    
    }

    private Regex GetRegexForWord(string? word) 
    {
        string filter = string.Empty;

        if (!string.IsNullOrWhiteSpace(word))
        {
            word = word.ToLower();

            foreach (char character in word.ToCharArray())
            {
                var characterFilter = character == '*' ? "." : $"[{ character }]";

                filter = $"{filter}{characterFilter}";        
            }
        }

        filter = @$"\b{ filter }\b";

        return new Regex(filter, RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }

    private Regex GetRegexForIgnoredLetters(string? word, string? ignoredLetters) 
    {
        string filter = string.Empty;

        if (!string.IsNullOrWhiteSpace(word))
        {
            string ignoredLettersFilter = string.Empty;
        
            if (!string.IsNullOrWhiteSpace(ignoredLetters))
                ignoredLettersFilter = $"(?![{ ignoredLetters.ToLower() }])";
            
            word = word.ToLower();

            foreach (char character in word.ToCharArray())
            {
                if (character != '*')
                {
                    filter += $"[{ character }]";
                    continue;
                }
                
                filter += string.IsNullOrEmpty(ignoredLettersFilter) ? "." : $"{ ignoredLettersFilter }.";                
            }
        }

        filter = @$"\b{ filter }\b";

        return new Regex(filter, RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }

    private Regex GetRegexForIncludedLetters(string? word, string? includedLetters) 
    {
        string filter = string.Empty;

        if (!string.IsNullOrWhiteSpace(word))
        {
            string includedLettersFilter = string.Empty;
        
            if (!string.IsNullOrWhiteSpace(includedLetters)) {
                includedLetters = includedLetters.ToLower();
                includedLettersFilter = $"[{ string.Join('|', includedLetters.ToCharArray()) }]";
            }
            
            word = word.ToLower();

            foreach (char character in word.ToCharArray())
            {
                if (character != '*')
                {
                    filter += $"[{ character }]";
                    continue;
                }
                
                filter += string.IsNullOrEmpty(includedLettersFilter) ? "." : $"{ includedLettersFilter }";                
            }
        }

        filter = @$"\b{ filter }\b";

        return new Regex(filter, RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
