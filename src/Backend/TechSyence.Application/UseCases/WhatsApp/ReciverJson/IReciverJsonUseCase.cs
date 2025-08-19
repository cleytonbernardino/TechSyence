using TechSyence.Communication;

namespace TechSyence.Application.UseCases.WhatsApp.ReciverJson;

public interface IReciverJsonUseCase
{
    Task Execute(RequestWhatsAppMessage request);
}
