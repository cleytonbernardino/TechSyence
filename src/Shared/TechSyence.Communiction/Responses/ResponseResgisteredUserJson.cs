namespace TechSyence.Communiction.Responses;

public class ResponseResgisteredUserJson
{
    public string FirstName { get; set; } = string.Empty;
    public ResponseTokenJson Tokens { get; set; } = default!;
}
