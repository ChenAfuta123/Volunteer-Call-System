

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
}


