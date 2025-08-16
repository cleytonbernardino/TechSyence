namespace TechSyence.Communication.Responses;

public class ResponseResgisteredUserJson
{
    public string FirstName { get; set; } = string.Empty;
    public ResponseTokenJson Tokens { get; set; } = default!;
}
