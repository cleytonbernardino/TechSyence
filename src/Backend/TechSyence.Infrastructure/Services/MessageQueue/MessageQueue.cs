using System.Threading.Channels;
using TechSyence.Application.Services.MessageQueue;
using TechSyence.Domain.Dtos;

namespace TechSyence.Infrastructure.Services.MessageQueue;

public class MessageQueue : IMessageQueue
{
    private readonly Channel<WhatsAppMessageDto> _channel = Channel.CreateUnbounded<WhatsAppMessageDto>();

    public async Task<WhatsAppMessageDto> DequeueAsync(CancellationToken cancellationToken) => await _channel.Reader.ReadAsync(cancellationToken);

    public async Task EnqueueAsync(WhatsAppMessageDto message) => await _channel.Writer.WriteAsync(message);
}
