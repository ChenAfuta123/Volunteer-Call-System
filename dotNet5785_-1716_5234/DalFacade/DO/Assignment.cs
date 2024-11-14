//Module  Assignment.cs
namespace DO; 
/// <summary>
/// Enum that represents the possible end states of an assignment.
/// </summary>
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
    int VolunteerId,
    DateTime EntryTime,
    DateTime? EndTime = null
    
)
{
   
    public Assignment() : this(0, 0, 0, DateTime.Now,null) { }
}
        
