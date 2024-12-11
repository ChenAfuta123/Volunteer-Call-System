namespace DO;

/// <summary>
/// Represents an assignment of a volunteer to a call, containing details of the assignment and its status.
/// </summary>

/// <param name="Id">Unique identifier for the assignment.</param>
/// <param name="CallId">ID of the call associated with this assignment.</param>
/// <param name="VolunteerId">ID of the volunteer assigned to the call.</param>
/// <param name="EntryTime">The time the volunteer was assigned to the call.</param>
/// <param name="EndTimeType">The type representing the end status of the assignment, if completed (optional).</param>
/// <param name="EndTime">The time the assignment ended, if completed (optional).</param>
public record Assignment
(
    int Id,
    int CallId,
    int? VolunteerId,
    DateTime EntryTime,
    EndTimeType? EndTimeType = null,
    DateTime? EndTime = null

)
{
    /// <summary>
    /// Default constructor for Assignment, initializing with default values.
    /// </summary>
    public Assignment() : this(0, 0, 0, DateTime.Now, null, null) { }
}
