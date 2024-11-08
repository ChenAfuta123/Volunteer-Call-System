
using System.Security.Cryptography.X509Certificates;

namespace DO;

public record Volunteer
{
    int Id;
    string Name=string.Empty;
    string PhoneNumber = string.Empty;
    string Email = string.Empty;
    string? Password;
    string? Address;
    double? Latitude;
    double? Longitude;
    enum Role  { volunteer, manager };
    Boolean Active;
    double? MaxDistance;
    enum DistanceType { AirDistance, walkingDistance, DrivingDistance }

}

