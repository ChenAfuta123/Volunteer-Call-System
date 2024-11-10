
namespace DalApi;

public interface IConfig
{
    DateTime Clock { get; set; }
    TimeSpan RiskRange { get; set; }
    int NextCallId { get;  }
    int NextAssignmentId { get; }
void Reset();
}
