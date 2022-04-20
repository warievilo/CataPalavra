namespace CataPalavra.Buscador.Interfaces;

public interface IBuscadorService
{
    IEnumerable<string> Search(string? mascara, string? letrasIgnoradas, string? letrasObrigatorias);
}