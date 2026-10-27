using Microsoft.AspNetCore.Mvc;
using Talleres.Api.DTOs;
using Talleres.Logica;

namespace Talleres.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TalleresController : ControllerBase
{
    private readonly InscripcionService _servicio;

    public TalleresController(InscripcionService servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public IActionResult ObtenerTodos()
    {
        try
        {
            var respuesta = _servicio.ObtenerTalleresConCupos().Select(t => t.ATallerResponse());
            return Ok(respuesta);
        }
    }
}
