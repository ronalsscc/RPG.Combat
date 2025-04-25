namespace RPG.Combate.Tests;

public record Personaje(Guid Id, int Vida, EstadoPersonaje Estado)
{
    public Guid Id { get; init; } = Id;
    public int Vida { get; private set; } = Vida;
    public EstadoPersonaje Estado { get; private set; } = Estado;

    public void RecibirDaño(int daño)
    {
        Vida -= daño;
        if (Vida == 0)
            Estado = EstadoPersonaje.Muerto;
    }

}