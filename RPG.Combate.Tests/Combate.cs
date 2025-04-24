namespace RPG.Combate.Tests;

public class Combate
{
    public Guid Id { get; private set; }
    
    private List<Personaje> _personajes = [];

    public Combate()
    {
        Id =  Guid.NewGuid();
    }
    public Guid AgregarPersonaje()
    {
        var id = Guid.CreateVersion7();
        _personajes.Add(new(id, 1000, EstadoPersonaje.Vivo));
        return id;
    }
    
    public void InfligirDaño(Guid personajeAgresorId, Guid personajeAfectadoId, int daño)
    {
        ExisteMasDeUnPersonaje();
        var agresor = ObtenerInformacionPersonaje(personajeAgresorId);
        var afectado = ObtenerInformacionPersonaje(personajeAfectadoId);
        afectado.RecibirDaño(daño);
    }

    private void ExisteMasDeUnPersonaje()
    {
        if(_personajes.Count is 1 )
            throw new ArgumentException("Para inflingir daño deben existir al menos dos personajes.");
    }
    
    public Personaje ObtenerInformacionPersonaje(Guid idPersonaje)
        => _personajes.FirstOrDefault(per => per.Id == idPersonaje)
           ?? throw new ArgumentException("No existe el personaje");
}