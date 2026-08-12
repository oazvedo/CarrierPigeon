namespace BirdMessage.Domain.Entities;

public class Bird
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // km/h
    public decimal Velocity { get; set; }
}
