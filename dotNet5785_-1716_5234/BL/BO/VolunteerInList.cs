
namespace BO;

internal class VolunteerInList
{
    public int Id { get; init; }
    public string Name { get; init; }
    public bool Active { get; init; }
    public int TotalHandledCalls { get; init; }
    public int TotalCanceledCalls { get; init; }
    public int TotalExpiredCalls { get; init; }
    public int? HandledCallId { get; init; }
   
}
