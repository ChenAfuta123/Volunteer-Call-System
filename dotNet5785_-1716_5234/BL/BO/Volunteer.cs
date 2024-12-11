using DO;
using Helpers;
namespace BO;
public class Volunteer
{
    public int Id { get; init; }
    public  Role role { get; init; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
   public required string Email { get; set; }
   public string? Password { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? MaxDistance { get; set; }
    public bool Active { get; set; }
    public DistanceType distanceType { get; set; }
    public int TotalHandledCalls { get; init; }
    public int TotalCanceledCalls { get; init; }
    public int TotalExpiredCalls { get; init; }
    internal BO.CallInProgress? VolunteerHandledCall { get; set; }
    public override string ToString() => this.ToStringProperty();
}