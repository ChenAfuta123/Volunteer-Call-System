
namespace BO;

internal class CallInProgress
{
    public int Id { get; init; }
    public int CallId { get; init; }
    public CallType callType { get; init; }
    public required string Address{ get; init; }
    public DateTime OpeningTime { get; init; }
    public string? Description { get; init; }
    public DateTime? maxEndingTime { get; init; }
    public DateTime EntryTime { get; init; }
    public double CallDistanceFromVolunteer { get; init; }
    public CallStatus callStatus { get; init; }

}
