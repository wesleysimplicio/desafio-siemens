namespace BookManager.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string entity, object key)
        : base($"{entity} with id '{key}' was not found.") { }
}

public class ConflictException : DomainException
{
    public ConflictException(string message) : base(message) { }
}

public class BusinessException : DomainException
{
    public BusinessException(string message) : base(message) { }
}
