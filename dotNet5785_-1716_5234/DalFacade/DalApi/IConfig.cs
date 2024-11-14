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

    /// <summary>
    /// Sets a new value for a configuration setting based on the variable name.
    /// </summary>
    /// <param name="variableName">The name of the configuration variable to update.</param>
    /// <param name="newValue">The new value to set for the specified configuration variable.</param>
    void SetConfigValue(string variableName, string newValue);

    /// <summary>
    /// Retrieves the current value of a configuration setting based on the variable name.
    /// </summary>
    /// <param name="variableName">The name of the configuration variable to retrieve.</param>
    /// <returns>The current value of the specified configuration variable.</returns>
    string GetConfigValue(string variableName);
}

