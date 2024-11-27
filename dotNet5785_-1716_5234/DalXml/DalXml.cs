using DalApi;

namespace Dal;

sealed public class DalXml : IDal
{
    /// <summary>
    /// Provides access to the volunteer operations (Create, Read, Update, Delete).
    /// </summary>
    public IVolunteer Volunteer { get; } = new VolunteerImplementation();

    /// <summary>
    /// Provides access to the call operations (Create, Read, Update, Delete).
    /// </summary>
    public ICall Call { get; } = new CallImplementation();

    /// <summary>
    /// Provides access to the assignment operations (Create, Read, Update, Delete).
    /// </summary>
    public IAssignment Assignment { get; } = new AssignmentImplementation();

    /// <summary>
    /// Provides access to the configuration operations.
    /// </summary>
    public IConfig Config { get; } = new ConfigImplementation();

    /// <summary>
    /// Resets the entire database by deleting all volunteers, calls, assignments, and resetting the configuration.
    /// </summary>
    public void ResetDB()
    {
        Volunteer.DeleteAll();
        Call.DeleteAll();
        Assignment.DeleteAll();
        Config.Reset();
    }
}
