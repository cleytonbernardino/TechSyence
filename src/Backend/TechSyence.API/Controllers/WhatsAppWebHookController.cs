using Microsoft.AspNetCore.Mvc;
using TechSyence.Application.Services.MessageQueue;
using TechSyence.Communication;
using TechSyence.Domain.Dtos;

namespace TechSyence.API.Controllers;

public class WhatsappWebHookController(
    IMessageQueue queue
    ) : TechSyenceBaseController
{
    private readonly IMessageQueue _queue = queue;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReciverMessage(
        [FromBody] RequestWhatsAppMessage request
        )
    {
        //if (request.Entry.Count == 0)
        //    return BadRequest();

        //// Verificação Fraca, serve apenas para testes da api
        //// Refinamente é extremamente necessario
        //var dto = new WhatsAppMessageDto
        //{
        //    Messages = request.Entry[0].Changes[0].Value.Messages[0].Text.Body,
        //    UserIndentifier = request.Entry[0].Changes[0].Value.Messages[0].From
        //};

        //await _queue.EnqueueAsync(dto);
        //return Accepted();

        return NotFound();
    }
}
