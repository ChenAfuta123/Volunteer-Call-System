namespace BO
{
    /// <summary>Exception thrown when a requested object does not exist.</summary>
    [Serializable]
    public class BlDoesNotExistsException : BOException
    {
        public BlDoesNotExistsException(string? message) : base(message) { }
        public BlDoesNotExistsException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    /// <summary>Exception thrown when an object already exists.</summary>
    [Serializable]
    public class BlAlreadyExistsException : BOException
    {
        public BlAlreadyExistsException(string? message) : base(message) { }
        public BlAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    /// <summary>Exception thrown when a required property is null.</summary>
    [Serializable]
    public class BlNullPropertyException : BOException
    {
        public BlNullPropertyException(string? message) : base(message) { }
    }

    /// <summary>Exception thrown for validation errors in business logic.</summary>
    [Serializable]
    public class BlValidationException : BOException
    {
        public BlValidationException(string? message) : base(message) { }
    }

    /// <summary>Exception thrown when an object cannot be deleted.</summary>
    [Serializable]
    public class BlCannotBeDeletedException : BOException
    {
        public BlCannotBeDeletedException(string? message) : base(message) { }
    }

    /// <summary>Exception thrown for unauthorized operations.</summary>
    [Serializable]
    public class BlUnauthorizedException : BOException
    {
        public BlUnauthorizedException(string? message) : base(message) { }
    }

    /// <summary>Exception thrown when a specified object cannot be found.</summary>
    [Serializable]
    public class BlObjectNotFoundException : BOException
    {
        public BlObjectNotFoundException(string? message) : base(message) { }
    }
   
    public class BLTemporaryNotAvailableException : BOException
    {
        public BLTemporaryNotAvailableException(string? message) : base(message) { }
    }
}
