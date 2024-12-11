using Helpers;
namespace BO;
public class OpenCallInList
{

    public int Id { get; init; }
    public CallType callType { get; init; }
    public string? description { get; init; }
    public required string Address { get; init; }
    public DateTime OpeningTime { get; init; }
    public  DateTime? maxEndingTime { get; init; }
    public double CallDistanceFromVolunteer { get; init; }

    public override string ToString() => this.ToStringProperty();
}


