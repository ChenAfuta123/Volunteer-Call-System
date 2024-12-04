
namespace BlImplementation;
using BlApi;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class CallImplementation : ICall
{

    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void CanceltreatmentUpdate(int id, int AssignmentId)
    {
        throw new NotImplementedException();
    }

    public void ChooseCallForTreatment(int volunteerId, int AssignmentId)
    {
        throw new NotImplementedException();
    }

    public void EndOftreatmentUpdate(int volunteerId, int AssignmentId)
    {
        throw new NotImplementedException();
    }

    public void OpenCallsByVolunteer(int id, global::CallType? calltype, Enum? Sorting)
    {
        throw new NotImplementedException();
    }

    public void RequestForCallAdd(Call )
    {
        throw new NotImplementedException();
    }

    public void RequestForCallDelete(int callId)
    {
        throw new NotImplementedException();
    }

    public List<BO.CallAssignInList> RequestForCallDetails(int callId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.CallInList> RequestForCallList(Enum? filter, object? obg, Enum? Sorting)
    {
        throw new NotImplementedException();
    }

    //public Array<int> RequestForCallQuantities()
    //{
    //    var Quantities = _dal.Call.ReadAll().;
    //    int[] statusCounts = new int[maxStatus + 1];
    //    var grouped = Quantities.GroupBy(C => C.callType);
    //     foreach (var group in grouped)
    //    {
            
    //        foreach (var call in group)
    //        {
    //            Console.WriteLine($"  {employee.Name}");
    //        }
           
    //    }  
    //}
    public void UpdateCallDetails(BO.Volunteer volunteer)
    {
        throw new NotImplementedException();
    }
}


    

  
