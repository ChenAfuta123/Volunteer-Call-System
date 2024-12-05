
using DalApi;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Helpers
{
    internal static class VolunteerManager
    {
        private static IDal s_dal = Factory.Get;


        public static bool ValidateVolunteer(BO.Volunteer volunteer, int id)
        {
            try
            {
                // בדיקת אימייל
                if (!IsValidEmail(volunteer.Email))
                    throw new Exception("Invalid email address.");

using BO;
using DalApi;
using DO;

            // חישוב ספרת ביקורת
            int sum = 0;
            for (int i = 0; i < 8; i++)
            {
                int digit = int.Parse(id[i].ToString());
                sum += (i % 2 == 0) ? digit : digit * 2;
            }
            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == int.Parse(id[8].ToString());
        }

        // פונקציה לבדוק אם קו אורך ורוחב תקינים/
        private static bool IsValidCoordinates(double? latitude, double? longitude)
        {
            latitude = latitude ?? throw new ArgumentNullException(nameof(latitude), "Latitude cannot be null.");
            longitude = longitude ?? throw new ArgumentNullException(nameof(longitude), "Longitude cannot be null.");
            return latitude >= -90 && latitude <= 90 && longitude >= -180 && longitude <= 180;
        }

        // פונקציה לבדוק אם שדה מספרי הוא אכן מספרי
        private static bool IsValidNumber(string input)
        {
            return double.TryParse(input, out _);
        }

        // פונקציה לבדוק אם אורך שדה תקין
        private static bool IsValidLength(string? input, int? minLength, int? maxLength)
        {
            input = input ?? throw new ArgumentNullException(nameof(input), "Input cannot be null.");
            minLength = minLength ?? throw new ArgumentNullException(nameof(minLength), "Minimum length cannot be null.");
            maxLength = maxLength ?? throw new ArgumentNullException(nameof(maxLength), "Maximum length cannot be null.");

            return input.Length >= minLength && input.Length <= maxLength;
        }
        private static bool IsValidPassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // אורך מינימלי של 6 תווים
            if (password.Length < 6)
                return false;

            // לפחות ספרה אחת
            if (!password.Any(char.IsDigit))
                return false;

            // לפחות אות אחת רישית
            if (!password.Any(char.IsUpper))
                return false;

            // לפחות אות אחת קטנה
            if (!password.Any(char.IsLower))
                return false;

            // לפחות תו מיוחד
            if (!password.Any(c => !char.IsLetterOrDigit(c)))
                return false;

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

