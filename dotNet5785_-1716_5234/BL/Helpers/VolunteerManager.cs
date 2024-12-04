

using BO;
using DalApi;
using DO;

namespace Helpers;

internal static class VolunteerManager
{

    private static IDal s_dal = Factory.Get;
    public static CallStatus Status(int callId)
    {

        return CallStatus.Open;
    }
    public static BO.Volunteer DOtoBO(DO.Volunteer volunteer)
    {
        var Assignment = s_dal.Assignment.ReadAll().FirstOrDefault(assignment => assignment.VolunteerId == volunteer.Id);
        var call = s_dal.Call.ReadAll().FirstOrDefault(call => call.Id == Assignment.CallId);
        var volunteerHandledCall = Assignment != null && call != null
          ? new BO.CallInProgress
          {
              Id = Assignment.Id,
              CallId = call.Id,
              callType = call.callType, // סוג הקריאה
              Address = call.Address,
              OpeningTime = call.OpeningTime,
              Description = call.Description,
              maxEndingTime = call.MaxEndingTime,
              EntryTime = Assignment.EntryTime,
              CallDistanceFromVolunteer = Helpers.CalculateDistance(volunteer.Latitude, volunteer.Longitude, call.Latitude, call.Longitude),
              callStatus = Status(call.Id) // סטטוס הקריאה
          };
          : null;
        return new BO.Volunteer
        {
            Id = volunteer.Id,
            Name = volunteer.Name,
            PhoneNumber = volunteer.PhoneNumber,
            Email = volunteer.Email,
            Password = volunteer.Password,
            Address = volunteer.Address,
            Latitude = volunteer.Latitude,
            Longitude = volunteer.Longitude,
            MaxDistance = volunteer.MaxDistance,
            Active = volunteer.Active,
            distanceType = volunteer.distanceType,
            role = volunteer.role,
            TotalHandledCalls = ,
            TotalCanceledCalls = 2,
            TotalExpiredCalls = 1,
            VolunteerHandledCall = volunteerHandledCall
        };

    }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error retrieving volunteer details.", ex); }
}
    public static int g(Assignment )
{
    int num = s_dal.Assignment.ReadAll()
    .Count(assignment => assignment.EndTimeType == Treated);
}

