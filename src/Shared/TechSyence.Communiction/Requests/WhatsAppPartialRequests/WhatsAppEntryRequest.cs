using System.Text.Json.Serialization;

namespace TechSyence.Communication.Requests.WhatsApp;

public class WhatsAppEntryRequest
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("changes")]
    public IList<WhatsAppChangesRequest> Changes { get; set; } = [];
}
