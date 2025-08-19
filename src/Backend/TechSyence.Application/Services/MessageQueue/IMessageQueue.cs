using TechSyence.Domain.Dtos;

namespace TechSyence.Application.Services.MessageQueue;

public interface IMessageQueue
{
    Task EnqueueAsync(WhatsAppMessageDto message);
    Task<WhatsAppMessageDto> DequeueAsync(CancellationToken cancellationToken);
}
