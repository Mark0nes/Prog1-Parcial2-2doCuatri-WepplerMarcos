using System.ComponentModel.DataAnnotations;

namespace Talleres.Api.DTOs;

public class InscripcionRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "El TallerId es obligatorio y debe ser positivo.")]
    public int TallerId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El Dni es obligatorio y debe ser positivo.")]
    public int Dni { get; set; }

    [Required(ErrorMessage = "El nombre del alumno es obligatorio.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre no puede estar vacío ni superar 100 caracteres.")]
    public string NombreAlumno { get; set; } = string.Empty;
}
