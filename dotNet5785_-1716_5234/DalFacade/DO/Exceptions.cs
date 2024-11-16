

namespace DO;

[Serializable]
public class DalDoesNotExistsException : Exception
{
    public DalDoesNotExistsException(string? message) : base(message) { }
}
[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}

