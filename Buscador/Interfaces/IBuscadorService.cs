namespace Buscador.Interfaces;

public interface IBuscadorService
{
    Task<IEnumerable<string>> Buscar(string? mascara, string? letrasIgnoradas, string? letrasObrigatorias);
}