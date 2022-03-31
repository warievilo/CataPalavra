using CataPalavra.Buscador.Interfaces;
using System.Reflection;

namespace CataPalavra.Buscador.Services;

public class DicionarioService : IDicionarioService
{
    private readonly string[] _dicionario;

    public DicionarioService()
    {
        var executionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";

        var filePath = Path.Combine(executionPath, "dicio", "pt-br-master", "dicio");

        _dicionario = File.ReadAllLines(filePath);
    }

    public string[] GetDicionario()
    {
        return _dicionario;
    }
}

