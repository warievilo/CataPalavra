using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text.RegularExpressions;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(string mascara, string letrasIgnoradas, string letrasObrigatorias)
    {
        ViewData["mascara"] = mascara;
        ViewData["letrasIgnoradas"] = letrasIgnoradas;
        ViewData["letrasObrigatorias"] = letrasObrigatorias;

        List<string> words = Search(mascara, letrasIgnoradas, letrasObrigatorias);

        int nSize = 6;
        var list = new List<List<string>>(); 

        for (int i = 0; i < words.Count; i += nSize) 
            list.Add(words.GetRange(i, Math.Min(nSize, words.Count - i))); 

        return View(list);
    }

    private List<string> Search(string? inputedWord, string? ignoredLetters, string? requiredLetters)
    {
        var words = new List<string>();

        if (string.IsNullOrEmpty(inputedWord))
            return words;

        var ra = GetRegexPrimaryFilter(inputedWord, ignoredLetters);

        var rb = GetRegexSecondaryFilter(requiredLetters);
        
        //string[] dicio = System.IO.File.ReadAllLines(@".\dicio\pt-br-sem-acentos\br-sem-acentos.txt");
        string[] dicio = System.IO.File.ReadAllLines(@".\dicio\pt-br-master\dicio");
        
        foreach (string word in dicio)
        {
            if (ra.IsMatch(word) && rb.IsMatch(word))
                words.Add(word);
        }

        return words;
    }

    private Regex GetRegexPrimaryFilter(string inputedWord, string? ignoredLetters) 
    {
        string ignoredLettersFilter = string.Empty;
        
        if (!string.IsNullOrEmpty(ignoredLetters) && ignoredLetters.Length > 0)
            ignoredLettersFilter = $"(?![{ ignoredLetters.ToLower() }])";

        string lettersfilter = string.Empty;

        foreach (char c in inputedWord.ToLower().ToCharArray())
        {
            var wordFilter = ".";

            if (c != '*')
                wordFilter = $"[{ c }]";

            if (string.IsNullOrEmpty(ignoredLettersFilter))
                lettersfilter += wordFilter;
            else
                lettersfilter += $"{ignoredLettersFilter}{wordFilter}";        
        }

        lettersfilter = @$"\b{ lettersfilter }\b";

        return new Regex(lettersfilter, RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }

    private Regex GetRegexSecondaryFilter(string? requiredLetters) 
    {
        //(?=\w*c)(?=\w*y)\w+
        var filter = @"\w+";

        if (!string.IsNullOrEmpty(requiredLetters))
            foreach (var c in requiredLetters.ToLower().ToCharArray())
                filter = $@"(?=\w*{ c }){ filter }";

        return new Regex(filter, RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
