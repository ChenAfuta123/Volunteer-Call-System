
namespace BO;

internal class CallInList
{
    public int? Id { get; init; }
    public int CallId { get; init; }
    public CallType callType { get; init; }
    public DateTime OpeningTime { get; init; }
    public TimeSpan? RemainingCallTime { get; init; }
    public string? LastVolunteerName { get; init; }
    public TimeSpan? TotalHandlingTime { get; init; }
    public CallStatus callStatus { get; init; }
    public int TotalAllocations { get; init; }
}
