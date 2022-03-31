namespace CataPalavra.Buscador.Interfaces;

public interface IBuscadorService
{
    IEnumerable<string> Buscar(string? mascara, string? letrasIgnoradas, string? letrasObrigatorias);
}