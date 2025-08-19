using System.Text.Json.Serialization;

namespace TechSyence.Communication.Requests.WhatsApp;

public class WhatsAppValueRequest
{
    [JsonPropertyName("messaging_product")]
    public string MessagingProduct { get; set; } = string.Empty;

    [JsonPropertyName("metadata")]
    public WhatsAppMetaDataRequest MetaData { get; set; } = new();

    [JsonPropertyName("contacts")]
    public IList<WhatsAppContactsRequest> Contacts { get; set; } = [];

    [JsonPropertyName("messages")]
    public IList<WhatsAppMessagesRequest> Messages { get; set; } = [];
}

