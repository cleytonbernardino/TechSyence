using Bogus;
using TechSyence.Domain.Dtos;

namespace CommonTestUtilities.Dto;
public class WhatsAppMessageDtoBuilder
{
    public static WhatsAppMessageDto Build()
    {
        return new Faker<WhatsAppMessageDto>()
            .RuleFor(dto => dto.Messages, f => f.Lorem.Paragraph())
            .RuleFor(dto => dto.UserIndentifier, f => f.Phone.PhoneNumber("###########"));
    }
}
