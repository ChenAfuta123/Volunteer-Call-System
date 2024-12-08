using BlApi;
using BO;
using DalApi;
using DO;
using System.Net;
using System.Xml.Linq;
namespace Helpers;

internal static class CallManager
{
    private static IDal s_dal = Factory.Get;
    public static BO.Call DOtoBO(DO.Call call)
    {
        // שלב 1: שליפת כל ה-Assignments הרלוונטיים לאותו ID
        var assignments = s_dal.Assignment.ReadAll()
            .Where(assignment => assignment.CallId == call.Id)
            .ToList();

        // שלב 2: יצירת רשימה של BO.CallAssignInList
        List<BO.CallAssignInList>? callAssignInList = assignments.Select(assignment =>
        {
            var volunteer = s_dal.Volunteer.Read(assignment.VolunteerId);
            if (volunteer == null)
            {
                throw new NotImplementedException($"The volunteer with ID {assignment.VolunteerId} was not found.");
            }
            return new BO.CallAssignInList
            {
                VolunteerId = assignment.VolunteerId,
                Name = volunteer.Name,
                EntryTime = assignment.EntryTime,
                EndTime = assignment.EndTime,
                EndTimeType = (BO.EndTimeType?)assignment.EndTimeType
            };
        }).ToList();

        return new BO.Call
        {
            Id = call.Id,
            callType = (BO.CallType)call.callType,
            Address = call.Address,
            Latitude = call.Latitude,
            Longitude = call.Longitude,
            OpeningTime = call.OpeningTime,
            description = call.Description,
            maxEndingTime = call.maxEndingTime,
            callStatus = Tools.Status(call.Id),
            CallAssignList = callAssignInList
        };
    }
    private static IEnumerable<DO.Call> FilterCalls(IEnumerable<BO.Call> calls, CallInListField? filter, object? obg)
    {
        // אם filter לא null, מבצע סינון לפי filter ו-obg
        if (!filter.HasValue)
            return calls; // אם אין סינון, מחזיר את כל הקריאות

        return calls.Where(call =>
        {
            switch ((CallInListField)filter.Value)
            {
                case CallInListField.Id:
                    return call.Id.Equals(obg);
                case CallInListField.CallId:
                    return call.CallId.Equals(obg);
                case CallInListField.CallType:
                    return call.callType.Equals(obg);
                case CallInListField.OpeningTime:
                    return call.OpeningTime.Equals(obg);
                case CallInListField.RemainingCallTime:
                    return call.RemainingCallTime.Equals(obg);
                case CallInListField.LastVolunteerName:
                    return call.LastVolunteerName.Contains((string)obg);
                case CallInListField.TotalHandlingTime:
                    return call.TotalHandlingTime.Equals(obg);
                case CallInListField.CallStatus:
                    return call.callStatus.Equals(obg);
                case CallInListField.TotalAllocations:
                    return call.TotalAllocations.Equals(obg);
                default:
                    return true;
            }
        });
    }

    private   static List<BO.CallInList> SortCalls(List<BO.CallInList> calls, CallInListField? sorting)
    {
        // אם ה-sort לא null, מבצע מיון לפי הערך של sorting
        if (!sorting.HasValue)
            return calls.OrderBy(call => call.CallId).ToList(); // מיון לפי default (ID של הקריאה)

        return sorting switch
        {
            CallInListField.Id => calls.OrderBy(call => call.Id).ToList(),
            CallInListField.CallId => calls.OrderBy(call => call.CallId).ToList(),
            CallInListField.CallType => calls.OrderBy(call => call.callType).ToList(),
            CallInListField.OpeningTime => calls.OrderBy(call => call.OpeningTime).ToList(),
            CallInListField.RemainingCallTime => calls.OrderBy(call => call.RemainingCallTime).ToList(),
            CallInListField.LastVolunteerName => calls.OrderBy(call => call.LastVolunteerName).ToList(),
            CallInListField.TotalHandlingTime => calls.OrderBy(call => call.TotalHandlingTime).ToList(),
            CallInListField.CallStatus => calls.OrderBy(call => call.callStatus).ToList(),
            CallInListField.TotalAllocations => calls.OrderBy(call => call.TotalAllocations).ToList(),
            _ => calls.OrderBy(call => call.CallId).ToList() // אם לא עבר שדה תקני, מיין לפי ID ברירת מחדל
        };
    }
}
