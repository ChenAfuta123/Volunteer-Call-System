using BlApi;
using BO;
using BO;
using DalApi;
using DO;
using System.Net;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace Helpers;

internal static class CallManager
{
    private static IDal s_dal = Factory.Get;
    internal static BO.CallStatus Status(int callId)
    {
        try
        {
            DO.Call? call = s_dal.Call.Read(a => a.Id == callId);
            if (call == null) throw new BO.BlObjectNotFoundException("Call is null.\"");



            var now = ClockManager.Now;

            // בדיקה אם יש משימה פעילה
            if (s_dal.Assignment.Read(a => a.CallId == call.Id && a.EndTime == null) != null)
            {
                return BO.CallStatus.InProgress;
            }

            // בדיקה אם הקריאה פגה
            if (call.maxEndingTime.HasValue && now > call.maxEndingTime.Value)
            {
                return BO.CallStatus.Expired;
            }

            // בדיקה אם יש משימה שהסתיימה
            if (s_dal.Assignment.Read(a => a.CallId == call.Id && a.EndTime != null) != null)
            {
                return BO.CallStatus.Closed;
            }

            // בדיקה אם הקריאה פתוחה אך נמצאת בסיכון
            if (call.maxEndingTime.HasValue && now > call.OpeningTime.AddHours(1))
            {
                return BO.CallStatus.OpenAtRisk;
            }

            // אם אף אחד מהמקרים לא נכון, הקריאה פתוחה
            return BO.CallStatus.Open;
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Error while reading a call:", ex);
        }


    }
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
            Description = call.Description,
            MaxEndingTime = call.maxEndingTime,
            CallStatus = Status(call.Id),
            CallAssignList = callAssignInList
        };
    }
    //private static IEnumerable<DO.Call> FilterCalls(IEnumerable<BO.Call> calls, CallInListField? filter, object? obg)
    //{
    //    // אם filter לא null, מבצע סינון לפי filter ו-obg
    //    if (!filter.HasValue)
    //        return calls; // אם אין סינון, מחזיר את כל הקריאות

    //    return calls.Where(call =>
    //    {
    //        switch ((CallInListField)filter.Value)
    //        {
    //            case CallInListField.Id:
    //                return call.Id.Equals(obg);
    //            case CallInListField.CallId:
    //                return call.CallId.Equals(obg);
    //            case CallInListField.CallType:
    //                return call.callType.Equals(obg);
    //            case CallInListField.OpeningTime:
    //                return call.OpeningTime.Equals(obg);
    //            case CallInListField.RemainingCallTime:
    //                return call.RemainingCallTime.Equals(obg);
    //            case CallInListField.LastVolunteerName:
    //                return call.LastVolunteerName.Contains((string)obg);
    //            case CallInListField.TotalHandlingTime:
    //                return call.TotalHandlingTime.Equals(obg);
    //            case CallInListField.CallStatus:
    //                return call.callStatus.Equals(obg);
    //            case CallInListField.TotalAllocations:
    //                return call.TotalAllocations.Equals(obg);
    //            default:
    //                return true;
    //        }
    //    });
    //}

    public static IEnumerable<BO.CallInList> SortCalls(IEnumerable<BO.CallInList>? calls, CallInListField? sorting)
    {
        if(calls== null) {  throw new BO.BlObjectNotFoundException("calls not faund"); }
        if (!sorting.HasValue)
            return calls.OrderBy(call => call.CallId); // מיון לפי default (ID של הקריאה)

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
            _ => calls.OrderBy(call => call.CallId)
        };
    }
    public static bool ValidateCall(BO.Call call)
    {
        try
        {
            if (!Tools.IsValidID(call.Id))
                throw new Exception("Invalid Id.");

            if (!Enum.IsDefined(typeof(BO.CallType), call.callType))
                throw new Exception("Invalid call type.");

            if (!Enum.IsDefined(typeof(CallStatus), call.callStatus))
                throw new Exception("Invalid call status.");

            
            if (string.IsNullOrEmpty(call.Description))
                throw new Exception("Invalid call description.");


            if (!Tools.DistanceCalculator.IsValidAddress(call.Address, call.Longitude, call.Latitude))
                throw new Exception("Invalid Address.");

            if (call.OpeningTime == default)
                throw new Exception("Opening time is required.");

            if (call.MaxEndingTime.HasValue && call.MaxEndingTime <= call.OpeningTime)
                throw new Exception("Max ending time must be later than opening time.");

            return true;
        }
        catch (Exception ex)
        {
            throw new ValidationException("Error validating call details: " + ex.Message);
        }
    }
    internal static BO.CallInList DOtoBOList(BO.Call call)
    {
        if (call == null)
        {
            throw new ArgumentNullException(nameof(call), "The provided DO.Call object is null.");
        }

        try
        {
            return new BO.CallInList
            {
                Id = call.Id,
                CallId = call.Id,
                callType = call.CallType,
                OpeningTime = call.OpeningTime,
                RemainingCallTime = call.maxEndingTime.HasValue ? call.maxEndingTime.Value - DateTime.Now : (TimeSpan?)null,
                LastVolunteerName = GetLastVolunteerName(call),
                TotalHandlingTime = GetTotalHandlingTime(call),
                callStatus =Status(call.Id),
                TotalAllocations = call.CallAssignList?.Count ?? 0
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to convert DO.Call to BO.CallInList.", ex);
        }
    }

    // מתודת עזר לחישוב שם המתנדב האחרון
    private static string GetLastVolunteerName(BO.Call call)
    {
        return call.CallAssignList?
            .OrderByDescending(a => a.EntryTime)
            .FirstOrDefault()?.Volunteer.Name ?? "Unknown Volunteer";
    }

    // מתודת עזר לחישוב סך זמן הטיפול של כל ההקצאות
    private static TimeSpan? GetTotalHandlingTime(BO.Call call)
    {
        return call.CallAssignList?.Sum(a => a.TotalHandlingTime);
    }


    // מתודה לסינון הקריאות
    private static IEnumerable<DO.Call> FilterCalls(IEnumerable<DO.Call> calls, BO.CallInListField? filterField, object? filterValue)
    {
        if (filterField == null || filterValue == null)
        {
            return calls;
        }

        switch (filterField)
        {
            case BO.CallInListField.Id:
                return calls.Where(c => c.Id.Equals(filterValue));
            case BO.CallInListField.CallId:
                return calls.Where(c => c.CallId.Equals(filterValue));
            case BO.CallInListField.CallType:
                return calls.Where(c => c.CallType.Equals(filterValue));
            case BO.CallInListField.OpeningTime:
                return calls.Where(c => c.OpeningTime.Equals(filterValue));
            case BO.CallInListField.RemainingCallTime:
                return calls.Where(c => c.RemainingCallTime.Equals(filterValue));
            case BO.CallInListField.LastVolunteerName:
                return calls.Where(c => c.LastVolunteerName.Equals(filterValue));
            case BO.CallInListField.TotalHandlingTime:
                return calls.Where(c => c.TotalHandlingTime.Equals(filterValue));
            case BO.CallInListField.CallStatus:
                return calls.Where(c => c.CallStatus.Equals(filterValue));
            case BO.CallInListField.TotalAllocations:
                return calls.Where(c => c.TotalAllocations.Equals(filterValue));
            default:
                throw new ArgumentException("Unsupported filter field.", nameof(filterField));
        }

    }
    public static IEnumerable<BO.ClosedCallInList> FilterClosedCallsByVolunteer(IEnumerable<BO.Call> calls, int volunteerId)
    {
        try
        {
            return calls
                .Where(call => call.CallAssignList != null &&
                               call.CallAssignList.Any(assign => assign.VolunteerId == volunteerId && assign.EndTime.HasValue))
                .SelectMany(call => call.CallAssignList
                    .Where(assign => assign.VolunteerId == volunteerId && assign.EndTime.HasValue)
                    .Select(assign => new BO.ClosedCallInList
                    {
                        Id = call.Id,
                        callType = call.callType,
                        Address = call.Address,
                        OpeningTime = call.OpeningTime,
                        EntryTime = assign.EntryTime,
                        EndTime = assign.EndTime,
                        EndTimeType = assign.EndTimeType
                    }));
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to filter closed calls for volunteer ID {volunteerId}.", ex);
        }
    }
    public static IEnumerable<BO.ClosedCallInList> FilterCallsByType(IEnumerable<BO.ClosedCallInList> calls, BO.CallType callTypeFilter)
    {
        try
        {
            return calls.Where(call => call.callType == callTypeFilter);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to filter calls by type {callTypeFilter}.", ex);
        }
    }
    //public static IEnumerable<BO.ClosedCallInList> SortCalls(IEnumerable<BO.ClosedCallInList> calls, ClosedCallInListField? sortingField)
    //{
    //    return sortingField switch
    //    {
    //        ClosedCallInListField.Id => calls.OrderBy(c => c.Id),
    //        ClosedCallInListField.CallType => calls.OrderBy(c => c.callType),
    //        ClosedCallInListField.Address => calls.OrderBy(c => c.Address),
    //        ClosedCallInListField.OpeningTime => calls.OrderBy(c => c.OpeningTime),
    //        ClosedCallInListField.EntryTime => calls.OrderBy(c => c.EntryTime),
    //        ClosedCallInListField.EndTime => calls.OrderBy(c => c.EndTime),
    //        ClosedCallInListField.EndTimeType => calls.OrderBy(c => c.EndTimeType),
    //        _ => calls.OrderBy(c => c.Id) // ברירת מחדל: מיון לפי Id
    //    };
    //}
    public static void HandleOpenCall(int callId, int volunteerId, CallStatus callStatus)
    {
        // שלב 1: הבאת הקריאה על פי מזהה הקריאה
        var call = s_dal.Call.Read(callId);
        if (call == null)
        {
            throw new ApplicationException($"Call with ID {callId} does not exist.");
        }

        // שלב 2: בדיקה אם תוקף הקריאה פג
        if (call.maxEndingTime.HasValue && call.maxEndingTime.Value < DateTime.Now)
        {
            throw new ApplicationException("The call's validity period has expired.");
        }

        // שלב 3: בדיקה אם יש כבר הקצאה פתוחה על הקריאה
        var assignments = s_dal.Assignment.ReadAll()
            .Where(assign => assign.CallId == callId && assign.EndTime == null);
        if (assignments.Any())
        {
            throw new ApplicationException("The call is already assigned to another volunteer.");
        }

        // שלב 4: יצירת ישות הקצאה חדשה
        var newAssignment = new DO.Assignment
        {
            Id = 0, // יצירת מזהה ייחודי חדש
            CallId = callId,
            VolunteerId = volunteerId,
            EntryTime = DateTime.Now, // זמן כניסה לטיפול
            EndTimeType = null,
            EndTime = null
        };

        // שלב 5: הוספת ההקצאה למאגר הנתונים
        s_dal.Assignment.Create(newAssignment);

        // שלב 6: עדכון הסטטוס של הקריאה
       // call.callStatus = callStatus == CallStatus.Open ? CallStatus.InProgress : CallStatus.InProgressAtRisk;
        //s_dal.Call.Update(call);
    }
    internal static void HandleExpiredCalls()
    {
        var calls = s_dal.Call.ReadAll();

        foreach (var call in calls)
        {
            // בדוק אם השיחה עברה את הזמן המקסימלי
            if (call.maxEndingTime is null || call.maxEndingTime.Value >= ClockManager.Now)
                continue;

            var businessCall = DOtoBO(call);

            // דלג אם השיחה כבר נסגרה
            if (businessCall.CallStatus == CallStatus.Closed)
                continue;

            var relatedAssignments = s_dal.Assignment
                .ReadAll()
                .Where(assignment => assignment.CallId == call.Id)
                .ToList();

            if (!relatedAssignments.Any())
            {
                // צור משימה חדשה עם סטטוס "ביטול עקב פקיעת זמן"
                var newAssignment = new DO.Assignment
                {
                    CallId = call.Id,
                    VolunteerId = null,
                    EntryTime = ClockManager.Now,
                    EndTime = ClockManager.Now,
                    EndTimeType = DO.EndTimeType.Expired
                };

                 s_dal.Assignment.Create(newAssignment);
            }
            else
            {
                // עדכן את המשימה הפתוחה האחרונה
                var openAssignment = relatedAssignments.LastOrDefault(a => !a.EndTime.HasValue);
                if (openAssignment != null)
                {
                    var updatedAssignment = openAssignment with
                    {
                        EndTime = ClockManager.Now,
                        EndTimeType = DO.EndTimeType.Expired
                    };

                    s_dal.Assignment.Update(updatedAssignment);
                }
            }

            // עדכן את הסטטוס של השיחה ל-"סגור"
           // businessCall.CallStatus = CallStatus.Closed;
            s_dal.Call.Update(call);
        }
    }



}


