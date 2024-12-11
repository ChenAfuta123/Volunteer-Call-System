using BO;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BlApi;

public interface ICall
{
    public void Add(BO.Call call);
    public void Delete(int callId);
    public BO.Call Read(int callId);
    public IEnumerable<BO.CallInList>ReadAll(CallInListField? filter,object? obg, CallInListField? Sorting);
    public void Update(BO.Call Call);
    public int[] CallQuantities();
    public void ChooseCallForTreatment(int volunteerId, int AssignmentId);
    public void CanceltreatmentUpdate(int id, int AssignmentId);
    public void EndOftreatmentUpdate(int requesterId, int AssignmentId);
    public IEnumerable<BO.ClosedCallInList> ClosedCallsByVolunteer(int volunteerId, BO.CallType? callType, BO.ClosedCallInListField? sorting);
    public IEnumerable<BO.OpenCallInList> OpenCallsByVolunteer(int id,BO.CallType? calltype, BO.OpenCallInListField? Sorting);
   
    

}