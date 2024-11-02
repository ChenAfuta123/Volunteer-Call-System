//Module  Assignment.cs
namespace DO;
/// <summary>
/// Enum that represents the possible end states of an assignment.
/// </summary>
public enum EndTimeType { Treated, SelfCancel, ManagerCancel, Expired };
/// <summary>
/// Enum that represents the possible end states of an assignment.
/// </summary>
/// <param name="Id">Unique identifier for the assignment</param>
/// <param name="CallId">ID of the call associated with this assignment.</param></param>
/// <param name="VolunteerId">ID of the volunteer assigned to the call.</param>
public record Assignment
(
    int Id,
    int CallId,
    int VolunteerId
    

)
{
    /// <summary>
    /// The time the assignment was created.
    /// </summary>
    public readonly DateTime EntryTime = DateTime.Now;
    /// <summary>
    /// he time the assignment was ended, or null if the assignment is still active.
    /// </summary>
    public readonly DateTime? EndTime = null;
    /// <summary>
    /// Initializes a new instance of the Assignment record with default values.
    /// </summary>
    public Assignment() : this(0, 0, 0) { }
}
        
