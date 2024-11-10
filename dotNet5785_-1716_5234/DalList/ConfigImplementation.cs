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
    public int nextCallId
    {
        get => Config.NextCallId;
 
    }
   public int nextAssignmentId
    {
        get => Config.NextAssignmentId;
    }
    public void Reset()
    {
        Config.Reset();
  
    }
}


