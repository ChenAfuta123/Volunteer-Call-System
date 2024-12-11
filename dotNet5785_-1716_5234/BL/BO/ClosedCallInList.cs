using Helpers;
namespace BO;
public class ClosedCallInList
{
   
    public int Id { get; init; }
    public CallType callType { get; init; }
    public required string Address { get; init; }
    public DateTime OpeningTime { get; init; }
    public DateTime EntryTime { get; init; }

    public DateTime? EndTime { get; init; }
    public EndTimeType? EndTimeType { get; init; }

    public override string ToString() => this.ToStringProperty();
}


