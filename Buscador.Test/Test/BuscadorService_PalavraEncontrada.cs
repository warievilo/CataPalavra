using Microsoft.VisualStudio.TestTools.UnitTesting;
using Buscador.Interfaces;
using Buscador.Services;
using System.Linq;

namespace Buscador.Test.Test;

[TestClass]
public class BuscadorService_PalavraEncontrada
{
    private readonly IBuscadorService _buscadorService;

    public BuscadorService_PalavraEncontrada()
    {
        _buscadorService = new BuscadorService();
    }

    [TestMethod]
    [DataRow("teste", "", "")]
    [DataRow("test*", "", "")]
    [DataRow("tes**", "", "a")]
    public void BuscadorService_PalavraEncontrada_ReturnTrue(string mascara, string letrasIgnoradas, string letrasObrigatorias)
    {
        var resultadoDaBusca = _buscadorService.Buscar(mascara, letrasIgnoradas, letrasObrigatorias);
        
        Assert.IsTrue(resultadoDaBusca.Count() > 0, "Alguma palavra deve ser encontrada");
    }

}