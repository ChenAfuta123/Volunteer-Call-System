
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

                // בדיקת תעודת זהות
                if (!IsValidID(id))
                    throw new Exception("Invalid ID.");

                // בדיקת שם המתנדב
                if (string.IsNullOrWhiteSpace(volunteer.Name))
                    throw new Exception("Name cannot be empty.");

                // בדיקת מספר טלפון
                if (!IsValidPhoneNumber(volunteer.PhoneNumber))
                    throw new Exception("Invalid phone number.");

                // בדיקת כתובת
                if (!IsValidLength(volunteer.Address, 10, 100))
                    throw new Exception("Invalid address.");

                // בדיקת קווי אורך ורוחב
                if (!IsValidCoordinates(volunteer.Latitude, volunteer.Longitude))
                    throw new Exception("Invalid latitude and longitude.");

                // בדיקת מרחק מקסימלי
                if (volunteer.MaxDistance.HasValue && volunteer.MaxDistance.Value <= 0)
                    throw new Exception("Max distance must be a positive value.");

                // בדיקת סיסמה (אם קיימת)
                if (!IsValidPassword(volunteer.Password))
                    throw new Exception("Password must be at least 6 characters long.");

                // בדיקת סוג מרחק (distanceType)
                if (!Enum.IsDefined(typeof(DistanceType), volunteer.distanceType))
                    throw new Exception("Invalid distance type.");

                // בדיקת סה"כ שיחות
                if (volunteer.TotalHandledCalls < 0 || volunteer.TotalCanceledCalls < 0 || volunteer.TotalExpiredCalls < 0)
                    throw new Exception("Total handled, canceled, and expired calls must be non-negative.");

                // אם כל הבדיקות עברו בהצלחה
                return true;
            }
            catch (Exception ex)
            {
                // תפיסת השגיאה וזריקת חריגה עם הודעה חדשה
                throw new Exception("Error validating volunteer details: " + ex.Message);
            }
        }

        // פונקציות עזר לבדוק את תקינות הנתונים (לפי מה שהגדרנו קודם)

        // פונקציה לבדיקת תקינות אימייל
        private static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }

        // פונקציה לבדיקת אם תעודת זהות תקינה
        private static bool IsValidID(int Id)
        {
           string id= Id.ToString();
            // תעודת זהות חייבת להיות מספר בעל 9 ספרות
            if (id.Length != 9 || !id.All(char.IsDigit))
                return false;

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

            return true;
        }
        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            // כאן ניתן להוסיף בדיקה למספר טלפון (למשל, האם הוא מתחיל עם "+" ומכיל רק מספרים)
            return  phoneNumber.Length == 10 && phoneNumber.All(c => char.IsDigit(c));
        }
    }
}
