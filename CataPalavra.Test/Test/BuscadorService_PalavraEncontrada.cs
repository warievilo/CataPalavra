using Microsoft.VisualStudio.TestTools.UnitTesting;
using CataPalavra.Buscador.Interfaces;
using System.Linq;

namespace CataPalavra.Test.Test;

[TestClass]
public class BuscadorService_PalavraEncontrada
{
    private readonly IBuscadorService _buscadorService;

    public BuscadorService_PalavraEncontrada() => _buscadorService = Factory.InstanceFactory.CreateBuscadorService();

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