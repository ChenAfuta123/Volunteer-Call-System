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
    public static BO.Call DOtoBO(DO.Call call)
    {
        // שלב 1: שליפת כל ה-Assignments הרלוונטיים לאותו ID
        var assignments = s_dal.Assignment.ReadAll()
            .Where(assignment => assignment.CallId == call.Id)
            .ToList();

        // שלב 2: יצירת רשימה של BO.CallAssignInList
        var callAssignInList = assignments.Select(assignment => new BO.CallAssignInList
        {
            VolunteerId = assignment.VolunteerId,
            Name = assignment.Name,
            EntryTime = assignment.EntryTime,
            EndTime = assignment.EndTime,
            EndTimeType = assignment.EndTimeType
        }).ToList();
            return new BO.Call
        {
            Id = call.Id,
            callType = call.callType,
            Address = call.Address,
            Latitude = call.Latitude,
            Longitude = call.Longitude,
            OpeningTime = call.OpeningTime,
            description = call.Description,
            maxEndingTime = call.maxEndingTime,
            callStatus = Tools.Status(call.Id)
            CallAssignList= callAssignInList
            };
    }
    public static bool ValidateCall(BO.Call call)
    {
        try
        {
            if (!Tools.IsValidID(call.Id))
                throw new Exception("Invalid Id.");

            if (!Enum.IsDefined(typeof(CallType), call.callType))
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
}


