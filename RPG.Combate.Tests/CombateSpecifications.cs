using FluentAssertions;

namespace RPG.Combate.Tests;

public class CombateSpecifications : CombateTests
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
        Guid idPersonaje2 = Guid.NewGuid();
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

    [Fact]
    public void Si_NoHayMasDeUnPersonaje_NoDebe_InflingirDaño_y_debe_arrojar_ArgumentException()
    {
        var combate = new Combate();
        var personaje1Id = Guid.NewGuid();
        combate.AgregarPersonaje(personaje1Id);

        var caller = () => combate.InfligirDaño(personaje1Id, Guid.NewGuid(),100);

        caller.Should().ThrowExactly<ArgumentException>().WithMessage("No existe el personaje");
    }

    [Fact]
    public void Si_UnPersonajeInflingeDañoAOtroQueNoExiste_Debe_ArrojarArgumentException()
    {
        var combate = new Combate();
        var personaje1Id = Guid.NewGuid();
        combate.AgregarPersonaje(personaje1Id);
        var personaje2Id = Guid.NewGuid();
        combate.AgregarPersonaje(personaje2Id);
        
        var caller = () => combate.InfligirDaño(personaje1Id, Guid.NewGuid(), 100);
        
        caller.Should().ThrowExactly<ArgumentException>().WithMessage("No puede infligir daño, no existe el personaje agresor o afectado");
    }

    [Fact]
    public void Si_UnPersonajeInflingeDañoAOtroQueEstaMuerto_Debe_ArrojarInvalidOperationException()
    {
        var combate = new Combate();
        var personaje1Id = Guid.NewGuid();
        combate.AgregarPersonaje(personaje1Id);
        var personaje2Id = Guid.NewGuid();
        combate.AgregarPersonaje(personaje2Id);
        
        combate.InfligirDaño(personaje1Id, personaje2Id, 100);

        var personajeAfectado = combate.ObtenerInformacionPersonaje(personaje2Id);
        personajeAfectado.Vida.Should().Be(900);
    }
}

public class Combate
{
    public Guid Id { get; private set; }
    
    private List<Personaje> _personajes = [];

    public Combate()
    {
        Id =  Guid.NewGuid();
    }
    public void AgregarPersonaje(Guid id)
    {
        _personajes.Add(new(id, 1000, EstadoPersonaje.Vivo));
    }

    public Personaje ObtenerInformacionPersonaje(Guid idPersonaje)
        => _personajes.Find(per => per.Id == idPersonaje)!;

    public void InfligirDaño(Guid personajeAgresorId, Guid personajeAfectadoId, int daño)
    {
        ValidarSiExisteMasDeUnPersonaje();
        ValidarExistenciaDePersonajes(personajeAgresorId, personajeAfectadoId);
    }

    private void ValidarSiExisteMasDeUnPersonaje()
    {
        if(_personajes.Count is 1 )
            throw new ArgumentException("No existe el personaje");
    }

    private void ValidarExistenciaDePersonajes(Guid personajeAgresorId, Guid personajeAfectadoId)
    {
        var existePersonajeAgresor = _personajes.Any(per => per.Id == personajeAgresorId);
        var existePersonajeAfectado = _personajes.Any(per => per.Id == personajeAfectadoId);
        if (existePersonajeAgresor || existePersonajeAfectado)
            throw new ArgumentException("No puede infligir daño, no existe el personaje agresor o afectado");
    }
}

public record Personaje(Guid Id, int Vida, EstadoPersonaje Estado);

public enum EstadoPersonaje
{
    Vivo,
    Muerto
}