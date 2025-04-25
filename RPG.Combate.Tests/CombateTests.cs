namespace RPG.Combate.Tests;

public abstract class CombateTests 
{
    protected (Guid Personaje1Id, Guid Personaje2Id) AgregarDosPersonajesAlCombate(Combate combate)
    {
        var personaje1Id = combate.AgregarPersonaje();
        var personaje2Id = combate.AgregarPersonaje();
        return (personaje1Id, personaje2Id);
    }
    
    
}