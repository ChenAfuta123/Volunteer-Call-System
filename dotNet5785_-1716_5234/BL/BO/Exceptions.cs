namespace BO
{
    [Serializable]
    public class BlDoesNotExistsException : BOException
    {
        public BlDoesNotExistsException(string? message) : base(message) { }
        public BlDoesNotExistsException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    [Serializable]
    public class BlAlreadyExistsException : BOException
    {
        public BlAlreadyExistsException(string? message) : base(message) { }
        public BlAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    [Serializable]
    public class BlNullPropertyException : BOException
    {
        public BlNullPropertyException(string? message) : base(message) { }
    }

    [Serializable]
    public class BlValidationException : BOException
    {
        public BlValidationException(string? message) : base(message) { }
    }

    [Serializable]
    public class BlCannotBeDeletedException : BOException
    {
        public BlCannotBeDeletedException(string? message) : base(message) { }
    }

    [Serializable]
    public class BlUnauthorizedException : BOException
    {
        public BlUnauthorizedException(string? message) : base(message) { }
    }

    [Serializable]
    public class BlObjectNotFoundException : BOException
    {
        public BlObjectNotFoundException(string? message) : base(message) { }
    }
}
