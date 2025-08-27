using Moq;
using Sqids;
using System.Configuration;
using TechSyence.Application.Services.Encoder;

namespace CommonTestUtilities.Cryptography;

public class IdEncoderBuilder
{
    public static IIdEncoder Build()
    {
        Mock<IIdEncoder> mock = new();

        mock.Setup(idEncoder => idEncoder.Encode(1)).Returns("yyy");
        mock.Setup(idEncoder => idEncoder.Decode("yyy")).Returns(1);

        return mock.Object;
    }
}
