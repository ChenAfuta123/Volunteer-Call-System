
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
using System.Runtime.CompilerServices;
namespace Helpers;


internal static class VolunteerManager
{
    private static IDal s_dal = Factory.Get;
    internal static ObserverManager Observers = new();
    public static int TotalEndTimeType(int Vid,DO.EndTimeType endTimeType)
    {
        lock (AdminManager.BlMutex)
            return s_dal.Assignment.ReadAll()
      .Count(assignment => assignment.EndTimeType == endTimeType&& assignment.VolunteerId == Vid);

    }
    public static BO.Volunteer DOtoBO(DO.Volunteer? volunteer)
    {
        try
        {
            BO.CallInProgress? volunteerHandledCall = null;
            if (volunteer == null) throw new BO.BlObjectNotFoundException("volunteer not found.");
            lock (AdminManager.BlMutex)
            {

                var assignment = s_dal.Assignment.Read(a => a.VolunteerId == volunteer.Id && a.EndTime == null);
                if (assignment != null)
                {

                    var call = s_dal.Call.Read(call => call.Id == assignment.CallId);
                    if (call != null)
                    {

                        volunteerHandledCall = new BO.CallInProgress
                        {
                            Id = assignment.Id,
                            CallId = call.Id,
                            callType = (BO.CallType)call.callType,
                            Address = call.Address,
                            OpeningTime = call.OpeningTime,
                            Description = call.Description,
                            maxEndingTime = call.maxEndingTime,
                            EntryTime = assignment.EntryTime,
                            CallDistanceFromVolunteer = Tools.DistanceCalculator.CalculateDistance(volunteer.Latitude, volunteer.Longitude,
                            call.Latitude, call.Longitude, volunteer.distanceType),
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
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Error while reading a volunteer:", ex);
        }
        //catch (Exception ex)
        //{
        //    throw new Exception($"Unexpected error while  reading a volunteer: {ex.Message}");
        //}


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
    public static bool ValidateVolunteer(BO.Volunteer volunteer,bool flag)
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
            if (flag)
            {
                if (!IsValidPassword(volunteer.Password))
                    throw new Exception("\"Weak password: Password must be at least 5 characters long and contain only letters and digits. " +
                        "It must also include at least 2 letters and 3 digits.\""
                    );
            }

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
    public static bool IfisClose(DO.Volunteer v, DO.Call call)
    {
        double distance =  Tools.DistanceCalculator.CalculateDistance(v.Latitude, v.Longitude,
            call.Latitude, call.Longitude, v.distanceType);
        return v.MaxDistance <= distance;
    }
    private static bool IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        // Check length constraints
        if (name.Length < 2 || name.Length > 12)
            return false;

        // Ensure only letters and spaces are allowed
        if (!name.All(c => char.IsLetter(c) || c == ' '))
            return false;

        // Ensure no leading, trailing, or multiple consecutive spaces
        if (name.StartsWith(' ') || name.EndsWith(' ') || name.Contains("  "))
            return false;

        return true;
    }

    private static DO.Call? IfHandledCall(DO.Volunteer volunteer)
    {
        var assignment = s_dal.Assignment.Read(assignment => assignment.VolunteerId == volunteer.Id &&
        assignment.EndTime == null);
        if (assignment != null)
        {
            return s_dal.Call.Read(call => call.Id == assignment.CallId);
        }
        return null;
    }
    private static void CancelTreatment(Assignment assignment)
    {
        assignment = assignment with
        {
            EndTime = AdminManager.Now,
            EndTimeType = DO.EndTimeType.ManagerCancel
        };
        s_dal.Assignment.Update(assignment);
        CallManager.Observers.NotifyItemUpdated(assignment.CallId);
        CallManager.Observers.NotifyListUpdated();
    }

    private static int s_simulatorCounter = 0;
    private static readonly Random s_rand = new Random();

    internal static void SimulateVolunteerCallHandling() //stage 7
    {
        Thread.CurrentThread.Name = $"Simulator{++s_simulatorCounter}";

        LinkedList<int> volunteersToUpdate = new(); //stage 7
        List<DO.Volunteer> doVolunteerList;

        lock (AdminManager.BlMutex) //stage 7
            doVolunteerList = s_dal.Volunteer.ReadAll(st => st.Active == true).ToList();

        foreach (var doVolunteer in doVolunteerList)
        {
            int VolunteerId = doVolunteer.Id;

            //stage 7

            var Call = IfHandledCall(doVolunteer);

            if (Call == null)
            {
                //BO.Year studentYear = GetStudentCurrentYear(doVolunteer.RegistrationDate);

                //the above method, includes network requests to compute the distances
                //between courses address and current student address
                //these network requests are done synchronically
                //var coursesNotRegistered = CallManager.GetUnRegisteredCallForValunteer(doStudent.Id, studentYear);
                IEnumerable<DO.Call> calls;
                lock (AdminManager.BlMutex)
                    calls = s_dal.Call.ReadAll(call => (CallManager.Status(call.Id) == BO.CallStatus.Open||
               CallManager.Status(call.Id) == BO.CallStatus.OpenAtRisk) && IfisClose(doVolunteer, call));
                int size = calls.Count();
                if (size != 0)
                {
                    int callId = calls.Skip(s_rand.Next(0, size)).First()!.Id;
                    CallManager.HandleOpenCall(callId, doVolunteer.Id, CallManager.Status(callId));
                    VolunteerId = doVolunteer.Id;
                }
            }
            else
            {
                var Assignment = s_dal.Assignment.Read(assignment => assignment.CallId == Call.Id);
                if (Assignment!.EntryTime <= DateTime.Now.AddHours(-10))
                {
                    Assignment = Assignment with
                    {
                        EndTime = AdminManager.Now,
                        EndTimeType = DO.EndTimeType.Treated
                    };


                    s_dal.Assignment.Update(Assignment);
                    Observers.NotifyItemUpdated(Assignment.CallId);
                    CallManager.Observers.NotifyListUpdated();
                }
                else
                {
                    if (s_rand.NextDouble() <= 0.1)
                    {
                        // ביטול הטיפול בקריאה
                        CancelTreatment(Assignment);
                    }
                }

            } //lock

        }

        foreach (int id in volunteersToUpdate)
        {
            Observers.NotifyItemUpdated(id);
        }
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



        if (password.Length < 5)
         {
        return false;
         }

    int letterCount = 0;
    int digitCount = 0;

    // סורקים כל תו בסיסמא
    foreach (var ch in password)
    {
        if (char.IsLetter(ch))
        {
            letterCount++;  // סופרים את האותיות
        }
        else if (char.IsDigit(ch))
        {
            digitCount++;   // סופרים את הספרות
        }
        else
        {
            // אם יש תו שלא אות או ספרה, נחזיר false
            return false;
        }
    }
   
    // בודקים אם יש לפחות 2 אותיות ושהסיסמא מכילה רק אותיות וספרות
    return letterCount >= 2 && digitCount >=/* password.Length - 2*/3;
     }

 }      
    

