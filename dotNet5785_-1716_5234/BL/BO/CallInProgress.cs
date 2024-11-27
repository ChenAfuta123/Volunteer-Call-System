
namespace BO;

internal class CallInProgress
{
    public int Id { get; init; }
    public int CallId { get; }
    public CallType callType { get; }
    public string Address{ get; }
    public DateTime OpeningTime { get; }
    public string? Description { get; }
    public DateTime? maxEndingTime { get; }
    public DateTime EntryTime { get; }
    public double CallDistanceFromVolunteer { get; }
    public CallStatus callStatus { get; }

}
