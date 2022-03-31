using CataPalavra.Buscador.Interfaces;
using CataPalavra.Buscador.Services;

namespace CataPalavra.Test.Factory;

public static class InstanceFactory
{
    public static IDicionarioService CreateDicionarioService()
    {
        return new DicionarioService();
    }

    public static IBuscadorService CreateBuscadorService()
    {
        var dicionarioService = CreateDicionarioService();

        return new BuscadorService(dicionarioService);
    }
}