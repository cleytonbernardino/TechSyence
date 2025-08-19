using System.Text.Json.Serialization;

namespace TechSyence.Communication.Requests.WhatsAppPartialRequests;

public class WhatsAppMessageHeader
{
    [JsonPropertyName("from")]
    public string From { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}
