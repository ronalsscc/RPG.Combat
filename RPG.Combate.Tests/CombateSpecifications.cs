using FluentAssertions;

namespace RPG.Combate.Tests;

public class CombateSpecifications : CombateTests
{
    private Combate _combate;

    public CombateSpecifications()
    {
        _combate = new Combate();
    }

    [Fact]
    public void Cuando_se_crea_un_personaje_debe_tener_1000_de_vida_y_estado_vivo()
    {
        var personajeCreadoId = _combate.AgregarPersonaje();

        Personaje personajeCreado = _combate.ObtenerInformacionPersonaje(personajeCreadoId);
        personajeCreado.Vida.Should().Be(1000);
        personajeCreado.Estado.Should().Be(EstadoPersonaje.Vivo);
    }

    [Fact]
    public void Cuando_se_crean_dos_personajes_deben_tener_1000_de_vida_y_estar_en_estado_Vivo()
    {
        var (personaje1Id, personaje2Id) = AgregarDosPersonajesAlCombate(_combate);

        Personaje personajeCreado1 = _combate.ObtenerInformacionPersonaje(personaje1Id);
        Personaje personajeCreado2 = _combate.ObtenerInformacionPersonaje(personaje2Id);

        ValidarEstadoInicialPersonaje(personajeCreado1, personaje1Id);
        ValidarEstadoInicialPersonaje(personajeCreado2, personaje2Id);
    }

    private static void ValidarEstadoInicialPersonaje(Personaje personaje, Guid id)
    {
        personaje.Vida.Should().Be(1000);
        personaje.Estado.Should().Be(EstadoPersonaje.Vivo);
        personaje.Id.Should().Be(id);
    }

    [Fact]
    public void Si_UnPersonajeIntentarInflingirseDañoASiMismo_Debe_Arrojar_InvalidOperationException()
    {
        var personajeId = _combate.AgregarPersonaje();

        var caller = () => _combate.InfligirDaño(personajeId, personajeId, 100);

        caller.Should().ThrowExactly<InvalidOperationException>().WithMessage("No puede inflingirse daño a si mismo");
    }

    [Fact]
    public void Si_UnPersonajeInflingeDañoAOtroQueNoExiste_Debe_ArrojarArgumentException()
    {
        var personajeId = _combate.AgregarPersonaje();

        var caller = () => _combate.InfligirDaño(personajeId, Guid.CreateVersion7(), 100);

        caller.Should().ThrowExactly<ArgumentException>().WithMessage("No existe el personaje");
    }

    [Theory]
    [InlineData(100, 900)]
    [InlineData(500, 500)]
    [InlineData(300, 700)]
    [InlineData(1000, 0)]
    public void Si_UnPersonajeInflingeDañoAOtro_Debe_DisminuirSuVida(int daño, int vidaRestante)
    {
        var (personajeAfectadoId, personajeAgresorId) = AgregarDosPersonajesAlCombate(_combate);
        _combate.InfligirDaño(personajeAgresorId, personajeAfectadoId, daño);

        var personajeAfectado = _combate.ObtenerInformacionPersonaje(personajeAfectadoId);
        personajeAfectado.Vida.Should().Be(vidaRestante);
    }

    [Theory]
    [InlineData(1000)]
    [InlineData(2000)]
    [InlineData(3000)]
    [InlineData(4000)]
    public void Si_UnPersonajeInflingeElMismoDañoComoSaludTieneElAfectadoElPersonajeAfectado_Debe_Morir(int daño)
    {
        var (personajeAfectadoId, personajeAgresorId) = AgregarDosPersonajesAlCombate(_combate);
        _combate.InfligirDaño(personajeAgresorId, personajeAfectadoId, daño);

        var personajeAfectado = _combate.ObtenerInformacionPersonaje(personajeAfectadoId);
        personajeAfectado.Vida.Should().Be(1000 - daño);
        personajeAfectado.Estado.Should().Be(EstadoPersonaje.Muerto);
    }

    [Fact]
    public void Si_UnPersonajeIntentaInflingirDañoAUnPersonajeYaMuerto_Debe_ArrojarInvalidOperationException()
    {
        var (personajeAfectadoId, personajeAgresorId) = AgregarDosPersonajesAlCombate(_combate);
        _combate.InfligirDaño(personajeAgresorId, personajeAfectadoId, 1000);

        var caller = () => _combate.InfligirDaño(personajeAgresorId, personajeAfectadoId, 500);

        caller.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage("No se puede inflingir daño a un personaje muerto.");
    }
}

public enum EstadoPersonaje
{
    Vivo,
    Muerto
}