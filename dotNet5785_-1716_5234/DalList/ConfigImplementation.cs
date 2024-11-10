
using DalApi;

namespace Dal;
public class ConfigImplementation : IConfig
{
  
    public DateTime Clock
    {
        get => Config.Clock;
        set => Config.Clock = value;
    }
    public TimeSpan RiskRange
    {
        get => Config.RiskRange;
        set => Config.RiskRange = value;
    }
    public int NextCallId
    {
        get => Config.NextCallId;
 
    }
   public int NextAssignmentId
    {
        get => Config.NextAssignmentId;
    }
    public void Reset()
    {
        Config.Reset();
  
    }
    //private static Dictionary<string, string> configVariables = new Dictionary<string, string>();
    internal static void SetConfigValue(string variableName, string newValue)
    {
        Config.SetConfigValue(variableName, newValue);
    }
    internal static string GetConfigValue(string variableName)
    {
        return Config.GetConfigValue(variableName); 
    }
}


