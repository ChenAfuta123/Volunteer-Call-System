
using System;

namespace DO;


/// <summary>
/// Represents a call with various properties like ID, address, description, coordinates, timestamps, and more.
/// </summary>
/// <param name="Id">Personal unique ID of the call.</param>
/// <param name="Address">Address associated with the call.</param>
/// <param name="Description">Additional description for the call (optional).</param>
/// <param name="Latitude">Latitude of the call location (optional).</param>
/// <param name="Longitude">Longitude of the call location
public record Call
(
    int Id,
    CallType callType,
    string Address,
    double Latitude,
    double Longitude,
    DateTime OpeningTime,
    string? Description = null,
    DateTime? maxEndingTime = null


)
{
   
    /// <summary>
    /// Parameterless constructor for Call, initializing properties with default values.
    /// </summary>
    public Call() : this(0, CallType.EssentialSupplies, "",0, 0, DateTime.Now,null,null)
    {
        
    }

}