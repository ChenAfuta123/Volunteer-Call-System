
using DalApi;

namespace Dal;

internal class ConfigImplementation: IConfig
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
    public void SetConfigValue(string variableName, int newValue)
    {
        XMLTools.SetConfigValue(variableName, newValue);
    }

    public string GetConfigValue(int variableName)
    {
        return XMLTools.GetConfigValue(variableName);
    }





}
