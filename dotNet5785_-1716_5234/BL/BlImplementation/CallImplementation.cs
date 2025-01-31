namespace BlImplementation;
using BlApi;
using BO;

using Helpers;
using Microsoft.VisualBasic;
using System;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class CallImplementation : ICall
{
    public void AddObserver(Action listObserver) =>
    CallManager.Observers.AddListObserver(listObserver); //stage 5
    public void AddObserver(int id, Action observer) =>
    CallManager.Observers.AddObserver(id, observer); //stage 5
    public void RemoveObserver(Action listObserver) =>
   CallManager.Observers.RemoveListObserver(listObserver); //stage 5
    public void RemoveObserver(int id, Action observer) =>
    CallManager.Observers.RemoveObserver(id, observer); //stage 5

    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    private async Task updateCoordinatesForCallAddressAsync(DO.Call doCall)
    {
        if (doCall.Address is not null)
        {
            var loc = await Tools.DistanceCalculator.GetAddressCoordinatesAsync(doCall.Address);
            double Long = loc.Longitude ?? 0.0;
            double Lat = loc.Latitude ?? 0.0;

            doCall = doCall with { Latitude = Lat, Longitude = Long };
            lock (AdminManager.BlMutex)
                _dal.Call.Update(doCall);
            CallManager.Observers.NotifyListUpdated();
            CallManager.Observers.NotifyItemUpdated(doCall.Id);

        }
    }

    public void Add(BO.Call boCall)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        CallManager.ValidateCall(boCall);
        ;

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
            lock (AdminManager.BlMutex)
                _dal.Call.Create(doCall);
            CallManager.Observers.NotifyItemUpdated(doCall.Id);
            CallManager.Observers.NotifyListUpdated();
            _ = updateCoordinatesForCallAddressAsync(doCall);

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
        AdminManager.ThrowOnSimulatorIsRunning();
        lock (AdminManager.BlMutex)
        {

            try
            {
                if (CanBeDeleted(callId))
                {
                    _dal.Call.Delete(callId);
                    CallManager.Observers.NotifyItemUpdated(callId);
                    CallManager.Observers.NotifyListUpdated();
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
    }
    public BO.Call Read(int callId)
    {

        lock (AdminManager.BlMutex)
        {
            DO.Call? call = _dal.Call.Read(callId);
            if (call == null) throw new BO.BlObjectNotFoundException("Call not found");


            return CallManager.DOtoBO(call);

        }
    }
    public IEnumerable<BO.CallInList> ReadAll(CallInListField? filter, object? obg, CallInListField? sorting)
    {

        lock (AdminManager.BlMutex)
        {
            var calls = _dal.Call.ReadAll();


            IEnumerable<BO.CallInList> CallsInList = calls.Select(CallManager.DOToBOCallInList);

            // סינון
            if (filter != null && obg != null)
            {
                CallsInList = CallManager.FilterCalls(CallsInList, filter, obg);
            }

            // מיון
            if (sorting == null)
            {
                CallsInList = CallsInList.OrderBy(c => c.Id);
            }
            else
            {
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
                    _ => CallsInList.OrderBy(c => c.Id)
                };
            }

            return CallsInList;

        }
    }

    public void Update(BO.Call boCall)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        CallManager.ValidateCall(boCall);


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
            lock (AdminManager.BlMutex)
                _dal.Call.Update(doCall);
            CallManager.Observers.NotifyItemUpdated(doCall.Id);
            CallManager.Observers.NotifyListUpdated();
            _ = updateCoordinatesForCallAddressAsync(doCall);

        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Error while updating a call:", ex);
        }

    }

    public int[] CallQuantities()
    {
        lock (AdminManager.BlMutex)
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

    }

    public void ChooseCallForTreatment(int volunteerId, int callId)
    {
        AdminManager.ThrowOnSimulatorIsRunning();

        AdminManager.ThrowOnSimulatorIsRunning();
        var callStatus = CallManager.Status(callId);


        switch (callStatus)
        {
            case BO.CallStatus.Open:
            case BO.CallStatus.OpenAtRisk:
                CallManager.HandleOpenCall(callId, volunteerId, callStatus);
                break;

            case BO.CallStatus.InProgress:
            case BO.CallStatus.InProgressAtRisk:
                throw new BO.BlValidationException("The call is already in progress and cannot be reassigned.");

            case BO.CallStatus.Closed:
                throw new BO.BlValidationException("The call has already been closed and cannot be assigned.");

            case BO.CallStatus.Expired:
                throw new BO.BlValidationException("The call's validity period has expired and cannot be assigned.");

            default:
                throw new BO.BlValidationException("Unknown call status. Cannot assign the call.");
        }


    }

    public void CanceltreatmentUpdate(int requesterId, int assignmentId)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        try
        {
            lock (AdminManager.BlMutex)
            {
                DO.Assignment? assignment = _dal.Assignment.Read(assignmentId);
                if (assignment == null) throw new BO.BlObjectNotFoundException("Assignment not found");
                DO.Volunteer? volunteer = _dal.Volunteer.Read(requesterId);
                if (volunteer == null) throw new BO.BlObjectNotFoundException("Volunteer not found");


                if (assignment.EndTime != null)
                {
                    throw new BO.BlValidationException("The assignment is already closed.");
                }
                //if (assignment.VolunteerId != requesterId && (BO.Role)volunteer.role == BO.Role.volunteer)
                //{
                //    throw new BO.BlUnauthorizedException("Only the assigned volunteer or manager can complete this treatment.");
                //}

                DO.EndTimeType endTimeType = (assignment.VolunteerId == requesterId)
                    ? DO.EndTimeType.SelfCancel
                    : DO.EndTimeType.ManagerCancel;


                assignment = assignment with
                {

                    EndTime = AdminManager.Now,
                    EndTimeType = endTimeType
                };


                _dal.Assignment.Update(assignment);
                CallManager.Observers.NotifyItemUpdated(assignment.Id);
                CallManager.Observers.NotifyListUpdated();
            }
        }
        catch (DO.DalDoesNotExistsException ex)
        {

            throw new BO.BlDoesNotExistsException($"Error while reading assignment or related data: {ex.Message}", ex);
        }
    }

    public void EndOftreatmentUpdate(int requesterId, int assignmentId)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        try
        {
            lock (AdminManager.BlMutex)
            {
                DO.Assignment? assignment = _dal.Assignment.Read(assignmentId);
                if (assignment == null) throw new BO.BlObjectNotFoundException("Assignment not found");

                //if (assignment!.VolunteerId != requesterId)
                //{
                //    throw new BO.BlUnauthorizedException("Only the assigned volunteer can complete this treatment.");
                //}

                if (assignment.EndTime != null)
                {
                    throw new BO.BlValidationException("This assignment is already completed or canceled.");
                }


                assignment = assignment with
                {
                    EndTime = AdminManager.Now,
                    EndTimeType = DO.EndTimeType.Treated
                };
                _dal.Assignment.Update(assignment);
                CallManager.Observers.NotifyItemUpdated(assignment.Id);
                CallManager.Observers.NotifyListUpdated();
            }
        }
        catch (DO.DalDoesNotExistsException ex)
        {

            throw new BO.BlDoesNotExistsException($"Error  while completing the treatment:", ex);
        }
    }


    public IEnumerable<BO.ClosedCallInList> ClosedCallsByVolunteer(int volunteerId, BO.CallType? callType = null, BO.ClosedCallInListField? sorting = null)
    {
        lock (AdminManager.BlMutex)
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



            if (callType != null)
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
    }

    public IEnumerable<BO.OpenCallInList> OpenCallsByVolunteer(int id, BO.CallType? calltype, BO.OpenCallInListField? Sorting)
    {
        bool ifIsOpen(DO.Call? call)
        {
            if (call == null) throw new BO.BlObjectNotFoundException("Call not found");
            return CallManager.Status(call.Id) == CallStatus.Open || CallManager.Status(call.Id) == CallStatus.OpenAtRisk;
        }
        lock (AdminManager.BlMutex)
        {
            IEnumerable<DO.Call> FilteredCalls = _dal.Call.ReadAll(ifIsOpen);
            IEnumerable<BO.OpenCallInList> boCalls = FilteredCalls.Select(CallManager.DOToBOOpenCallInList);

            if (calltype != null)
            {
                boCalls = boCalls.Where(c => c.callType == calltype);
            }

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

    public IEnumerable<BO.OpenCallInList> ReadAll(OpenCallInListField? filter, object? obg, OpenCallInListField? sorting, int VolunteerID)
    {
        lock (AdminManager.BlMutex)
        {
            var volunteer = _dal.Volunteer.Read(VolunteerID);

            bool ifIsOpen(DO.Call? call)
            {

                return CallManager.Status(call!.Id) == CallStatus.Open || CallManager.Status(call.Id) == CallStatus.OpenAtRisk;
            }
            var calls = _dal.Call.ReadAll(call => ifIsOpen(call) && IfCallCloseToVolunteer(VolunteerID, call));

            

            IEnumerable<BO.OpenCallInList> openCallsInList = calls.Select(call => new BO.OpenCallInList

            {

                Id = call.Id,

                callType = (BO.CallType)call.callType,

                description = call.Description,

                Address = call.Address,

                OpeningTime = call.OpeningTime,

                maxEndingTime = call.maxEndingTime,

                CallDistanceFromVolunteer = Tools.DistanceCalculator.CalculateDistance(volunteer!.Latitude, volunteer.Longitude,
                        call.Latitude, call.Longitude, volunteer.distanceType)
            });



            if (filter != null)

            {

                openCallsInList = CallManager.FilterCalls(openCallsInList, filter, obg);

            }



            if (sorting == null)

            {

                openCallsInList = openCallsInList.OrderBy(c => c.Id);

            }

            else

            {

                openCallsInList = sorting switch

                {

                    OpenCallInListField.Id => openCallsInList.OrderBy(c => c.Id),

                    OpenCallInListField.callType => openCallsInList.OrderBy(c => c.callType),

                    OpenCallInListField.description => openCallsInList.OrderBy(c => c.description),

                    OpenCallInListField.Address => openCallsInList.OrderBy(c => c.Address),

                    OpenCallInListField.OpeningTime => openCallsInList.OrderBy(c => c.OpeningTime),

                    OpenCallInListField.maxEndingTime => openCallsInList.OrderBy(c => c.maxEndingTime),

                    OpenCallInListField.CallDistanceFromVolunteer => openCallsInList.OrderBy(c => c.CallDistanceFromVolunteer),

                    _ => openCallsInList.OrderBy(c => c.Id) // מיון ברירת מחדל לפי Id

                };

            }



            return openCallsInList;

        }
    }


    public int findAssignment(int callID, int? VolunteerID)
    {
        lock (AdminManager.BlMutex)
        {
            var assignment = _dal.Assignment.Read(a => a.CallId == callID && a.VolunteerId == VolunteerID);
            if (assignment != null)
                return assignment.Id;
        }
        return -1;

    }
    public bool CanBeDeleted(int callId)
    {
        return (CallManager.Status(callId) == BO.CallStatus.Open || CallManager.Status(callId) == BO.CallStatus.OpenAtRisk) 
            && !_dal.Assignment.ReadAll(a => a.CallId == callId).Any();
    }
    public bool IfCallCloseToVolunteer(int Vid,DO.Call call)
    {
        var volunteer= _dal.Volunteer.Read(Vid);
        var distance = Tools.DistanceCalculator.CalculateDistance(volunteer!.Latitude, volunteer.Longitude,
            call.Latitude, call.Longitude, volunteer.distanceType);
        return distance <= volunteer.MaxDistance;


    }
   
}