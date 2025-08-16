using System.Text.Json.Serialization;
using TechSyence.Communication.Requests.WhatsApp;

namespace TechSyence.Communication.Requests;

public class RequestWhatsAppMessage
{
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    [JsonPropertyName("entry")]
    public IList<WhatsAppEntryRequest> Entry { get; set; } = [];
}
