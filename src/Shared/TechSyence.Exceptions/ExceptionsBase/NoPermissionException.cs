namespace TechSyence.Exceptions.ExceptionsBase;

public class NoPermissionException : TechSyenceException
{
    public NoPermissionException() : base(ResourceMessagesException.NO_PERMISSION) { }
    public NoPermissionException(string message) : base(message) { }
}
