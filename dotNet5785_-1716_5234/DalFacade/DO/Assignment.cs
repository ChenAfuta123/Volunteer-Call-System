namespace DO;

/// <summary>
/// Represents an assignment of a volunteer to a call, containing details of the assignment and its status.
/// </summary>
public record Assignment
(
    /// <summary>
    /// Unique identifier for the assignment.
    /// </summary>
    int Id,

    /// <summary>
    /// ID of the call associated with this assignment.
    /// </summary>
    int CallId,

    /// <summary>
    /// ID of the volunteer assigned to the call.
    /// </summary>
    int VolunteerId,

    /// <summary>
    /// The time the volunteer was assigned to the call.
    /// </summary>
    DateTime EntryTime,

    /// <summary>
    /// The type representing the end status of the assignment, if completed.
    /// </summary>
    EndTimeType? EndTimeType = null,

    /// <summary>
    /// The time the assignment ended, if completed.
    /// </summary>
    DateTime? EndTime = null

)
{
    /// <summary>
    /// Default constructor for Assignment, initializing with default values.
    /// </summary>
    public Assignment() : this(0, 0, 0, DateTime.Now, null, null) { }
}
