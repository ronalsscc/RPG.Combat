using FluentAssertions;

namespace RPG.Combate.Tests;

public class CombateSpecifications
{
    [Fact]
    public void Cuando_se_crea_un_personaje_debe_tener_1000_de_vida_y_estado_vivo()
    {
        var combate = new Combate();
        Guid id = Guid.NewGuid();
        combate.AgregarPersonaje(id);

        Personaje personajeCreado = combate.ObtenerInformacionPersonaje(id);
        personajeCreado.Vida.Should().Be(1000);
        personajeCreado.Estado.Should().Be(EstadoPersonaje.Vivo);
    }

    [Fact]
    public void Cuando_se_crean_dos_personajes_deben_tener_1000_de_vida_y_estar_en_estado_Vivo()
    {
        var combate = new Combate();
        Guid idPersonaje1 = Guid.NewGuid();
        Guid idPersonaje2= Guid.NewGuid();
        combate.AgregarPersonaje(idPersonaje1);
        
        combate.AgregarPersonaje(idPersonaje2);
        
        Personaje personajeCreado1 = combate.ObtenerInformacionPersonaje(idPersonaje1);
        Personaje personajeCreado2 = combate.ObtenerInformacionPersonaje(idPersonaje2);
        
        personajeCreado1.Vida.Should().Be(1000);
        personajeCreado1.Estado.Should().Be(EstadoPersonaje.Vivo);
        personajeCreado1.Id.Should().Be(idPersonaje1);
        personajeCreado2.Vida.Should().Be(1000);
        personajeCreado2.Estado.Should().Be(EstadoPersonaje.Vivo);
        personajeCreado2.Id.Should().Be(idPersonaje2);
    }
}

public class Combate
{
    private Personaje? _personajeCreado;

    public void AgregarPersonaje(Guid id)
    {
        _personajeCreado = new Personaje(id, 1000, EstadoPersonaje.Vivo);
    }

    public Personaje? ObtenerInformacionPersonaje(Guid idPersonaje1) => _personajeCreado;
    
}

public record Personaje(Guid Id, int Vida, EstadoPersonaje Estado);
public enum EstadoPersonaje
{
    Vivo,
    Muerto
}
