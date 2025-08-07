namespace TechSyence.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : TechSyenceException
{
    public IList<string> ErrorMessages { get; set; }

    public ErrorOnValidationException(IList<string> errMessages) : base("")
    {
        ErrorMessages = errMessages;
    }
}
