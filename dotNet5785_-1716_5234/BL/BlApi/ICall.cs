using BO;

namespace BlApi;

/// <summary>Interface for managing call operations.</summary>
public interface ICall
{

    /// <summary>Add a new call.</summary>
    public void Add(BO.Call call);

    /// <summary>Delete a call by its ID.</summary>
    public void Delete(int callId);

    /// <summary>Read a call by its ID.</summary>
    public BO.Call Read(int callId);

    /// <summary>
    /// Get all calls, with optional filtering and sorting.
    /// </summary>
    public IEnumerable<BO.CallInList> ReadAll(CallInListField? filter, object? obg, CallInListField? Sorting);

    /// <summary>Update an existing call.</summary>
    public void Update(BO.Call call);

    /// <summary>Get quantities of calls by category.</summary>
    public int[] CallQuantities();

    /// <summary>Assign a call for treatment by a volunteer.</summary>
    public void ChooseCallForTreatment(int volunteerId, int AssignmentId);

    /// <summary>Cancel a treatment for a specific call.</summary>
    public void CanceltreatmentUpdate(int id, int AssignmentId);

    /// <summary>Mark a treatment as completed for a specific call.</summary>
    public void EndOftreatmentUpdate(int requesterId, int AssignmentId);

    /// <summary>Get closed calls handled by a specific volunteer.</summary>
    public IEnumerable<BO.ClosedCallInList> ClosedCallsByVolunteer(int volunteerId, BO.CallType? callType, BO.ClosedCallInListField? sorting);

    /// <summary>Get open calls assigned to a specific volunteer.</summary>
    public IEnumerable<BO.OpenCallInList> OpenCallsByVolunteer(int id, BO.CallType? calltype, BO.OpenCallInListField? Sorting);
}
