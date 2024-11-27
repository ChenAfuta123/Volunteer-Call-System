using Helpers;
namespace BO;
public class OpenCallInList
{

    public int Id { get; }
    public CallType callType { get; }
    public string? description { get; }
    public string Address { get; }
    public DateTime OpeningTime { get; }
    public  DateTime? maxEndingTime { get; }
    public double CallDistanceFromVoluntee { get; }

    public override string ToString() => this.ToStringProperty();
}


