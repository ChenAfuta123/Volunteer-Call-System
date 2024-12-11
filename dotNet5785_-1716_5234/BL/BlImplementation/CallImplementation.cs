
namespace BlImplementation;
using BlApi;
using BO;
using DO;
using Helpers;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class CallImplementation : ICall
{

    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
  
    public void CanceltreatmentUpdate(int id, int AssignmentId)
    {
        throw new NotImplementedException();
    }

    public void ChooseCallForTreatment(int volunteerId, int callId)
    {
        try
        {
            // שלב 1: חישוב הסטטוס של הקריאה
            var callStatus = CallManager.Status(callId);

            // שלב 2: טיפול בסטטוסים שונים באמצעות switch
            switch (callStatus)
            {
                case CallStatus.Open:
                case CallStatus.OpenAtRisk:
                    CallManager.HandleOpenCall(callId, volunteerId, callStatus);
                    break;

                case CallStatus.InProgress:
                case CallStatus.InProgressAtRisk:
                    throw new ApplicationException("The call is already in progress and cannot be reassigned.");

                case CallStatus.Closed:
                    throw new ApplicationException("The call has already been closed and cannot be assigned.");

                case CallStatus.Expired:
                    throw new ApplicationException("The call's validity period has expired and cannot be assigned.");

                default:
                    throw new ApplicationException("Unknown call status. Cannot assign the call.");
            }
        }
        catch (Exception ex)
        {
            // טיפול בחריגות
            throw new ApplicationException("Failed to assign the call to the volunteer.", ex);
        }
    }

    public void EndOftreatmentUpdate(int volunteerId, int AssignmentId)
    {
        throw new NotImplementedException();
    }

    public void OpenCallsByVolunteer(int id, BO.CallType? calltype, Enum? Sorting)
    {
        throw new NotImplementedException();
    }

   // int Id,
   //CallType callType,
   // string Address,
   // double Latitude,
   // double Longitude,
   // DateTime OpeningTime,
   // string? Description = null,
   // DateTime? maxEndingTime = null

    public void Add(BO.Call boCall)
    {
        CallManager.ValidateCall(boCall);
        DO.Call doCall = new DO.Call
        {
            Id = boCall.Id,
            callType = (DO.CallType)boCall.callType,
            Address = boCall.Address ?? " ",
            Latitude = boCall.Latitude ?? 0.0,
            Longitude = boCall.Longitude ?? 0.0,
            OpeningTime = boCall.OpeningTime,
            Description = boCall.Description,
            maxEndingTime = boCall.MaxEndingTime

        };
        try
        {
            _dal.Call.Create(doCall);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Call with ID={boCall.Id} already exists", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error while adding a call: {ex.Message}");
        }
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
        { throw new ArgumentException("Call not found."); }
    }
    public IEnumerable<BO.CallInList> ReadAll(CallInListField? filter, object? obg, CallInListField? sorting)
    {
        try
        {
            // שליפת כל הקריאות מה- DAL
            var calls = _dal.Call.ReadAll();  // כאן ייתכן שצריך לשנות את המתודה הזו לפי הצורך.

            // אם לא נמצאו קריאות ב-DAL
            if (calls == null || !calls.Any())
            {
                throw new ApplicationException("No calls found in the database.");
            }
            var boCall = calls.Select(CallManager.DOtoBO);
            var boCalls = boCall.Select(CallManager.DOtoBOList);
            // סינון הקריאות לפי פרמטרים
            var filteredCalls = CallManager.FilterCalls(calls, filter, obg);
            // מיון הקריאות לפי פרמטרים
            var sortedCalls = CallManager.SortCalls(boCalls, sorting);
             
   

            return boCalls;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to read, filter, and sort the calls.", ex);
        }
    }

    public int[] CallQuantities()
    {
        var calls = _dal.Call.ReadAll();
        var statusCounts = calls
            .GroupBy(call => (int)CallManager.Status(call.Id))  
            .Aggregate(
                new int[Enum.GetValues(typeof(CallStatus)).Length], 
                (counts, group) =>
                {
                    counts[group.Key] = group.Count(); 
                    return counts;
                });
            
        return statusCounts;
    }

    public void Update(BO.Call boCall)
    {
        CallManager.ValidateCall(boCall);

        DO.Call doCall = new DO.Call
        {
            Id = boCall.Id,
            callType = (DO.CallType)boCall.callType,
            Address = boCall.Address ?? " ",
            Latitude = boCall.Latitude ?? 0.0,
            Longitude = boCall.Longitude ?? 0.0,
            OpeningTime = boCall.OpeningTime,
            Description = boCall.Description,
            maxEndingTime = boCall.MaxEndingTime,

        };
        try
        {
            _dal.Call.Update(doCall);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Error while updating a call:", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error while updating a call: {ex.Message}");
        }

    }


    public IEnumerable<BO.ClosedCallInList> GetClosedCallsByVolunteer( int volunteerId,  BO.CallType? callTypeFilter = null, ClosedCallInListField? sortingField = null)
    {
        try
        {
            // שליפת כל הקריאות מה-DAL
            var calls = _dal.Call.ReadAll();
            if (calls == null)
            {
                throw new ApplicationException("Failed to retrieve calls: the data source returned null.");
            }
            // המרה של קריאות לוגיות משכבת DO ל-BO
            var boCalls =calls.Select(call => CallManager.DOtoBO(call));
            
            // שלב 2: סינון הקריאות עבור מתנדב עם ת.ז ספציפי
            var closedCalls = CallManager.FilterClosedCallsByVolunteer(boCalls, volunteerId);

            // שלב 3: סינון לפי סוג הקריאה אם נדרש
            if (callTypeFilter.HasValue)
            {
                closedCalls = CallManager.FilterCallsByType(closedCalls, callTypeFilter.Value);
            }

            // שלב 4: מיון הקריאות לפי השדה המבוקש
            closedCalls = CallManager.SortCalls(closedCalls, sortingField);

            // החזרת הרשימה המסוננת והמסודרת
            return closedCalls;
        }
        catch (Exception ex)
        {
            // טיפול בחריגות
            throw new ApplicationException("An error occurred while fetching closed calls by volunteer.", ex);
        }
    }


}






