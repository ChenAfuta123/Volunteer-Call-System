
// Module: Volunteer.cs
namespace DO
{
    /// <summary>
    /// Enum representing the role of the volunteer within the system.
    /// </summary>
    public enum Role
    {
        volunteer, 
        manager    
    }

    /// <summary>
    /// Enum representing the type of distance measurement used.
    /// </summary>
    public enum DistanceType
    {
        AirDistance,      
        WalkingDistance,  
        DrivingDistance   
    }

    /// <summary>
    /// Represents a volunteer with specific details such as contact information,
    /// location, and role within the system.
    /// </summary>
    /// <param name="Id">Unique identifier for the volunteer.</param>
    /// <param name="Name">Name of the volunteer.</param>
    /// <param name="PhoneNumber">Phone number of the volunteer.</param>
    /// <param name="Email">Email address of the volunteer.</param>
    /// <param name="Password">Optional password for accessing the system.</param>
    /// <param name="Address">Optional address of the volunteer.</param>
    /// <param name="Latitude">Latitude coordinate for the volunteer's location.</param>
    /// <param name="MaxDistance">Maximum distance the volunteer is willing to travel.</param>
    /// <param name="Active">Indicates if the volunteer is currently active in the system.</param>
public record Volunteer
    (
        int Id,
        string Name,
        string PhoneNumber,
        string Email,
        string? Password = null,
        string? Address = null,
        double? Latitude = null,
        double? MaxDistance = null,
        bool Active = false
    )
{
        /// <summary>
        /// Initializes a new instance of the Volunteer record with default values.
        /// </summary>
        public Volunteer() : this(0, " ", " ", " ", null, null, null, null) { } // Empty constructor for stage 3
    }
}

