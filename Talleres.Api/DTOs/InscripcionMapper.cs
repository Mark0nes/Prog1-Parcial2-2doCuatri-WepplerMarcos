using Talleres.Logica;

namespace Talleres.Api.DTOs;

public static class InscripcionMapper
{
    public static InscripcionResponse AInscripcionResponse(this Inscripcion inscripcion)
    {
        return new InscripcionResponse
        {
            Id = inscripcion.Id,
            TallerId = inscripcion.TallerId,
            Dni = inscripcion.Dni,
            NombreAlumno = inscripcion.NombreAlumno,
            FechaInscripcion = inscripcion.FechaInscripcion,
            FechaBaja = inscripcion.FechaBaja,
            Activa = inscripcion.EstaActiva()
        };
    }
    
    public static Inscripcion AInscripcion(this InscripcionRequest request)
    {
        return new Inscripcion
        {
            TallerId = request.TallerId,
            Dni = request.Dni,
            NombreAlumno = request.NombreAlumno.Trim()
        };
    }
}
