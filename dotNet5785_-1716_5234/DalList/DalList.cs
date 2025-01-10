namespace Dal;
using DalApi;

/// <summary>
/// Implementation of the IDal interface using in-memory data storage.
/// Provides access to various data-related functionalities.
/// </summary>
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }
    /// <summary>
    /// Provides access to Call-related operations.
    /// </summary>
    public ICall Call { get; } = new CallImplementation();

    /// <summary>
    /// Provides access to Volunteer-related operations.
    /// </summary>
    public IVolunteer Volunteer { get; } = new VolunteerImplementation();

    /// <summary>
    /// Provides access to Assignment-related operations.
    /// </summary>
    public IAssignment Assignment { get; } = new AssignmentImplementation();

    /// <summary>
    /// Provides access to configuration settings and related operations.
    /// </summary>
    public IConfig Config { get; } = new ConfigImplementation();

    /// <summary>
    /// Resets the entire database by clearing all data and resetting configuration settings.
    /// </summary>
    public void ResetDB()
    {
        Call.DeleteAll();
        //Volunteer.DeleteAll();
        Assignment.DeleteAll();
        Config.Reset();
    }
}
