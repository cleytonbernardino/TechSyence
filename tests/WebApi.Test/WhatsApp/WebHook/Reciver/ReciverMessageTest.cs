using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;

namespace WebApi.Test.WhatsApp.WebHook.Reciver;

public class ReciverMessageTest(
    CustomWebApplicationFactory factory
    ) : TechSyenceClassFixture(factory)
{

    private const string METHOD = "WhatsappWebHook";

    [Fact]
    public async Task Success()
    {
        var request = RequestWhatsAppMessageBuilder.Build();

        var response = await DoPostAsync(METHOD, request);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
