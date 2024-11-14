namespace DalApi;

/// <summary>
/// Interface for configuration settings, providing properties and methods 
/// to manage system-wide settings such as IDs, time settings, and reset functionality.
/// </summary>
public interface IConfig
{
    /// <summary>
    /// Gets or sets the current date and time in the system.
    /// </summary>
    DateTime Clock { get; set; }

    /// <summary>
    /// Gets or sets the time range within which a call is considered at risk.
    /// </summary>
    TimeSpan RiskRange { get; set; }

    /// <summary>
    /// Retrieves the next unique ID for Call entities.
    /// </summary>
    int NextCallId { get; }

    /// <summary>
    /// Retrieves the next unique ID for Assignment entities.
    /// </summary>
    int NextAssignmentId { get; }

    /// <summary>
    /// Resets the configuration settings to their initial default states.
    /// </summary>
    void Reset();
    //private static Dictionary<string, string> configVariables = new Dictionary<string, string>();
    // void SetConfigValue(string variableName, string newValue);

    //string GetConfigValue(string variableName);
    void SetConfigValue(string variableName, string newValue);

    // פונקציה להשיג ערך של משתנה תצורה
    string GetConfigValue(string variableName);
}

