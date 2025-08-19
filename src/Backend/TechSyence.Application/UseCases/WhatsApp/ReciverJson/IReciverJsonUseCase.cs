using TechSyence.Communication.Requests;

namespace TechSyence.Application.UseCases.WhatsApp.ReciverJson;

public interface IReciverJsonUseCase
{
    Task Execute(RequestWhatsAppMessage request);
}
