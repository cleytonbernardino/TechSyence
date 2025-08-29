namespace CommonTestUtilities.Cryptography;

public class IdEncoderForTests : IdEncoderBase
{
    public string Encode(long id) => _sqids.Encode(id);
    public long Decode(string id) => _sqids.Decode(id).Single();
}
