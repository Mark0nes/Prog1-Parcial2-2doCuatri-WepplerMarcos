using Talleres.Logica;
using Talleres.Logica.Data;

namespace Talleres.Tests;

[TestFixture]
public class InscripcionServiceTests
{
    //   1 Robótica   Lunes     18-20 cupo 3
    //   2 Fotografía Lunes     20-22 cupo 2
    //   3 Ajedrez    Lunes     19-21 cupo 4
    //   4 Teatro     Miércoles 18-20 cupo 2
    private static InscripcionService CrearServicioAislado()
    {
        string sufijo = Guid.NewGuid().ToString("N");
        var tallerRepository = new TallerRepository($"talleres_test_{sufijo}.json");
        var inscripcionRepository = new InscripcionRepository($"inscripciones_test_{sufijo}.json");
        return new InscripcionService(tallerRepository, inscripcionRepository);
    }

    private static Inscripcion Pedido(int tallerId, int dni, string nombre)
    {
        return new Inscripcion { TallerId = tallerId, Dni = dni, NombreAlumno = nombre };
    }

    [Test]
    public void Inscribir_TallerSinCupo_LanzaInvalidOperationException_Caso26()
    {
        InscripcionService servicio = CrearServicioAislado();
        servicio.Inscribir(Pedido(2, 40111222, "Ana"));
        servicio.Inscribir(Pedido(2, 40333444, "Bruno"));

        Inscripcion tercera = Pedido(2, 40555666, "Carla");

        Assert.Throws<InvalidOperationException>(() => servicio.Inscribir(tercera));
    }

    [Test]
    public void Inscribir_TallerQueEmpiezaCuandoTerminaOtro_SeRegistra_Caso26()
    {
        InscripcionService servicio = CrearServicioAislado();
        servicio.Inscribir(Pedido(1, 40111222, "Ana"));

        Inscripcion resultado = servicio.Inscribir(Pedido(2, 40111222, "Ana"));

        Assert.That(resultado.EstaActiva(), Is.True);
    }
}
