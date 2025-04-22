using FluentAssertions;

namespace RPG.Combate.Tests;

public class CombateSpecifications
{
    [Fact]
    public void Cuando_se_crea_un_personaje_debe_tener_1000_de_vida()
    {
        var combate = new Combate();

        combate.AgregarPersonaje();

        Personaje personajeCreado = combate.ObtenerInformacionPersonaje();
        personajeCreado.Vida.Should().Be(1000);
    }
}

public class Combate
{
    private Personaje? personajeCreado; 
    public void AgregarPersonaje()
    {
        throw new NotImplementedException();
    }

    public Personaje ObtenerInformacionPersonaje()
    {
        throw new NotImplementedException();
    }
}

public class Personaje
{
    public object Vida { get; set; }
}