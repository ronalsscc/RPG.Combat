using FluentAssertions;

namespace RPG.Combate.Tests;

public class CombateSpecifications
{
    [Fact]
    public void Cuando_se_crea_un_personaje_debe_tener_1000_de_vida()
    {
        var combate = new Combate();
        Guid id = Guid.NewGuid();
        combate.AgregarPersonaje(id);

        Personaje personajeCreado = combate.ObtenerInformacionPersonaje();
        personajeCreado.Vida.Should().Be(1000);
    }
}

public class Combate
{
    private Personaje? _personajeCreado;

    public void AgregarPersonaje(Guid id)
    {
        _personajeCreado = new Personaje(id, 1000, EstadoPersonaje.Vivo);
    }

    public Personaje? ObtenerInformacionPersonaje() => _personajeCreado;
    
}

public record Personaje(Guid Id, int Vida, EstadoPersonaje Estado);
public enum EstadoPersonaje
{
    Vivo,
    Muerto
}
