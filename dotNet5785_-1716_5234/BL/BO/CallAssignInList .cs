using Helpers;
namespace BO;
public class CallAssignInList
{

   
 
    int? VolunteerId { get; }
    string? Name { get; }
    DateTime EntryTime { get; }
    DateTime? EndTime { get; }
    EndTimeType? EndTimeType { get; }


    public override string ToString() => this.ToStringProperty();
}


