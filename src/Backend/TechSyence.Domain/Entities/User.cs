namespace TechSyence.Domain.Entities;

public class User : EntityBase
{
    public bool IsAdmin { get; set; } = false;
    public DateTime UpdatedOn { get; set; }
    public Guid UserIndentifier { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
