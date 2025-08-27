namespace TechSyence.Application.Services.Encoder;

public interface IIdEncoder
{
    string Encode(long id);
    long Decode(string encryptedId);
}
