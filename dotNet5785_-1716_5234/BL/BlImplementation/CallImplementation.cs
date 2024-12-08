
namespace BlImplementation;
using BlApi;
using BO;
using DO;
using Helpers;
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

    public void OpenCallsByVolunteer(int id, BO.CallType? calltype, Enum? Sorting)
    {
        throw new NotImplementedException();
    }

public void Add(BO.Call call)
{
       
}


public void Delete(int callId)
    {
        throw new NotImplementedException();
    }

    public BO.Call Read(int id)
    {
        try
        {
            var call = _dal.Call.Read(id);
            if (call == null)
                throw new ArgumentException("Call not found.");

            // מחזיר את רשימת ה-CallAssignList מתוך BO.Call
            return CallManager.DOtoBO(call);
        }
        catch (Exception)
        {
            throw new ArgumentException("Call not found.");
        }
    }


    public IEnumerable<BO.CallInList> ReadAll(CallInListField? filter, object? obg, CallInListField? sorting)
    {
        // 1. קבלת כל הקריאות מה-Data Access Layer
        var calls = _dal.Call.ReadAll();
        // 3. המרת כל קריאה ל-BO.Call
        var Bo_calls = calls.Select(call => CallManager.DOtoBO(call)).ToList();
        // 4. החזרת רשימת CallInList מתוך כל BO.Call
        var callInLists = Bo_calls.SelectMany(call => call.CallAssignList).ToList();
        // 2. סינון הקריאות לפי filter ו-obg
        var filteredCalls = CallManager.FilterCalls(callInLists, filter, obg);
        // 5. מיון הקריאות אם הועבר filter למיון
        var sortedCalls = CallManager.SortCalls(filteredCalls, sorting);

        return sortedCalls;
    }



    public int[] CallQuantities()//צריך  לממש את סטטוס
    {
        var calls = _dal.Call.ReadAll();
        // קיבוץ וספירה ישירות למערך
        var statusCounts = calls
            .GroupBy(call => (int)call.Status(call.Id))  // המרה לערך המספרי של ה-enum
            .Aggregate(
                new int[Enum.GetValues(typeof(CallStatus)).Length], // יצירת מערך בגודל 6
                (counts, group) =>
                {
                    counts[group.Key] = group.Count(); // עדכון המערך לפי הסטטוס
                    return counts;
                });
            
        return statusCounts;
           
        throw new NotImplementedException();
    }
    public void Update(BO.Volunteer volunteer)
    {
        throw new NotImplementedException();
    }
}


    

  
