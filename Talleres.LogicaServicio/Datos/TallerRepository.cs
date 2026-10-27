using Newtonsoft.Json;

namespace Talleres.Logica.Data;

public class TallerRepository
{
    private readonly string _rutaArchivo;

    public TallerRepository(string rutaArchivo)
    {
        _rutaArchivo = rutaArchivo;
    }

    public List<Taller> Leer()
    {
        CrearArchivoSiNoExiste();

        string json = File.ReadAllText(_rutaArchivo);
        if (string.IsNullOrWhiteSpace(json))
            return new List<Taller>();

        return JsonConvert.DeserializeObject<List<Taller>>(json) ?? new List<Taller>();
    }

    public void CrearArchivoSiNoExiste()
    {
        if (File.Exists(_rutaArchivo))
            return;

        var talleresIniciales = new List<Taller>
        {
            new Taller { Id = 1, Nombre = "Robótica",   Dia = "Lunes",     HoraInicio = 18, HoraFin = 20, Cupo = 3 },
            new Taller { Id = 2, Nombre = "Fotografía", Dia = "Lunes",     HoraInicio = 20, HoraFin = 22, Cupo = 2 },
            new Taller { Id = 3, Nombre = "Ajedrez",    Dia = "Lunes",     HoraInicio = 19, HoraFin = 21, Cupo = 4 },
            new Taller { Id = 4, Nombre = "Teatro",     Dia = "Miércoles", HoraInicio = 18, HoraFin = 20, Cupo = 2 }
        };

        string json = JsonConvert.SerializeObject(talleresIniciales, Formatting.Indented);
        File.WriteAllText(_rutaArchivo, json);
    }
}
