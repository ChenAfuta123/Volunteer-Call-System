

namespace DalApi;

public interface IConfig
{
    DateTime Clock { get; set; }
    TimeSpan RiskRange { get; set; }
    int nextCallId { get;  }
    int nextAssignmentId { get; }
     void Reset();
}
