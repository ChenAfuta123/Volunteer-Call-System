
namespace BO;

internal class VolunteerInList
{
    public int Id { get; }
    public string Name { get; }
    public bool Active { get; }
    public int TotalHandledCalls { get; }
    public int TotalCanceledCalls { get; }
    public int TotalExpiredCalls { get; }
    public int? HandledCallId { get; }
    public CallType callType { get; }
   
}
