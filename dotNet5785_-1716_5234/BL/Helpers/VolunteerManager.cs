
using BO;
using DalApi;
using DO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
namespace Helpers
{
    internal static class VolunteerManager
    {
        private static IDal s_dal = Factory.Get;
        public static CallStatus Status(int callId)
        {

            return CallStatus.Open;
        }
        public static int TotalEndTimeType(int Vid,EndTimeType endTimeType)
        {

            return s_dal.Assignment.ReadAll()
    .Count(assignment => assignment.EndTimeType == endTimeType&& assignment.VolunteerId == Vid);

        }
        public static BO.Volunteer DOtoBO(DO.Volunteer volunteer)
        {
            BO.CallInProgress ?volunteerHandledCall=null;

            try
            {
        
                var Assignment = s_dal.Assignment.ReadAll().FirstOrDefault(assignment => assignment.VolunteerId == volunteer.Id);
                if (Assignment == null) throw new Exception("Assignment not found.");

                var call = s_dal.Call.ReadAll().FirstOrDefault(call => call.Id == Assignment.CallId);
                if (call == null) throw new Exception("Call not found.");

                 volunteerHandledCall = new BO.CallInProgress
                {
                    Id = Assignment.Id,
                    CallId = call.Id,
                    callType = (BO.CallType)call.callType, // סוג הקריאה
                    Address = call.Address,
                    OpeningTime = call.OpeningTime,
                    Description = call.Description,
                    maxEndingTime = call.maxEndingTime,
                    EntryTime = Assignment.EntryTime,
                    CallDistanceFromVolunteer = CalculateDistance(volunteer.Address, call.Address, volunteer.distanceType),
                    callStatus = Status(call.Id) // סטטוס הקריאה צריך לממש את סטטוס
                };
            }
            catch (Exception)
            {
                throw new Exception("Assignment not found.");
            }

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
                role = (BO.Role)volunteer.role,
                TotalHandledCalls = TotalEndTimeType(volunteer.Id,EndTimeType.Treated),
                TotalCanceledCalls = TotalEndTimeType(volunteer.Id,EndTimeType.ManagerCancel) + TotalEndTimeType(volunteer.Id,EndTimeType.SelfCancel),
                TotalExpiredCalls = TotalEndTimeType(volunteer.Id, EndTimeType.Expired),
                VolunteerHandledCall = volunteerHandledCall
            };
        }
        public static BO.VolunteerInList VolunteerToVolunteerList(BO.Volunteer volunteer)
        {
            return new BO.VolunteerInList
            {
                Id= volunteer.Id,
                Name =volunteer.Name,
                Active=volunteer.Active,
                TotalHandledCalls=volunteer.TotalHandledCalls,
                TotalCanceledCalls=volunteer.TotalCanceledCalls,
                TotalExpiredCalls=volunteer.TotalExpiredCalls,
                HandledCallId=volunteer.VolunteerHandledCall?.Id
            };
        }
    }

}
