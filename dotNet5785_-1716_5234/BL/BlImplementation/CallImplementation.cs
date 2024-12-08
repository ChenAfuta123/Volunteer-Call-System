
namespace BlImplementation;
using BlApi;
using BO;
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
    try
    {
        // בדיקת תקינות ערכים מבחינת פורמט
        if (call.Id <= 0)
            throw new ArgumentException("Call ID must be a positive integer.");

        if (string.IsNullOrWhiteSpace(call.Address))
            throw new ArgumentException("Address cannot be empty.");

        if (call.Latitude is < -90 or > 90)
            throw new ArgumentException("Latitude must be between -90 and 90.");

        if (call.Longitude is < -180 or > 180)
            throw new ArgumentException("Longitude must be between -180 and 180.");

        if (call.OpeningTime > DateTime.Now)
            throw new ArgumentException("Opening time cannot be in the future.");

        if (call.maxEndingTime.HasValue && call.maxEndingTime < call.OpeningTime)
            throw new ArgumentException("Max ending time cannot be earlier than opening time.");

        // יצירת אובייקט חדש מטיפוס DO.Call
        var newCall = new DO.Call(
            call.Id,
            call.callType,
            call.Address ?? string.Empty,
            call.Latitude ?? 0,
            call.Longitude ?? 0,
            call.OpeningTime,
            call.description,
            call.maxEndingTime
        );

        // ניסיון הוספת הקריאה החדשה לשכבת הנתונים
        try
        {
            _dal.Call.Create(newCall); // שכבת הנתונים אחראית על ההוספה.
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("duplicate key")) // זיהוי בעיית מפתח כפול
            {
                throw new InvalidOperationException($"A call with ID {call.Id} already exists.", ex);
            }
            throw; // לזרוק חריגות אחרות כפי שהן
        }
    }
    catch (Exception ex)
    {
        // זריקת החריגה מחדש עם הודעה ברורה לכיוון שכבת התצוגה
        throw new InvalidOperationException("Failed to add the call. See inner exception for details.", ex);
    }
}


public void Delete(int callId)
    {
        throw new NotImplementedException();
    }

    public List<BO.CallAssignInList> Read(int id)
    {
        try
        {
            DO.Call call = _dal.Call.Read(id)!;
            if (call == null)
                throw new ArgumentException("Call not found.");
            return CallManager.DOtoBO(call);
        }
        catch (Exception)
        { throw new ArgumentException("Call not found."); }
    }

    public IEnumerable<BO.CallInList> ReadAll(Enum? filter, object? obg, Enum? Sorting)
    {
        throw new NotImplementedException();
    }

    public int[] CallQuantities()//צריך  לממש את סטטוס
    {
        var calls = _dal.Call.ReadAll();
        // קיבוץ וספירה ישירות למערך
        var statusCounts = calls
            .GroupBy(call => (int)call.callStatus)  // המרה לערך המספרי של ה-enum
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


    

  
