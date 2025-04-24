namespace RPG.Combate.Tests;

public record Personaje(Guid id, int vida, EstadoPersonaje estado)
{
    public Guid Id { get; init; } = id;
    public int Vida { get; private set; } = vida;
    public EstadoPersonaje Estado { get; init; } = estado;

    public void RecibirDaño(int daño)
    {
        this.Vida -= daño;
    }

}