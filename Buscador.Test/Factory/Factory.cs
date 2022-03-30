using Buscador.Interfaces;
using Buscador.Services;

namespace Buscador.Test.Factory;

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