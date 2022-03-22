﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Buscador.Interfaces;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBuscadorService _buscadorService;

    public HomeController(ILogger<HomeController> logger, IBuscadorService buscadorService)
    {
        _logger = logger;
        _buscadorService = buscadorService;
    }

    public async Task<IActionResult> Index(string mascara, string letrasIgnoradas, string letrasObrigatorias)
    {
        ViewData["mascara"] = mascara;
        ViewData["letrasIgnoradas"] = letrasIgnoradas;
        ViewData["letrasObrigatorias"] = letrasObrigatorias;

        var resultadoDaBusca = await _buscadorService.Buscar(mascara, letrasIgnoradas, letrasObrigatorias);

        var palavras = resultadoDaBusca.ToList();
        
        var retorno = new List<List<string>>(); 

        int tamanho = 6;

        for (int i = 0; i < palavras.Count; i += tamanho) 
            retorno.Add(palavras.GetRange(i, Math.Min(tamanho, palavras.Count - i))); 

        return View(retorno);
    }
}
