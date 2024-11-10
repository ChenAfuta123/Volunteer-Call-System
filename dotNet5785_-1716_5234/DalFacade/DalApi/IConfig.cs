

namespace DalApi;

public interface IConfig
{
    DateTime Clock { get; set; }
    TimeSpan RiskRange { get; set; }
    int nextCallId { get; init; }
    int nextAssignmentId { get; init; }
void Reset();
}
