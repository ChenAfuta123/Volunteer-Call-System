
namespace BO;

internal class CallInList
{
    public int? Id { get;}
    public int CallId { get; }
    public CallType callType { get; }
    public DateTime OpeningTime { get; }
    public TimeSpan? RemainingCallTime { get; }
    public string? LastVolunteerName { get; }
    public TimeSpan? TotalHandlingTime { get; }
    public CallStatus callStatus { get; }
    public int TotalAllocations { get; }
}
