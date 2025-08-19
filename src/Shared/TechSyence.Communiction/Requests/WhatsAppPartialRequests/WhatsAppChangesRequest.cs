using System.Text.Json.Serialization;

namespace TechSyence.Communication.Requests.WhatsApp;

public class WhatsAppChangesRequest
{
    [JsonPropertyName("value")]
    public WhatsAppValueRequest Value { get; set; } = new();


    [JsonPropertyName("field")]
    public string Field { get; set; } = string.Empty;
}
