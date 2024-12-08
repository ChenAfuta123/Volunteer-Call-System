using BO;
using DalApi;
using System.ComponentModel.DataAnnotations;

namespace Helpers;

internal static class CallManager
{
    private static IDal s_dal = Factory.Get;
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


