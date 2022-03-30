using Buscador.Interfaces;
using System.Reflection;

namespace Buscador.Services;

public class DicionarioService : IDicionarioService
{
    private readonly string[] _dicionario;

    public DicionarioService()
    {
        var executionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";

        _dicionario = File.ReadAllLines($"{ executionPath }{ @"\dicio\pt-br-master\dicio" }");
    }

    public string[] GetDicionario()
    {
        return _dicionario;
    }
}

