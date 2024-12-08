using Helpers;
namespace BO;
public class CallAssignInList
{
    public int? VolunteerId { get; init; }
    public string? Name { get; init; }
    public DateTime EntryTime { get; init; }
    public DateTime? EndTime { get; init; }
    public EndTimeType? EndTimeType { get; init; }


    public override string ToString() => this.ToStringProperty();
}


