
using BO;
using DalApi;
using DO;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
namespace Helpers
{
    internal static class VolunteerManager
    {
        private static IDal s_dal = Factory.Get;
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
                    CallDistanceFromVolunteer = Tools.CalculateDistance(volunteer.Address, call.Address, volunteer.distanceType),
                    callStatus = Tools.Status(call.Id) // סטטוס הקריאה צריך לממש את סטטוס
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
        public static bool ValidateVolunteer(BO.Volunteer volunteer)
        {
            try
            {


                if (!IsValidID(volunteer.Id))
                    throw new Exception("Invalid Id.");

                if (!IsValidName(volunteer.Name))
                    throw new Exception("Invalid Name.");

                if (!IsValidPhoneNumber(volunteer.PhoneNumber))
                    throw new Exception("Invalid Phone number.");

                if (!IsValidEmail(volunteer.Email))
                    throw new Exception("Invalid Email.");

                if (!IsValidPassword(volunteer.Password))
                    throw new Exception("Invalid Password.");

                if (!Enum.IsDefined(typeof(DistanceType), volunteer.distanceType))
                    throw new Exception("Invalid distance type.");

                if (!Enum.IsDefined(typeof(Role), volunteer.distanceType))
                    throw new Exception("Invalid role.");

                Tools.NullVal(volunteer.MaxDistance);
                if (volunteer.MaxDistance.HasValue && volunteer.MaxDistance.Value <= 0)
                    throw new Exception("Max distance must be a positive value.");

                if (!Tools.DistanceCalculator.IsValidAddress(volunteer.Address, volunteer.Longitude, volunteer.Latitude))
                    throw new Exception("Invalid Address.");

                if (volunteer.TotalHandledCalls < 0 || volunteer.TotalCanceledCalls < 0 || volunteer.TotalExpiredCalls < 0)
                    throw new Exception("Total handled, canceled, and expired calls must be non-negative.");

                return true;
            }
            catch (Exception ex)
            {

                throw new ValidationException("Error validating volunteer details: " + ex.Message);
            }
        }

        private static bool IsValidID(int Id)
        {
            string id = Id.ToString();

            if (id.Length != 9 || !id.All(char.IsDigit))
                return false;

            int sum = 0;
            for (int i = 0; i < 8; i++)
            {
                int digit = int.Parse(id[i].ToString());
                sum += (i % 2 == 0) ? digit : digit * 2;
            }
            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == int.Parse(id[8].ToString());
        }
        private static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;
            if (name.Length < 2 || name.Length > 12)
                return false;
            if (!name.All(c => char.IsLetter(c)))
                return false;
            return true;
        }
        private static bool IsValidPhoneNumber(string phoneNumber)
        {

            return phoneNumber.Length == 10 && (phoneNumber.All(c => char.IsDigit(c)));


        }


        private static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }

        private static bool IsValidPassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;


            if (password.Length < 6 || password.Length > 30)
                return false;


            if (!password.Any(c => !char.IsLetterOrDigit(c)))
                return false;

            return true;
        }


    }
}

}
