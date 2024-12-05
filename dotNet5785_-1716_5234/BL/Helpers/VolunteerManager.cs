
using DalApi;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BO;
using System.Net;
//using Newtonsoft.Json.Linq;


namespace Helpers
{
    
    internal static class VolunteerManager
    {
        private static IDal s_dal = Factory.Get;


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

               if(!Tools.DistanceCalculator.IsValidAddress(volunteer.Address, volunteer.Longitude, volunteer.Latitude))
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