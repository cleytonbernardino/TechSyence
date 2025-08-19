namespace TechSyence.Domain.Dtos;

public record WhatsAppMessageDto
{
    public required string UserIndentifier { get; init; }
    public required string Messages { get; init; }
}
