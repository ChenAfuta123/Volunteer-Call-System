
namespace BlImplementation;
using BlApi;
using BO;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class CallImplementation : ICall
{

    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public int[] CallQuantities()
    {
        //var Quantities = _dal.Call.ReadAll().;
        //int[] statusCounts = new int[maxStatus + 1];
        //var grouped = Quantities.GroupBy(C => C.callType);
        //foreach (var group in grouped)
        //{

        //    foreach (var call in group)
        //    {
        //        Console.WriteLine($"  {employee.Name}");
        //    }

        //}


        //var calls = _dal.Call.ReadAll();

        //// קיבוץ וספירה ישירות למערך
        //var statusCounts = calls
        //    .GroupBy(call => (int)call.CallStatus)  // המרה לערך המספרי של ה-enum
        //    .Aggregate(
        //        new int[Enum.GetValues(typeof(CallStatus)).Length], // יצירת מערך בגודל 6
        //        (counts, group) =>
        //        {
        //            counts[group.Key] = group.Count(); // עדכון המערך לפי הסטטוס
        //            return counts;
        //        });

        //return statusCounts;


    }
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

    public void OpenCallsByVolunteer(int id, BO.CallType? calltype, Enum? Sorting)
    {
        throw new NotImplementedException();
    }

    public void Add(BO.Call call )
    {
        throw new NotImplementedException();
    }

    public void Delete(int callId)
    {
        throw new NotImplementedException();
    }

    public List<BO.CallAssignInList> Read(int callId)
    {
       _dal.Call.
    }

    public IEnumerable<BO.CallInList> ReadAll(Enum? filter, object? obg, Enum? Sorting)
    {
        throw new NotImplementedException();
    }

  
    public void Update(BO.Volunteer volunteer)
    {
        throw new NotImplementedException();
    }
}


    

  
