
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlApi;

public interface ICall
{
    public int[] CallQuantities();
    public IEnumerable<BO.CallInList> ReadAll(Enum? filter,object? obg, Enum? Sorting);
    public List<BO.CallAssignInList> Read(int callId);
    public void Update(BO.Volunteer volunteer);
    public void Delete(int callId);
    public void Add(BO.Call call);
    public void OpenCallsByVolunteer(int id,BO.CallType? calltype, Enum? Sorting);
    public void EndOftreatmentUpdate(int volunteerId, int AssignmentId);
    public void CanceltreatmentUpdate(int id, int AssignmentId);
    public void ChooseCallForTreatment(int volunteerId, int AssignmentId);
}