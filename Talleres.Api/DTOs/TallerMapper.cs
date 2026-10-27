using Talleres.Logica;

namespace Talleres.Api.DTOs;

public static class TallerMapper
{
    public static TallerResponse ATallerResponse(this TallerConCupos item)
    {
        return new TallerResponse
        {
            Id = item.Taller.Id,
            Nombre = item.Taller.Nombre,
            Dia = item.Taller.Dia,
            HoraInicio = item.Taller.HoraInicio,
            HoraFin = item.Taller.HoraFin,
            Cupo = item.Taller.Cupo,
            CuposDisponibles = item.CuposDisponibles
        };
    }
}
