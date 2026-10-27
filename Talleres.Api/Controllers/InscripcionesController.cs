using Microsoft.AspNetCore.Mvc;
using Talleres.Api.DTOs;
using Talleres.Logica;

namespace Talleres.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InscripcionesController : ControllerBase
{
    private readonly InscripcionService _servicio;

    public InscripcionesController(InscripcionService servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public IActionResult ObtenerTodas([FromQuery] int? dni)
    {
        try
        {
            var respuesta = _servicio.ObtenerInscripciones(dni).Select(i => i.AInscripcionResponse());
            return Ok(respuesta);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Ocurrió un error inesperado", detalle = ex.Message });
        }
    }

    [HttpPost]
    public IActionResult Inscribir([FromBody] InscripcionRequest request)
    {
        try
        {
            Inscripcion inscripcion = _servicio.Inscribir(request.AInscripcion());
            return StatusCode(201, inscripcion.AInscripcionResponse());
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Ocurrió un error inesperado", detalle = ex.Message });
        }
    }

    [HttpPut("{id}/baja")]
    public IActionResult DarDeBaja(int id)
    {
        try
        {
            Inscripcion inscripcion = _servicio.DarDeBaja(id);
            return Ok(inscripcion.AInscripcionResponse());
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        
    }
}
