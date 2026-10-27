using Talleres.Logica.Data;

namespace Talleres.Logica;

public class InscripcionService
{

    private const int TOPE_TALLERES = 2;

    private readonly TallerRepository _tallerRepository;
    private readonly InscripcionRepository _inscripcionRepository;

    public InscripcionService()
        : this(new TallerRepository("talleres.json"), new InscripcionRepository("inscripciones.json"))
    {
    }

    public InscripcionService(TallerRepository tallerRepository, InscripcionRepository inscripcionRepository)
    {
        _tallerRepository = tallerRepository;
        _inscripcionRepository = inscripcionRepository;

        _tallerRepository.CrearArchivoSiNoExiste();
    }

    public List<TallerConCupos> ObtenerTalleresConCupos()
    {
        List<Inscripcion> activas = _inscripcionRepository.Leer()
            .Where(i => i.EstaActiva())
            .ToList();

        return _tallerRepository.Leer()
            .Select(t => new TallerConCupos
            {
                Taller = t,
                CuposDisponibles = t.Cupo - activas.Count(i => i.TallerId == t.Id)
            })
            .ToList();
    }

    public List<Inscripcion> ObtenerInscripciones(int? dni = null)
    {
        List<Inscripcion> inscripciones = _inscripcionRepository.Leer();

        if (dni.HasValue)
            inscripciones = inscripciones.Where(i => i.Dni == dni.Value).ToList();

        return inscripciones;
    }
}
