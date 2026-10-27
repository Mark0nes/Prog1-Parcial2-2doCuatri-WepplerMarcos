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

    public Inscripcion Inscribir(Inscripcion nueva)
    {
        Taller? taller = _tallerRepository.Leer().FirstOrDefault(t => t.Id == nueva.TallerId);
        if (taller == null)
            throw new KeyNotFoundException($"No existe el taller con id {nueva.TallerId}.");

        List<Inscripcion> inscripciones = _inscripcionRepository.Leer();
        List<Inscripcion> activas = inscripciones.Where(i => i.EstaActiva()).ToList();

        int activasEnElTaller = activas.Count(i => i.TallerId == taller.Id);
        if (activasEnElTaller >= taller.Cupo)
            throw new InvalidOperationException("El taller no tiene cupo disponible.");

        if (activas.Any(i => i.Dni == nueva.Dni && i.TallerId == nueva.TallerId))
            throw new InvalidOperationException("El alumno ya está inscripto en este taller.");

        List<Inscripcion> activasDelAlumno = activas.Where(i => i.Dni == nueva.Dni).ToList();
        if (activasDelAlumno.Count >= TOPE_TALLERES)
            throw new InvalidOperationException(
                $"El alumno ya tiene {TOPE_TALLERES} inscripciones activas, que es el máximo permitido.");

        List<Taller> talleres = _tallerRepository.Leer();
        foreach (Inscripcion activa in activasDelAlumno)
        {
            Taller? tallerInscripto = talleres.FirstOrDefault(t => t.Id == activa.TallerId);
            if (tallerInscripto != null && taller.SeSuperponeCon(tallerInscripto))
                throw new InvalidOperationException(
                    $"El taller {taller.Nombre} se superpone con {tallerInscripto.Nombre}, en el que el alumno ya está inscripto.");
        }

        nueva.FechaInscripcion = DateTime.Now;
        nueva.FechaBaja = null;

        nueva.Id = inscripciones.Count > 0 ? inscripciones.Max(i => i.Id) + 1 : 1;
        inscripciones.Add(nueva);
        _inscripcionRepository.Guardar(inscripciones);

        return nueva;
    }

    public Inscripcion DarDeBaja(int id)
    {
        List<Inscripcion> inscripciones = _inscripcionRepository.Leer();
        Inscripcion? inscripcion = inscripciones.FirstOrDefault(i => i.Id == id);

        if (inscripcion == null)
            throw new KeyNotFoundException($"No existe la inscripción con id {id}.");

        if (!inscripcion.EstaActiva())
            throw new InvalidOperationException("La inscripción ya fue dada de baja.");

        inscripcion.FechaBaja = DateTime.Now;
        _inscripcionRepository.Guardar(inscripciones);

        return inscripcion;
    }
}
