namespace DO;

[Serializable]
/// <summary>
/// Exception for cases where a DAL (Data Access Layer) entity does not exist.
/// </summary>
public class DalDoesNotExistsException : Exception
{
    /// <summary>
    /// Initializes the exception with a specific error message.
    /// </summary>
    public DalDoesNotExistsException(string? message) : base(message) { }
}

[Serializable]
/// <summary>
/// Exception for cases where a DAL entity already exists.
/// </summary>
public class DalAlreadyExistsException : Exception
{
    /// <summary>
    /// Initializes the exception with a specific error message.
    /// </summary>
    public DalAlreadyExistsException(string? message) : base(message) { }
}

[Serializable]

public class DalXMLFileLoadCreateException : Exception
{
    
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}