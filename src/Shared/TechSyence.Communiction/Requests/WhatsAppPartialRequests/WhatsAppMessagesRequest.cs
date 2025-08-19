using System.Text.Json.Serialization;
using TechSyence.Communication.Requests.WhatsAppPartialRequests;

namespace TechSyence.Communication.Requests.WhatsApp;

public class WhatsAppMessagesRequest : WhatsAppMessageHeader
{
    [JsonPropertyName("context")]
    public WhatsAppMessageHeader? Context { get; set; }

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public WhatsAppTextObject? Text { get; set; }
}

public class WhatsAppTextObject
{
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;
}