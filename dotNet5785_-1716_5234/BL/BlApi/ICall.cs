
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlApi;

public interface ICall
{
    public Array<int> RequestForCallQuantities();
    public IEnumerable<BO.CallInList> RequestForCallList(Enum? filter,object? obg, Enum? Sorting);
    public List<BO.CallAssignInList> RequestForCallDetails(int callId);
    public void UpdateCallDetails(BO.Volunteer volunteer);
    public void RequestForCallDelete(int callId);
    public void RequestForCallAdd(BO.Call);
    public void OpenCallsByVolunteer(int id,CallType? calltype, Enum? Sorting);
    public void EndOftreatmentUpdate(int volunteerId, int AssignmentId);
    public void CanceltreatmentUpdate(int id, int AssignmentId);
    public void ChooseCallForTreatment(int volunteerId, int AssignmentId);
}