using Microsoft.VisualStudio.TestTools.UnitTesting;
using CataPalavra.Buscador.Interfaces;
using System.Linq;

namespace CataPalavra.Test.Test;

[TestClass]
public class BuscadorService_PalavraNaoEncontrada
{
    private readonly IBuscadorService _buscadorService;

    public BuscadorService_PalavraNaoEncontrada() => _buscadorService = Factory.InstanceFactory.CreateBuscadorService();

    [TestMethod]
    [DataRow("", "", "")]
    [DataRow("tes**", "aie", "")]
    [DataRow("tes**", "aie", "e")]
    [DataRow("tes**", "", "x")]
    public void BuscadorService_PalavraNaoEncontrada_ReturnFalse(string mascara, string letrasIgnoradas, string letrasObrigatorias)
    {
        var resultadoDaBusca = _buscadorService.Buscar(mascara, letrasIgnoradas, letrasObrigatorias);
        
        Assert.IsFalse(resultadoDaBusca.Count() > 0, "Nenhuma palavra deve ser encontrada");
    }
}