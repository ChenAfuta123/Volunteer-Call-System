
namespace BlImplementation;
using BlApi;
using BO;
using DO;
using Helpers;
using Microsoft.VisualBasic;
using System;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class CallImplementation : ICall
{

    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void Add(BO.Call boCall)
    {
        CallManager.ValidateCall(boCall);
        var coordinates = Tools.DistanceCalculator.GetAddressCoordinates(boCall.Address);
        double longtitude = coordinates.Latitude ?? 0.0;
        double latitude = coordinates.Latitude ?? 0.0;
        boCall.Latitude = latitude;
        boCall.Longitude = longtitude;

        DO.Call doCall = new DO.Call
        {
            Id = boCall.Id,
            callType = (DO.CallType)boCall.callType,
            Address = boCall.Address ?? " ",
            Latitude = boCall.Latitude,
            Longitude = boCall.Longitude,
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
            throw new BO.BlAlreadyExistsException($"Error while adding a call:", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error while adding a call: {ex.Message}");
        }
    }

    public void Delete(int callId)
    {
        DO.Call? call = _dal.Call.Read(callId);
        CallStatus callStatus = CallManager.Status(callId);
        
        try
        {
            if (callStatus == BO.CallStatus.Open && !_dal.Assignment.ReadAll(a => a.CallId == callId).Any())
            {
                _dal.Call.Delete(callId);
            }
            else
            {
                throw new BlCannotBeDeletedException("This Call cannot be deleted");
            }
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Error while deleting a call:", ex);

        }
    }

    public BO.Call Read(int callId)
    {
        
        
            DO.Call? call = _dal.Call.Read(callId);
            if (call == null) throw new BO.BlObjectNotFoundException("Call not found");
            
           
            return CallManager.DOtoBO(call);
      

    }

    public IEnumerable<BO.CallInList> ReadAll(CallInListField? filter, object? obg, CallInListField? sorting)
    {
        try
        {

          
            var calls = _dal.Call.ReadAll();  
            IEnumerable<BO.CallInList> CallsInList = calls.Select(CallManager.DOToBOCallInList);
       
            if (calls == null || !calls.Any())
            {
                throw new BO.BlNullPropertyException("No calls found in the database.");
            }
            if (filter != null)
            {
                CallsInList = CallManager.FilterCalls(CallsInList, filter, obg);
            }
            if (sorting == null)
            {
                 CallsInList = CallsInList.OrderBy(c => c.Id);
            }
            else if (sorting != null) {
                CallsInList = sorting switch
                {
                    CallInListField.CallId => CallsInList.OrderBy(c => c.CallId),
                    CallInListField.Id => CallsInList.OrderBy(c => c.Id),
                    CallInListField.CallType => CallsInList.OrderBy(c => c.callType),
                    CallInListField.OpeningTime => CallsInList.OrderBy(c => c.OpeningTime),
                    CallInListField.RemainingCallTime => CallsInList.OrderBy(c => c.RemainingCallTime),
                    CallInListField.LastVolunteerName => CallsInList.OrderBy(c => c.LastVolunteerName),
                    CallInListField.TotalHandlingTime => CallsInList.OrderBy(c => c.TotalHandlingTime),
                    CallInListField.CallStatus => CallsInList.OrderBy(c => c.callStatus),
                    CallInListField.TotalAllocations => CallsInList.OrderBy(c => c.TotalAllocations),
                    _ => CallsInList.OrderBy(c => c.Id) // מיון ברירת מחדל לפי Id
                };
            }
            return CallsInList;

          
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to read, filter, and sort the calls.", ex);
        }
    }
   
    public void Update(BO.Call boCall)
    {
       CallManager.ValidateCall(boCall);
        var coordinates = Tools.DistanceCalculator.GetAddressCoordinates(boCall.Address);
       double longtitude = coordinates.Latitude ?? 0.0;
       double latitude = coordinates.Latitude ?? 0.0;
        boCall.Latitude = latitude;
        boCall.Longitude = longtitude;

        DO.Call doCall = new DO.Call
        {
            Id = boCall.Id,
            callType = (DO.CallType)boCall.callType,
            Address = boCall.Address,
            Latitude = boCall.Latitude,
            Longitude = boCall.Longitude,
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

    public void ChooseCallForTreatment(int volunteerId, int callId)
    {
        try
        {

            var callStatus = CallManager.Status(callId);


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

            throw new ApplicationException("Failed to assign the call to the volunteer.", ex);
        }
    }

    public void CanceltreatmentUpdate(int requesterId, int assignmentId)
    {
        try
        {

            DO.Assignment? assignment = _dal.Assignment.Read(assignmentId);
            if (assignment == null) throw new BO.BlObjectNotFoundException("Assignment not found");
            DO.Volunteer? volunteer = _dal.Volunteer.Read(requesterId);
            if (volunteer == null) throw new BO.BlObjectNotFoundException("Volunteer not found");


            if (assignment.EndTimeType != null)
            {
                throw new BO.BlValidationException("The assignment is already closed.");
            }
            if (assignment.VolunteerId != requesterId || volunteer.role != DO.Role.manager)
            {
                throw new BO.BlUnauthorizedException("Only the assigned volunteer or manager can complete this treatment.");
            }

            DO.EndTimeType endTimeType = (assignment.VolunteerId == requesterId)
                ? DO.EndTimeType.SelfCancel
                : DO.EndTimeType.ManagerCancel;


            assignment = assignment with
            {

                EndTime = ClockManager.Now,
                EndTimeType = endTimeType
            };


            _dal.Assignment.Update(assignment);
        }
        catch (DO.DalDoesNotExistsException ex)
        {

            throw new BO.BlDoesNotExistsException($"Error while reading assignment or related data: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error while cancelling treatment: {ex.Message}", ex);
        }
    }

    public void EndOftreatmentUpdate(int requesterId, int assignmentId)
    {
        try
        {

            DO.Assignment? assignment = _dal.Assignment.Read(assignmentId);
            if (assignment == null) throw new BO.BlObjectNotFoundException("Assignment not found");
            
            if (assignment!.VolunteerId != requesterId)
            {
                throw new BO.BlUnauthorizedException("Only the assigned volunteer can complete this treatment.");
            }

            if (assignment.EndTime != null)
            {
                throw new BO.BlValidationException("This assignment is already completed or canceled.");
            }


            assignment = assignment with
            {
                EndTime = ClockManager.Now,
                EndTimeType = DO.EndTimeType.Treated
            };


            _dal.Assignment.Update(assignment);
        }
        catch (DO.DalDoesNotExistsException ex)
        {

            throw new BO.BlDoesNotExistsException($"Error  while completing the treatment:", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while completing the treatment: {ex.Message}", ex);
        }
    }


    public IEnumerable<BO.ClosedCallInList> ClosedCallsByVolunteer(int volunteerId, BO.CallType? callType, BO.ClosedCallInListField? sorting)
    {

        IEnumerable<DO.Call> allCalls = _dal.Call.ReadAll();


        var filteredCalls = allCalls.Where(call =>
        {
            var isClosed = CallManager.Status(call.Id) == CallStatus.Closed;
            var assignments = _dal.Assignment.ReadAll(a => a.CallId == call.Id && a.VolunteerId == volunteerId).ToList();
            return isClosed && assignments.Any();
        });

        IEnumerable<BO.ClosedCallInList> boCalls = filteredCalls
            .Select(CallManager.DOToBOClosedCallInList)
            .Where(c => c != null)
            .Cast<BO.ClosedCallInList>();


        if (callType.HasValue)
        {
            boCalls = boCalls.Where(c => c.callType == callType.Value);
        }


        boCalls = sorting switch
        {
            ClosedCallInListField.Id => boCalls.OrderBy(c => c.Id),
            ClosedCallInListField.CallType => boCalls.OrderBy(c => c.callType),
            ClosedCallInListField.Address => boCalls.OrderBy(c => c.Address),
            ClosedCallInListField.OpeningTime => boCalls.OrderBy(c => c.OpeningTime),
            ClosedCallInListField.EntryTime => boCalls.OrderBy(c => c.EntryTime),
            ClosedCallInListField.EndTime => boCalls.OrderBy(c => c.EndTime),
            ClosedCallInListField.EndTimeType => boCalls.OrderBy(c => c.EndTimeType),
            _ => boCalls.OrderBy(c => c.Id),
        };

        return boCalls;
    }

    public IEnumerable<BO.OpenCallInList> OpenCallsByVolunteer(int id, BO.CallType? calltype, BO.OpenCallInListField? Sorting)
    {
        bool ifIsOpen(DO.Call? call)
        {
            if (call == null) throw new BO.BlObjectNotFoundException("Call not found");
            return CallManager.Status(call.Id) == CallStatus.Open || CallManager.Status(call.Id) == CallStatus.OpenAtRisk;
        }
        IEnumerable<DO.Call> FilteredCalls = _dal.Call.ReadAll(ifIsOpen);
        IEnumerable<BO.OpenCallInList> boCalls = FilteredCalls.Select(CallManager.DOToBOOpenCallInList);

        if (calltype != null)
        {
            boCalls = boCalls.Where(c => c.callType == calltype);
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

        boCalls = Sorting switch
        {
            OpenCallInListField.Id => boCalls.OrderBy(c => c.Id),
            OpenCallInListField.callType => boCalls.OrderBy(c => c.callType),
            OpenCallInListField.description => boCalls.OrderBy(c => c.description),
            OpenCallInListField.Address => boCalls.OrderBy(c => c.Address),
            OpenCallInListField.OpeningTime => boCalls.OrderBy(c => c.OpeningTime),
            OpenCallInListField.maxEndingTime => boCalls.OrderBy(c => c.maxEndingTime),
            OpenCallInListField.CallDistanceFromVolunteer => boCalls.OrderBy(c => c.CallDistanceFromVolunteer),
            _ => boCalls.OrderBy(c => c.Id)
        };
        return boCalls;



    }

}





