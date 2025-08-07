namespace TechSyence.Domain.Entities;

public class EntityBase
{
    public long Id { get; set; }
    public DateTime CreatedOn { get; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;
}
