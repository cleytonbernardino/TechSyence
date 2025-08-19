using Bogus;
using TechSyence.Communication.Requests;
using TechSyence.Communication.Requests.WhatsApp;

namespace CommonTestUtilities.Requests;

public class RequestWhatsAppMessageBuilder
{
    public static RequestWhatsAppMessage Build()
    {
        return new Faker<RequestWhatsAppMessage>()
            .RuleFor(req => req.Object, () => "whatsapp_business_account")
            .RuleFor(req => req.Entry, () => new List<WhatsAppEntryRequest> { CreateEntryObject() })
            .Generate();
    }

    private static WhatsAppEntryRequest CreateEntryObject()
    {
        return new Faker<WhatsAppEntryRequest>()
            .RuleFor(req => req.Id, f => f.Random.Int().ToString())
            .RuleFor(req => req.Changes, () => new List<WhatsAppChangesRequest> { CreateChangesObject() })
            .Generate();
    }

    private static WhatsAppChangesRequest CreateChangesObject()
    {
        return new Faker<WhatsAppChangesRequest>()
            .RuleFor(req => req.Field, f => f.Random.Word())
            .RuleFor(req => req.Value, () => CreateValueObject())
            .Generate();
    }

    private static WhatsAppValueRequest CreateValueObject()
    {
        return new Faker<WhatsAppValueRequest>()
            .RuleFor(req => req.MessagingProduct, f => f.Random.Word())
            .RuleFor(req => req.MetaData, () => CreateMetaDataObject())
            .RuleFor(req => req.Contacts, () => new List<WhatsAppContactsRequest> { CreateContactsObject() })
            .RuleFor(req => req.Messages, () => new List<WhatsAppMessagesRequest> { CreateMessagesObject() })
            .Generate();
    }

    private static WhatsAppMetaDataRequest CreateMetaDataObject()
    {
        return new Faker<WhatsAppMetaDataRequest>()
            .RuleFor(req => req.DisplayPhone, f => f.Phone.PhoneNumber("###########"))
            .RuleFor(req => req.PhoneNumberId, f => f.Random.Int(100000000).ToString())
            .Generate();
    }

    private static WhatsAppContactsRequest CreateContactsObject()
    {
        var profile = new Faker<WhatsAppProfileObject>()
            .RuleFor(profile => profile.Name, f => f.Name.FirstName())
            .Generate();

        return new Faker<WhatsAppContactsRequest>()
            .RuleFor(req => req.Profile, () => profile)
            .RuleFor(req => req.WaId, f => f.Phone.PhoneNumberFormat(11))
            .Generate();
    }

    private static WhatsAppMessagesRequest CreateMessagesObject()
    {
        var textBody = new Faker<WhatsAppTextObject>()
            .RuleFor(text => text.Body, f => f.Lorem.Paragraph())
            .Generate();

        return new Faker<WhatsAppMessagesRequest>()
            .RuleFor(req => req.From, f => f.Phone.PhoneNumber("###########"))
            .RuleFor(req => req.Id, f => f.Random.AlphaNumeric(11))
            .RuleFor(req => req.Timestamp, f => f.Random.AlphaNumeric(11))
            .RuleFor(req => req.Type, () => "text")
            .RuleFor(req => req.Text, () => textBody)
            .Generate();
    }
}
