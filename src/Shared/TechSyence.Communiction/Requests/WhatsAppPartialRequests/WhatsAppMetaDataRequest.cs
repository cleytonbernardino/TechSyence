using System.Text.Json.Serialization;

namespace TechSyence.Communication.Requests.WhatsApp;

public class WhatsAppMetaDataRequest
{
    [JsonPropertyName("display_phone_number")]
    public string DisplayPhone { get; set; } = string.Empty;
    [JsonPropertyName("phone_number_id")]
    public string PhoneNumberId { get; set; } = string.Empty;
}
