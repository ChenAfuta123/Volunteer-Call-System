using Helpers;
namespace BO;
public class ClosedCallInList
{
   
    public int Id { get;  }
    public CallType callType { get; }

    public  string Address { get; }
    public DateTime OpeningTime { get; }
    public DateTime EntryTime { get; }

    public DateTime? EndTime { get; }
    public EndTimeType? EndTimeType { get; }

    public override string ToString() => this.ToStringProperty();
}


