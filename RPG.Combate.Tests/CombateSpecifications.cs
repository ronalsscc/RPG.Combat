using FluentAssertions;

namespace RPG.Combate.Tests;

public class CombateSpecifications : CombateTests
{
    [Fact]
    public void Cuando_se_crea_un_personaje_debe_tener_1000_de_vida_y_estado_vivo()
    {
        var combate = new Combate();
        var personajeCreadoId = combate.AgregarPersonaje();

        Personaje personajeCreado = combate.ObtenerInformacionPersonaje(personajeCreadoId);
        personajeCreado.Vida.Should().Be(1000);
        personajeCreado.Estado.Should().Be(EstadoPersonaje.Vivo);
    }

    [Fact]
    public void Cuando_se_crean_dos_personajes_deben_tener_1000_de_vida_y_estar_en_estado_Vivo()
    {
        var combate = new Combate();
        var personaje1Id = combate.AgregarPersonaje();
        
        var personaje2Id = combate.AgregarPersonaje();

        Personaje personajeCreado1 = combate.ObtenerInformacionPersonaje(personaje1Id);
        Personaje personajeCreado2 = combate.ObtenerInformacionPersonaje(personaje2Id);
        personajeCreado1.Vida.Should().Be(1000);
        personajeCreado1.Estado.Should().Be(EstadoPersonaje.Vivo);
        personajeCreado1.Id.Should().Be(personaje1Id);
        personajeCreado2.Vida.Should().Be(1000);
        personajeCreado2.Estado.Should().Be(EstadoPersonaje.Vivo);
        personajeCreado2.Id.Should().Be(personaje2Id);
    }

    [Fact]
    public void Si_NoHayMasDeUnPersonaje_NoDebe_InflingirDaño_y_debe_arrojar_ArgumentException()
    {
        var combate = new Combate();
        var personajeId = combate.AgregarPersonaje();

        var caller = () => combate.InfligirDaño(personajeId, Guid.CreateVersion7(),100);

        caller.Should().ThrowExactly<ArgumentException>().WithMessage("Para inflingir daño deben existir al menos dos personajes.");
    }

    [Fact]
    public void Si_UnPersonajeInflingeDañoAOtroQueNoExiste_Debe_ArrojarArgumentException()
    {
        var combate = new Combate();
        var personaje1Id= combate.AgregarPersonaje();
        var personaje2Id= combate.AgregarPersonaje();
        
        var caller = () => combate.InfligirDaño(personaje1Id, Guid.CreateVersion7(), 100);
        
        caller.Should().ThrowExactly<ArgumentException>().WithMessage("No existe el personaje");
    }

    [Theory]
    [InlineData(100, 900)]
    [InlineData(500, 500)]
    [InlineData(300, 700)]
    [InlineData(1000, 0)]
    public void Si_UnPersonajeInflingeDañoAOtro_Debe_DisminuirSuVida(int daño, int vidaRestante)
    {
        var combate = new Combate();
        var personajeAgresorId = combate.AgregarPersonaje();
        var personajeAfectadoId = combate.AgregarPersonaje();
        
        combate.InfligirDaño(personajeAgresorId, personajeAfectadoId, daño);

        var personajeAfectado = combate.ObtenerInformacionPersonaje(personajeAfectadoId);
        personajeAfectado.Vida.Should().Be(vidaRestante);
    }

    [Fact]
    public void Si_UnPersonajeInflingeElMismoDañoComoSaludTieneElAfectadoElPersonajeAfectado_Debe_Morir()
    {
        var combate = new Combate();
        var personajeAgresorId = combate.AgregarPersonaje();
        var personajeAfectadoId = combate.AgregarPersonaje();
        
        combate.InfligirDaño(personajeAgresorId, personajeAfectadoId, 1000);

        var personajeAfectado = combate.ObtenerInformacionPersonaje(personajeAfectadoId);
        personajeAfectado.Vida.Should().Be(0);
        personajeAfectado.Estado.Should().Be(EstadoPersonaje.Muerto);
    }
}

public enum EstadoPersonaje
{
    Vivo,
    Muerto
}