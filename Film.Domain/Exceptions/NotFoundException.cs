namespace Film.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string resourceType, string resourceIdentifier) : base($"{resourceType} with id {resourceIdentifier} not found")
    {

    }
    public NotFoundException(string message) : base(message)
    {

    }
}
