namespace BO;
[Serializable]
public class BlDoesNotExistsException : Exception
{
    public BlDoesNotExistsException(string? message) : base(message) { }
    public BlDoesNotExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}
[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}
[Serializable]
public class BlValidationException : Exception
{
    public BlValidationException(string? message) : base(message) { }
}
[Serializable]
public class BlCannotBeDeletedException : Exception
{
    public BlCannotBeDeletedException(string? message) : base(message) { }
}
[Serializable]
public class BlUnauthorizedException : Exception
{
    public BlUnauthorizedException(string? message) : base(message) { }
}
[Serializable]
public class BlObjectNotFoundException : Exception
{
   
    public BlObjectNotFoundException(string? message) : base(message) { }
}