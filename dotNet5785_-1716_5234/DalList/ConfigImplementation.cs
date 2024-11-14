using DalApi;

namespace Dal;

/// <summary>
/// Implementation of the IConfig interface, providing access to and management of configuration settings 
/// by using the internal Config class as a data source.
/// </summary>
public class ConfigImplementation : IConfig
{
    /// <summary>
    /// Gets or sets the current system clock, which represents the current date and time.
    /// </summary>
    public DateTime Clock
    {
        get => Config.Clock;
        set => Config.Clock = value;
    }

    /// <summary>
    /// Gets or sets the risk time range for calls, within which a call is considered at risk.
    /// </summary>
    public TimeSpan RiskRange
    {
        get => Config.RiskRange;
        set => Config.RiskRange = value;
    }

    /// <summary>
    /// Gets the next unique ID for Call entities.
    /// </summary>
    public int NextCallId
    {
        get => Config.NextCallId;
    }

    /// <summary>
    /// Gets the next unique ID for Assignment entities.
    /// </summary>
    public int NextAssignmentId
    {
        get => Config.NextAssignmentId;
    }

    /// <summary>
    /// Resets the configuration settings to their initial default values.
    /// </summary>
    public void Reset()
    {
        Config.Reset();
    }
    public void SetConfigValue(string variableName, string newValue)
    {
        Config.SetConfigValue(variableName, newValue);
    }

    // השגת ערך של משתנה תצורה
    public string GetConfigValue(string variableName)
    {
        return Config.GetConfigValue(variableName);
    }
}
    //private static Dictionary<string, string> configVariables = new Dictionary<string, string>();
   //static void SetConfigValue(string variableName, string newValue)
   // {
   //     Config.SetConfigValue(variableName, newValue);
   // }
   // static string GetConfigValue(string variableName)
   // {
   //     return Config.GetConfigValue(variableName); 
   // }



