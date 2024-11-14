namespace Dal;

/// <summary>
/// Static class that serves as the data source for storing lists of different entities.
/// This class provides collections for Volunteers, Assignments, and Calls entities.
/// </summary>
internal static class DataSource
{
    /// <summary>
    /// Gets the list of volunteers. This list is used to store instances of the Volunteer entity.
    /// </summary>
    internal static List<DO.Volunteer> Volunteers { get; } = new();

    /// <summary>
    /// Gets the list of assignments. This list is used to store instances of the Assignment entity.
    /// </summary>
    internal static List<DO.Assignment> Assignments { get; } = new();

    /// <summary>
    /// Gets the list of calls. This list is used to store instances of the Call entity.
    /// </summary>
    internal static List<DO.Call> Calls { get; } = new();
}
