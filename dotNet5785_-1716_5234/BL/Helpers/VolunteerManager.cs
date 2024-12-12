
using DalApi;
using DO;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;
using System.Threading.Tasks;
//using System.ComponentModel.DataAnnotations;
//using BO;
using System.Net;
namespace Helpers;


internal static class VolunteerManager
{
    private static IDal s_dal = Factory.Get;
    public static int TotalEndTimeType(int Vid,DO.EndTimeType endTimeType)
    {

        return s_dal.Assignment.ReadAll()
      .Count(assignment => assignment.EndTimeType == endTimeType&& assignment.VolunteerId == Vid);

    }
    public static BO.Volunteer DOtoBO(DO.Volunteer? volunteer)
    {
        try
        {
            BO.CallInProgress? volunteerHandledCall = null;
            if (volunteer == null) throw new BO.BlObjectNotFoundException("volunteer not found.");

            var Assignment = s_dal.Assignment.Read(assignment => assignment.VolunteerId == volunteer.Id);

            if (Assignment != null)
            {
                var call = s_dal.Call.Read(call => call.Id == Assignment.CallId);
                if (call != null)
                {
                    volunteerHandledCall = new BO.CallInProgress
                    {
                        Id = Assignment.Id,
                        CallId = call.Id,
                        callType = (BO.CallType)call.callType,
                        Address = call.Address,
                        OpeningTime = call.OpeningTime,
                        Description = call.Description,
                        maxEndingTime = call.maxEndingTime,
                        EntryTime = Assignment.EntryTime,
                        CallDistanceFromVolunteer = Tools.DistanceCalculator.CalculateDistance(volunteer.Address, call.Address, volunteer.distanceType),
                        callStatus = CallManager.Status(call.Id)
                    };
                }
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
                distanceType = (BO.DistanceType)volunteer.distanceType,
                role = (BO.Role)volunteer.role,
                TotalHandledCalls = TotalEndTimeType(volunteer.Id, EndTimeType.Treated),
                TotalCanceledCalls = TotalEndTimeType(volunteer.Id, EndTimeType.ManagerCancel) + TotalEndTimeType(volunteer.Id, EndTimeType.SelfCancel),
                TotalExpiredCalls = TotalEndTimeType(volunteer.Id, EndTimeType.Expired),
                VolunteerHandledCall = volunteerHandledCall
            };

        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Error while reading a volunteer:", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error while  reading a volunteer: {ex.Message}");
        }


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
            if (!Tools.IsValidID(volunteer.Id))
                throw new Exception("Invalid Id.");

            if (!IsValidName(volunteer.Name))
                throw new Exception("Invalid Name.");

            if (!IsValidPhoneNumber(volunteer.PhoneNumber))
                throw new Exception("Invalid Phone number.");

            if (!IsValidEmail(volunteer.Email))
                throw new Exception("Invalid Email.");

            //if (!IsValidPassword(volunteer.Password))
            //    throw new Exception("Invalid Password.");

            if (!Enum.IsDefined(typeof(BO.DistanceType), volunteer.distanceType))
                throw new Exception("Invalid distance type.");

            if (!Enum.IsDefined(typeof(BO.Role), volunteer.role))
                throw new Exception("Invalid role.");

            Tools.NullVal(volunteer.MaxDistance);
            if (volunteer.MaxDistance.HasValue && volunteer.MaxDistance.Value <= 0)
                throw new Exception("Max distance must be a positive value.");

            //if (!Tools.DistanceCalculator.IsValidAddress(volunteer.Address, volunteer.Longitude, volunteer.Latitude))
            //    throw new Exception("Invalid Address.");

            if (volunteer.TotalHandledCalls < 0 || volunteer.TotalCanceledCalls < 0 || volunteer.TotalExpiredCalls < 0)
                throw new Exception("Total handled, canceled, and expired calls must be non-negative.");

            return true;
        }
        catch (Exception ex)
        {

            throw new BO.BlValidationException("Error validating volunteer details: " + ex.Message);
        }
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
    

