using System.Text.Json.Serialization;

namespace TechSyence.Communication.Requests.WhatsApp;

public class WhatsAppContactsRequest
{
    [JsonPropertyName("profile")]
    public WhatsAppProfileObject Profile { get; set; } = new();

    [JsonPropertyName("wa_id")]
    public string WaId { get; set; } = string.Empty;
}

public class WhatsAppProfileObject
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}