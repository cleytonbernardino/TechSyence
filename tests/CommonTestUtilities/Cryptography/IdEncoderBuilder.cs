using Moq;
using Sqids;
using TechSyence.Application.Services.Encoder;

namespace CommonTestUtilities.Cryptography;

public class IdEncoderBuilder
{
    private readonly Mock<IIdEncoder> _mock = new();
    private readonly SqidsEncoder<long> _sqids = new(new SqidsOptions
    {
        MinLength = 3,
        Alphabet = "aJVu3P4s0foxAqivmTdrGO1ynS6eMtRLwEFzZkDgCNcj2IHpK7l5bXYWhUBQ89"
    });

    public IIdEncoder Build() => _mock.Object;

    public IdEncoderBuilder Encoder()
    {
        _mock.Setup(encoder => encoder.Encode(It.IsAny<long>())).Returns<long>(id =>_sqids.Encode(id));
        return this;
    }

    public IdEncoderBuilder Decode()
    {
        _mock.Setup(encoder => encoder.Decode(It.IsAny<string>())).Returns((string id) => _sqids.Decode(id).Single());
        return this;
    }
}
