
using BO;
using DalApi;
using DO;
using System.Net;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace Helpers;

internal static class CallManager
{
    private static IDal s_dal = Factory.Get;
    public static IEnumerable<BO.CallInList> FilterCalls(IEnumerable<BO.CallInList> calls, BO.CallInListField? filterField, object? filterValue)
    {
        if (filterField == null || filterValue == null)
        {
            return calls;
        }

        return filterField switch
        {
            BO.CallInListField.CallType when filterValue is string =>
                calls.Where(c => c.callType.Equals(filterValue)),

            BO.CallInListField.CallStatus when filterValue is string =>
               calls.Where(c => c.Id.HasValue && CallManager.Status(c.Id.Value).Equals(filterValue)),

            BO.CallInListField.OpeningTime when filterValue is DateTime =>
                calls.Where(c => c.OpeningTime.Equals((DateTime)filterValue)),

            BO.CallInListField.RemainingCallTime when filterValue is TimeSpan =>
                calls.Where(c => c.RemainingCallTime.Equals((TimeSpan)filterValue)),

            BO.CallInListField.LastVolunteerName when filterValue is string =>
                calls.Where(c => c.LastVolunteerName != null && c.LastVolunteerName.Equals(filterValue)),

            BO.CallInListField.TotalHandlingTime when filterValue is TimeSpan =>
                calls.Where(c => c.TotalHandlingTime.Equals((TimeSpan)filterValue)),

            BO.CallInListField.TotalAllocations when filterValue is int =>
                calls.Where(c => c.TotalAllocations == (int)filterValue),

            _ => throw new BO.BlNullPropertyException($"Unsupported or mismatched filter field")
        };
    }
    public static void HandleOpenCall(int callId, int volunteerId, CallStatus callStatus)
    {

        var call = s_dal.Call.Read(callId);
        if (call == null) throw new BO.BlObjectNotFoundException($"Call does not exist.");



        if (call.maxEndingTime.HasValue && call.maxEndingTime.Value < DateTime.Now)
        {
            throw new BO.BlValidationException("The call's validity period has expired.");
        }


        var assignments = s_dal.Assignment.ReadAll()
            .Where(assign => assign.CallId == callId && assign.EndTime == null);
        if (assignments.Any())
        {
            throw new BO.BlValidationException("The call is already assigned to another volunteer.");
        }


        var newAssignment = new DO.Assignment
        {
            Id = 0,
            CallId = callId,
            VolunteerId = volunteerId,
            EntryTime = ClockManager.Now,
            EndTimeType = null,
            EndTime = null
        };

        try
        {
            s_dal.Assignment.Create(newAssignment);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Error while creating a Assignment:", ex);
        }


    }

    public static BO.CallInList DOToBOCallInList(DO.Call doCall)
    {
        try
        {

            var assignments = s_dal.Assignment.ReadAll(a => a.CallId == doCall.Id).ToList();

            var lastAssignment = assignments
                .OrderByDescending(a => a.EntryTime)
                .FirstOrDefault();


            var lastVolunteer = lastAssignment != null
                ? s_dal.Volunteer.Read(lastAssignment.VolunteerId)
                : null;


            var remainingCallTime = doCall.maxEndingTime.HasValue
                ? doCall.maxEndingTime.Value - ClockManager.Now
                : (TimeSpan?)null;

            var totalHandlingTime = lastAssignment?.EndTime.HasValue == true
                ? lastAssignment.EndTime.Value - doCall.OpeningTime
                : (TimeSpan?)null;

            var totalAllocations = assignments.Count;

            return new BO.CallInList
            {
                Id = lastAssignment?.Id,
                CallId = doCall.Id,
                callType = (BO.CallType)doCall.callType,
                OpeningTime = doCall.OpeningTime,
                RemainingCallTime = remainingCallTime,
                LastVolunteerName = lastVolunteer?.Name,
                TotalHandlingTime = totalHandlingTime,
                callStatus = Status(doCall.Id),
                TotalAllocations = totalAllocations
            };
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Error while reading a call:", ex);
        }
    }

    public static BO.Call DOtoBO(DO.Call call)
    {

        var assignments = s_dal.Assignment.ReadAll(assignment =>
        assignment.CallId == call.Id).ToList();


        List<BO.CallAssignInList>? callAssignInList = assignments.Select(assignment =>
        {
            var volunteer = s_dal.Volunteer.Read(assignment.VolunteerId);
            if (volunteer == null) throw new BO.BlObjectNotFoundException("The volunteer  was not found.");

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
            callStatus = Status(call.Id),
            CallAssignList = callAssignInList
        };

    }
    public static BO.OpenCallInList DOToBOOpenCallInList(DO.Call doCall)
    {
        try
        {
            var assignment = s_dal.Assignment.Read(a => a.CallId == doCall.Id);
            if (assignment == null) throw new BO.BlObjectNotFoundException("Assignment not found.\"");
            var volunteer = s_dal.Volunteer.Read(assignment.VolunteerId);
            if (volunteer == null) throw new BO.BlObjectNotFoundException("volunteer not foundl.\"");



            return new BO.OpenCallInList
            {
                Id = doCall.Id,
                callType = (BO.CallType)doCall.callType,
                description = doCall.Description,
                Address = doCall.Address,
                OpeningTime = doCall.OpeningTime,
                maxEndingTime = doCall.maxEndingTime,
                CallDistanceFromVolunteer = Tools.DistanceCalculator.CalculateDistance(doCall.Address, volunteer.Address, volunteer.distanceType)
            };
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Error while reading a call:", ex);
        }
    }
    public static BO.ClosedCallInList DOToBOClosedCallInList(DO.Call doCall)
    {
        try
        {
            var assignments = s_dal.Assignment.ReadAll(a => a.CallId == doCall.Id).ToList();


            var assignment = assignments.LastOrDefault();


            if (assignment == null) throw new BO.BlObjectNotFoundException("Assignment not found.\"");


            return new BO.ClosedCallInList
            {
                Id = doCall.Id,
                callType = (BO.CallType)doCall.callType,
                Address = doCall.Address,
                OpeningTime = doCall.OpeningTime,
                EntryTime = assignment.EntryTime,
                EndTime = assignment.EndTime,
                EndTimeType = (BO.EndTimeType)assignment.EndTimeType!
            };
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Error while reading a call:", ex);
        }
    }
    public static bool ValidateCall(BO.Call call)
    {
        try
        {
           
            if (!Enum.IsDefined(typeof(BO.CallType), call.callType))
                throw new Exception("Invalid call type.");

            if (!Enum.IsDefined(typeof(CallStatus), call.callStatus))
                throw new Exception("Invalid call status.");


            if (string.IsNullOrEmpty(call.Description))
                throw new Exception("Invalid call description.");


            //if (!Tools.DistanceCalculator.IsValidAddress(call.Address, call.Longitude, call.Latitude))
            //    throw new Exception("Invalid Address.");

            if (call.OpeningTime == default)
                throw new Exception("Opening time is required.");

            if (call.MaxEndingTime.HasValue && call.MaxEndingTime <= call.OpeningTime)
                throw new Exception("Max ending time must be later than opening time.");

            return true;
        }
        catch (Exception ex)
        {
            throw new BlValidationException("Error validating call details: " + ex.Message);
        }

    }
    internal static BO.CallStatus Status(int callId)
    {
        try
        {
            DO.Call? call = s_dal.Call.Read(a => a.Id == callId);
            if (call == null) throw new BO.BlObjectNotFoundException("Call not found.\"");



            var now = ClockManager.Now;


            if (s_dal.Assignment.Read(a => a.CallId == call.Id && a.EndTime == null) != null)
            {
                return BO.CallStatus.InProgress;
            }


            if (call.maxEndingTime.HasValue && now > call.maxEndingTime.Value)
            {
                return BO.CallStatus.Expired;
            }


            if (s_dal.Assignment.Read(a => a.CallId == call.Id && a.EndTime != null) != null)
            {
                return BO.CallStatus.Closed;
            }


            if (call.maxEndingTime.HasValue && now > call.OpeningTime.AddHours(1))
            {
                return BO.CallStatus.OpenAtRisk;
            }


            return BO.CallStatus.Open;

        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Error while reading a call:", ex);
        }

    }
    internal static void CloseExpiredCalls(DateTime oldClock, DateTime newClock)
    {
        var allCalls = s_dal.Call.ReadAll(); 
        foreach (DO.Call call in allCalls)
        {
           
            if (call.maxEndingTime != null && call.maxEndingTime.Value < newClock)
            {
                BO.Call currentCall =DOtoBO(call);

               
                if (currentCall.callStatus != CallStatus.Closed)
                {
                    var assignments = s_dal.Assignment.ReadAll(a => a.CallId == call.Id).ToList();

                    if (assignments.Count() > 0) 
                    {
                       
                        var assignment = assignments.LastOrDefault(a => !a.EndTime.HasValue);
                        if (assignment != null)
                        {
                            var updatedAssignment = assignment with
                            {
                                EndTime = newClock,
                                EndTimeType = DO.EndTimeType.Expired
                            };

                            s_dal.Assignment.Update(updatedAssignment);
                        }
                    }

                  
                    currentCall.callStatus = CallStatus.Closed;
                    s_dal.Call.Update(call);
                }
            }
        }
    }
}



