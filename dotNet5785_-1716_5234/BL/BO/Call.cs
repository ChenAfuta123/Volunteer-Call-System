using Helpers;
namespace BO;
public class Call
{

    public int Id { get; init; }
    public CallType callType { get; set; }
    public string? Description { get; set; }
    public required string Address { get; set; }
    public double Latitude { get; set; }
    public  double Longitude { get; set; }
    public DateTime OpeningTime { get; init; }
    public DateTime? MaxEndingTime { get; set; }
    public CallStatus CallStatus { get; set; }

    public List<BO.CallAssignInList>? CallAssignList { get; set;}


   public override string ToString() => this.ToStringProperty();
}


