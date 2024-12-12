using BlApi;
using BO;
using DalApi;
using DO;
using Helpers;
using Microsoft.VisualBasic;
using System;
using System.Net.Http.Headers;
namespace BL
{
    internal class Program
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                DisplayMainMenu();

                string input = Console.ReadLine()!;
                switch (input)
                {
                    case "0":
                        exit = true;
                        break;

                    case "1":
                        ManageVolunteer();
                        break;

                    case "2":
                        ManageCall();
                        break;

                    case "3":
                        ManageAdmin();
                        break;

                    case "4":
                        ManageAdmin();
                        break;

                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        /// <summary>
        /// Displays the main menu of options.
        /// </summary>
        private static void DisplayMainMenu()
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("0 - Exit");
            Console.WriteLine("1 - Volunteer Operations");
            Console.WriteLine("2 - Call Operations");
            Console.WriteLine("3 - Admin Operations");
            Console.WriteLine("4 - Admin Operations");
        }
        private static void ManageVolunteer()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nVolunteer Operations:");
                Console.WriteLine("0 - Return to Main Menu");
                Console.WriteLine("1 - Add Volunteer");
                Console.WriteLine("2 - Login Volunteer");
                Console.WriteLine("3 - Read All Volunteers");
                Console.WriteLine("4 - Read Specific Volunteer");
                Console.WriteLine("5 - Update Volunteer");
                Console.WriteLine("6 - Delete Volunteer");

                string input = Console.ReadLine()!;
                switch (input)
                {
                    case "0":
                        exit = true;
                        break;

                    case "1":
                        AddVolunteer();
                        break;

                    case "2":
                        LoginVolunteer();
                        break;

                    case "3":
                        ReadAllVolunteers();
                        break;

                    case "4":
                        ReadSpecificVolunteer();
                        break;

                    case "5":
                        UpdateVolunteer();
                        break;

                    case "6":
                        DeleteVolunteer();
                        break;

                    default:
                        Console.WriteLine("Invalid input. Please select a valid option.");
                        break;
                }
            }
        }

        private static void AddVolunteer()

        {
            try
            {
                var volunteer = VolunteerDetailsFromUser(true, 0, BO.Role.volunteer, 0, 0, 0);
                s_bl.Volunteer.Add(volunteer);
                Console.WriteLine("Volunteer added successfully");
            }
            catch(BO.BlValidationException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlAlreadyExistsException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void LoginVolunteer()
        {
            try
            {
                Console.WriteLine("Enter Volunteer name:"); 
                string? name = Console.ReadLine()!;
                Console.WriteLine("Enter Volunteer password:"); 
                string? password = Console.ReadLine()!;
                DO.Role role = s_bl.Volunteer.LoginUser(name, password);
                Console.WriteLine($"Welcome {role}");
            }
            catch (BO.BlValidationException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlObjectNotFoundException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void ReadAllVolunteers()
        {
            Console.Write("Filter by active volunteers? (true/false): ");
            bool? filterByActive = bool.Parse(Console.ReadLine()!);

         
            Console.WriteLine("Choose a field to sort the list by:");
            foreach (var field in Enum.GetValues(typeof(VolunteerInListFields)))
            {
                Console.WriteLine($"- {field} ({(int)field})");
            }
            try
            {
                
                Console.Write("Enter the number corresponding to the field: ");
                VolunteerInListFields? sortField = (VolunteerInListFields)Enum.Parse(typeof(VolunteerInListFields), Console.ReadLine()!);
                var volunteerList = s_bl.Volunteer.ReadAll(filterByActive, sortField);
                PrintvolunteerInList(volunteerList);
            }
            catch (BO.BlNullPropertyException ex)
            {
                Console.WriteLine(ex);
            }
            catch(BO.BlDoesNotExistsException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlObjectNotFoundException ex)
            {
                Console.WriteLine(ex);
            }

        }

        private static void ReadSpecificVolunteer()
        {
            try
            {
                int id;
                Console.WriteLine("Enter Volunteer ID:");
                string input = Console.ReadLine()!;

                int.TryParse(input, out id);
                var BOVolunteer = s_bl.Volunteer.Read(id);
                PrintVolunteer(BOVolunteer);
            }
            catch (BO.BlDoesNotExistsException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlObjectNotFoundException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void UpdateVolunteer()
        {
            try
            {
                int ID;
                Console.WriteLine("Enter your ID:");
                string input1 = Console.ReadLine()!;

                int.TryParse(input1, out ID);


                int id;
                Console.WriteLine("Enter volunteer ID to update:");
                string input2 = Console.ReadLine()!;

                int.TryParse(input2, out id);

                var Volunteer = s_bl.Volunteer.Read(id);
                var volunteer = VolunteerDetailsFromUser(false, Volunteer.Id, Volunteer.role, Volunteer.TotalHandledCalls, Volunteer.TotalCanceledCalls, Volunteer.TotalExpiredCalls);
                s_bl.Volunteer.Update(ID, volunteer);
                Console.WriteLine("Volunteer is update successfully");
            }
            catch (BO.BlObjectNotFoundException ex)
            {
                Console.WriteLine(ex);
            }

            catch (BO.BlUnauthorizedException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlDoesNotExistsException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlValidationException ex)
            {
                Console.WriteLine(ex);
            }

        }

        private static void DeleteVolunteer()
        {
            try
            {
                int id;
                Console.WriteLine("Enter volunteer ID deletion.");
                string input = Console.ReadLine()!;

                int.TryParse(input, out id);

                s_bl.Volunteer.Delete(id);
            }
            catch (BO.BlDoesNotExistsException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlCannotBeDeletedException ex)
            {
                Console.WriteLine(ex);
            }
        }






        /// <summary>
        /// Manage Call-related operations.
        /// </summary>
        private static void ManageCall()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nCall Operations:");
                Console.WriteLine("0 - Return to Main Menu");
                Console.WriteLine("1 - Get Call Quantities");
                Console.WriteLine("2 - Read All Calls");
                Console.WriteLine("3 - Read Specific Call");
                Console.WriteLine("4 - Update Call");
                Console.WriteLine("5 - Delete Call");
                Console.WriteLine("6 - Add Call");
                Console.WriteLine("7 - Open Calls by Volunteer");
                Console.WriteLine("8 - End of Treatment Update");
                Console.WriteLine("9 - Cancel Treatment Update");
                Console.WriteLine("10 - Choose Call for Treatment");
                Console.WriteLine("11 - Get Closed Calls by Volunteer");

                string input = Console.ReadLine()!;
                switch (input)
                {
                    case "0":
                        exit = true;
                        break;

                    case "1":
                        GetCallQuantities();
                        break;

                    case "2":
                        ReadAllCalls();
                        break;

                    case "3":
                        ReadSpecificCall();
                        break;

                    case "4":
                        UpdateCall();
                        break;

                    case "5":
                        DeleteCall();
                        break;

                    case "6":
                        AddCall();
                        break;

                    case "7":
                        OpenCallsByVolunteer();
                        break;

                    case "8":
                        EndOfTreatmentUpdate();
                        break;

                    case "9":
                        CancelTreatmentUpdate();
                        break;

                    case "10":
                        ChooseCallForTreatment();
                        break;

                    case "11":
                        GetClosedCallsByVolunteer();
                        break;

                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        private static void GetCallQuantities()
        {
            int[] statusCounts = s_bl.Call.CallQuantities();

          
            string[] statusNames = Enum.GetNames(typeof(CallStatus));

            Console.WriteLine("Call Quantities by Status:");

            for (int i = 0; i < statusCounts.Length; i++)
            {
                Console.WriteLine($"{statusNames[i]}: {statusCounts[i]}");
            }

        }

        private static void ReadAllCalls()
        {

            Console.WriteLine("Select a field to filter by:");
            foreach (var value in Enum.GetValues(typeof(CallInListField)))
            {
                Console.WriteLine($"{(int)value + 1}. {value}");
            }
            CallInListField? filterField = int.TryParse(Console.ReadLine(), out int input1)
     ? (CallInListField?)(input1 - 1)
     : null;

            Console.Write("Enter a value to filter by (or leave empty to skip): ");
            var filterValue = Console.ReadLine();

       
            Console.WriteLine("Select a field to sort by:");
            foreach (var value in Enum.GetValues(typeof(CallInListField)))
            {
                Console.WriteLine($"{(int)value + 1}. {value}");
            }
            CallInListField? sortField = int.TryParse(Console.ReadLine(), out int input2)
             ? (CallInListField?)(input2 - 1)
             : null;

            try
            {
                var callinlist = s_bl.Call.ReadAll(filterField, filterValue, sortField);
                PrintCallInList(callinlist);
            }
            catch (BO.BlNullPropertyException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlValidationException ex)
            {
                Console.WriteLine(ex);
            }

        }

        private static void ReadSpecificCall()
        {
            try
            {
                int id;
                Console.WriteLine("Enter call ID:");
                string input = Console.ReadLine()!;

                if (!int.TryParse(input, out id))
                {
                    throw new BO.BlValidationException("Invalid ID entered");
                }

                var BOcall = s_bl.Call.Read(id);
                PrintCall(BOcall);
            }
            catch (BO.BlObjectNotFoundException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void UpdateCall()
        {
            try
            {
                Console.Write("Enter ID: ");
                int id = int.Parse(Console.ReadLine()!);
                var call = s_bl.Call.Read(id);
                var callToupdate = CallInput(call.Id);
                s_bl.Call.Update(callToupdate);
            }
            catch (BO.BlDoesNotExistsException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlObjectNotFoundException ex)
            {
                Console.WriteLine(ex);
            }

        }

        private static void DeleteCall()
        {
            try
            {
                Console.Write("Enter ID of call to delete ");
                int id = int.Parse(Console.ReadLine()!);
                s_bl.Call.Delete(id);
                Console.Write("Deleted successfully");
            }
            catch (BO.BlDoesNotExistsException ex)
            {
                Console.WriteLine(ex);
            }
            catch(BO.BlCannotBeDeletedException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void AddCall()
        {
            try
            {
                Console.WriteLine("Enter call details: ");
                var call = CallInput(0);
                s_bl.Call.Add(call);
            }
            catch (BO.BlAlreadyExistsException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlValidationException ex)
            {
                Console.WriteLine(ex);
            }

        }

        private static void OpenCallsByVolunteer()
        {
            Console.Write("Enter volunteer ID: ");
            int id = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Select a field to sort closed calls by:");
            var sortFields = Enum.GetValues(typeof(OpenCallInListField));
            for (int i = 0; i < sortFields.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {sortFields.GetValue(i)}");
            }

            Console.Write("Your choice: ");
            int sortFieldIndex = int.Parse(Console.ReadLine()!) - 1;
            OpenCallInListField? sortField = (OpenCallInListField)sortFields.GetValue(sortFieldIndex)!;

            Console.WriteLine("Select a call type:");
            var callTypes = Enum.GetValues(typeof(BO.CallType));
            for (int i = 0; i < callTypes.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {callTypes.GetValue(i)}");
            }

            Console.Write("Your choice: ");
            int callTypeIndex = int.Parse(Console.ReadLine()!) - 1;
            BO.CallType? callTypeFilter = (BO.CallType)callTypes.GetValue(callTypeIndex)!;

            try
            {
                var openCallList = s_bl.Call.OpenCallsByVolunteer(id, callTypeFilter, sortField);
                printOpenCallInList(openCallList);
            }
            catch(BO.BlObjectNotFoundException ex) 
            {
                Console.WriteLine(ex);
            }   
        }

        private static void EndOfTreatmentUpdate()
        {
            try
            {
                int Vid, Aid;
                Console.Write("Enter Volunteer ID: ");
                Vid = int.Parse(Console.ReadLine()!);
                Console.Write("Enter Assignment ID: ");
                Aid = int.Parse(Console.ReadLine()!);
                s_bl.Call.EndOftreatmentUpdate(Vid, Aid);
            }
            catch (BO.BlObjectNotFoundException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlDoesNotExistsException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlValidationException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlUnauthorizedException ex)
            {
                Console.WriteLine(ex);
            }

        }

        private static void CancelTreatmentUpdate()
        {
            try
            {
                int Vid, Cid;
                Console.Write("Enter Volunteer ID: ");
                Vid = int.Parse(Console.ReadLine()!);
                Console.Write("Enter call ID: ");
                Cid = int.Parse(Console.ReadLine()!);
                s_bl.Call.CanceltreatmentUpdate(Vid, Cid);
            }
            catch (BO.BlObjectNotFoundException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlDoesNotExistsException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlValidationException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlUnauthorizedException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void ChooseCallForTreatment()
        {
            try
            {
                int Vid, Cid;
                Console.Write("Enter Volunteer ID: ");
                Vid = int.Parse(Console.ReadLine()!);
                Console.Write("Enter call ID: ");
                Cid = int.Parse(Console.ReadLine()!);
                s_bl.Call.ChooseCallForTreatment(Vid, Cid);
            }
            catch (BO.BlObjectNotFoundException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlAlreadyExistsException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlValidationException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void GetClosedCallsByVolunteer()
        {
            Console.Write("Enter volunteer ID: ");
            int id = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Select a field to sort closed calls by:");
            var sortFields = Enum.GetValues(typeof(ClosedCallInListField));
            for (int i = 0; i < sortFields.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {sortFields.GetValue(i)}");
            }

            Console.Write("Your choice: ");
            int sortFieldIndex = int.Parse(Console.ReadLine()!) - 1;
            ClosedCallInListField? sortField = (ClosedCallInListField)sortFields.GetValue(sortFieldIndex)!;

            Console.WriteLine("Select a call type:");
            var callTypes = Enum.GetValues(typeof(BO.CallType));
            for (int i = 0; i < callTypes.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {callTypes.GetValue(i)}");
            }

            Console.Write("Your choice: ");
            int callTypeIndex = int.Parse(Console.ReadLine()!) - 1;
            BO.CallType? callTypeFilter = (BO.CallType)callTypes.GetValue(callTypeIndex)!;
            try
            {
                var closedCallInList = s_bl.Call.ClosedCallsByVolunteer(id, callTypeFilter, sortField);
                printClosedCallInList(closedCallInList);
            }
            catch (BO.BlObjectNotFoundException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BlDoesNotExistsException ex)
            {
                Console.WriteLine(ex);
            }

        }




        private static void ManageAdmin()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nAdmin Menu:");
                Console.WriteLine("0 - Exit");
                Console.WriteLine("1 - Get Clock Time");
                Console.WriteLine("2 - Advance Clock");
                Console.WriteLine("3 - Get Risk Time Range");
                Console.WriteLine("4 - Set Risk Time Range");
                Console.WriteLine("5 - Reset Database");
                Console.WriteLine("6 - Set Database");

                string input = Console.ReadLine()!;
                switch (input)
                {
                    case "0":
                        exit = true;
                        break;

                    case "1":
                        // קריאה למתודה getClockTime
                        GetClockTime();
                        break;

                    case "2":
                        // קריאה למתודה AdvanceClock
                        AdvanceClock();
                        break;

                    case "3":
                        // קריאה למתודה getRiskTimeRange
                        GetRiskTimeRange();
                        break;

                    case "4":
                        // קריאה למתודה setRiskTimeRange
                        SetRiskTimeRange();
                        break;

                    case "5":
                        // קריאה למתודה resetDatabase
                        ResetDatabase();
                        break;

                    case "6":
                        // קריאה למתודה setDatabase
                        SetDatabase();
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private static void GetClockTime()
        {
            // מתודה שתקרא getClockTime ותבצע את כל ההדפסות והבדיקות
            Console.WriteLine(s_bl.Admin.getClockTime());
        }

        private static void AdvanceClock()
        {
            Console.WriteLine("Select a Time Unit:");
            foreach (var value in Enum.GetValues(typeof(TimeUnit)))
            {
                Console.WriteLine($"{(int)value + 1}. {value}");
            }

            int unitChoice = int.Parse(Console.ReadLine()!) - 1;

            TimeUnit timeUnit = (TimeUnit)unitChoice;
           s_bl.Admin.AdvanceClock(timeUnit);
        }

        private static void GetRiskTimeRange()
        {
            Console.Write("Risk time range is: ");
            Console.Write(s_bl.Admin.getRiskTimeRange());
        }

        private static void SetRiskTimeRange()
        {
            Console.Write("Enter Risk Time Range (hh:mm:ss): ");
            var riskTimeRange = TimeSpan.Parse(Console.ReadLine()!);
            s_bl.Admin.setRiskTimeRange(riskTimeRange);
        }

        private static void ResetDatabase()
        {
            s_bl.Admin.resetDatabase();
        }

        private static void SetDatabase()
        {
           s_bl.Admin.setDatabase();
        }











        private static void PrintVolunteer(BO.Volunteer volunteer)
        {
            Console.WriteLine($"ID: {volunteer.Id}");
            Console.WriteLine($"Role: {volunteer.role}");
            Console.WriteLine($"Name: {volunteer.Name}");
            Console.WriteLine($"Phone Number: {volunteer.PhoneNumber}");
            Console.WriteLine($"Email: {volunteer.Email}");
            Console.WriteLine($"Password: {volunteer.Password}");
            Console.WriteLine($"Address: {volunteer.Address}");
            Console.WriteLine($"Latitude: {volunteer.Latitude}");
            Console.WriteLine($"Longitude: {volunteer.Longitude}");
            Console.WriteLine($"Max Distance: {volunteer.MaxDistance}");
            Console.WriteLine($"Active: {volunteer.Active}");
            Console.WriteLine($"Distance Type: {volunteer.distanceType}");
            Console.WriteLine($"Total Handled Calls: {volunteer.TotalHandledCalls}");
            Console.WriteLine($"Total Canceled Calls: {volunteer.TotalCanceledCalls}");
            Console.WriteLine($"Total Expired Calls: {volunteer.TotalExpiredCalls}");
            Console.WriteLine($"Volunteer Handled Call: {volunteer.VolunteerHandledCall}");
            if (volunteer.VolunteerHandledCall != null)
            {
                Console.WriteLine("Details of the volunteer's handled call:");
                PrintCallInProgress(volunteer.VolunteerHandledCall);
            }
        }
        private static BO.Volunteer VolunteerDetailsFromUser(bool flag, int ID, BO.Role R, int t1, int t2, int t3)
        {
            int id = 0;
            BO.Role role = BO.Role.volunteer;
            int totalHandledCalls = 0;
            int totalCanceledCalls = 0;
            int totalExpiredCalls = 0;
            if (flag)
            {
                Console.Write("Enter ID: ");
                id = int.Parse(Console.ReadLine()!);

                Console.WriteLine("Choose Role: 1. Volunteer 2. Manager");

              
                role = Console.ReadLine() switch
                {
                    "1" => BO.Role.volunteer,
                    "2" => BO.Role.manager,
                    _ => BO.Role.volunteer 
                };
            }
            Console.Write("Enter Name: ");
            string name = Console.ReadLine()!;

            Console.Write("Enter Phone Number: ");
            string phoneNumber = Console.ReadLine()!;

            Console.Write("Enter Email: ");
            string email = Console.ReadLine()!;

            Console.Write("Enter Password (optional): ");
            string? password = Console.ReadLine();

            Console.Write("Enter Address (optional): ");
            string? address = Console.ReadLine();

            Console.Write("Enter Max Distance (optional): ");
            double? maxDistance = double.TryParse(Console.ReadLine(), out double maxDistResult) ? maxDistResult : null;

            Console.Write("Is Active (true/false): ");
            bool active = bool.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter the distance type (0 - AirDistance, 1 - WalkingDistance, 2 - DrivingDistance):");
            BO.DistanceType distanceType = Console.ReadLine() switch
            {
                "O" => BO.DistanceType.AirDistance,
                "1" => BO.DistanceType.WalkingDistance,
                "2" => BO.DistanceType.DrivingDistance,
                _ => BO.DistanceType.AirDistance 
            };


            return new BO.Volunteer
            {
                Id = id ,
                role = role,
                Name = name,
                PhoneNumber = phoneNumber,
                Email = email,
                Password = password,
                Address = address,
                MaxDistance = maxDistance,
                Active = active,
                distanceType = distanceType,
                TotalHandledCalls = totalHandledCalls,
                TotalCanceledCalls = totalCanceledCalls,
                TotalExpiredCalls = totalExpiredCalls,
            };
         }
        private static void PrintvolunteerInList(IEnumerable<BO.VolunteerInList> volunteerList)
        {
            
                foreach (var volunteer in volunteerList)
                {
                    Console.WriteLine("Volunteer Details:");
                    Console.WriteLine($"- Id: {volunteer.Id}");
                    Console.WriteLine($"- Name: {volunteer.Name}");
                    Console.WriteLine($"- Active: {volunteer.Active}");
                    Console.WriteLine($"- Total Handled Calls: {volunteer.TotalHandledCalls}");
                    Console.WriteLine($"- Total Canceled Calls: {volunteer.TotalCanceledCalls}");
                    Console.WriteLine($"- Total Expired Calls: {volunteer.TotalExpiredCalls}");
                    Console.WriteLine($"- Handled Call Id: {(volunteer.HandledCallId.HasValue ? volunteer.HandledCallId.Value.ToString() : "None")}");
                    Console.WriteLine();
                }
            
        }


        private static void PrintCall(BO.Call call)
        {
            Console.WriteLine($"ID: {call.Id}");
            Console.WriteLine($"Call Type: {call.callType}");
            Console.WriteLine($"Description: {call.Description ?? "N/A"}");
            Console.WriteLine($"Address: {call.Address ?? "N/A"}");
            Console.WriteLine($"Opening Time: {call.OpeningTime}");
            Console.WriteLine($"Max Ending Time: {call.MaxEndingTime?.ToString() ?? "N/A"}");
            Console.WriteLine($"Call Status: {call.callStatus}");
            Console.WriteLine("Call Assign List:");
            if (call.CallAssignList != null && call.CallAssignList.Count > 0)
            {
                foreach (var assign in call.CallAssignList)
                {
                    Console.WriteLine($"- {assign}"); // assuming CallAssignInList has a meaningful ToString() override
                }
            }
            else
            {
                Console.WriteLine("  No assignments.");
            }
        }
        private static void printOpenCallInList(IEnumerable<OpenCallInList> openCallInList)
        {
           
                foreach (var call in openCallInList)
                {
                    Console.WriteLine($"- ID: {call.Id}");
                    Console.WriteLine($"- Call Type: {call.callType}");
                    Console.WriteLine($"- Address: {call.Address}");
                    Console.WriteLine($"- Opening Time: {call.OpeningTime}");
                    Console.WriteLine($"- Description: {call.description}");
                    Console.WriteLine($"- Max Ending Time: {call.maxEndingTime.HasValue}");
                    Console.WriteLine($"- Call Distance From Volunteer: {call.CallDistanceFromVolunteer}");
                    Console.WriteLine(new string('-', 40)); 
                }
            
        }
        private static void printClosedCallInList(IEnumerable<ClosedCallInList> closedCallInList)
        {
           
            
                foreach (var call in closedCallInList)
                {
                    Console.WriteLine($"- ID: {call.Id}");
                    Console.WriteLine($"- Call Type: {call.callType}");
                    Console.WriteLine($"- Address: {call.Address}");
                    Console.WriteLine($"- Opening Time: {call.OpeningTime}");
                    Console.WriteLine($"- Entry Time: {call.EntryTime}");
                    Console.WriteLine($"- End Time: {call.EndTime.HasValue}");
                    Console.WriteLine($"- End Time Type: {(call.EndTimeType.HasValue ? call.EndTimeType.ToString() : "N/A")}");
                    Console.WriteLine(new string('-', 40)); 
                }
            
        }
        public static BO.Call CallInput(int id)
        {
            Console.Write("Enter Call Type: ");
            var callType = (BO.CallType)Enum.Parse(typeof(BO.CallType), Console.ReadLine()!);

            Console.Write("Enter Description: ");
            var description = Console.ReadLine();

            Console.Write("Enter Address: ");
            var address = Console.ReadLine()!;

            Console.Write("Enter Max Ending Time (yyyy-MM-dd HH:mm:ss): ");
            string input = Console.ReadLine()!.Trim();
            if (!DateTime.TryParse(input, out DateTime parsedDate))
            {
                Console.WriteLine("kjhgf");
            }
            //var maxEndingTime = DateTime.Parse(Console.ReadLine()!);






            var call = new BO.Call
            {
                Id = id,
                callType = callType,
                Description = description,
                Address = address,
                MaxEndingTime = s_bl.Admin.getClockTime(),
                OpeningTime = s_bl.Admin.getClockTime()

            };

            return call;
        }
        public static void PrintCallInList(IEnumerable<CallInList> calls)
        {
            foreach (var call in calls)
            {
                Console.WriteLine($"Id: {call.Id}");
                Console.WriteLine($"CallId: {call.CallId}");
                Console.WriteLine($"CallType: {call.callType}");
                Console.WriteLine($"OpeningTime: {call.OpeningTime}");
                Console.WriteLine($"RemainingCallTime: {call.RemainingCallTime}");
                Console.WriteLine($"LastVolunteerName: {call.LastVolunteerName}");
                Console.WriteLine($"TotalHandlingTime: {call.TotalHandlingTime}");
                Console.WriteLine($"CallStatus: {call.callStatus}");
                Console.WriteLine($"TotalAllocations: {call.TotalAllocations}");
            }
        }
        public static void PrintCallInProgress(CallInProgress callInProgress)
        {
            Console.WriteLine($"Id: {callInProgress.Id}");
            Console.WriteLine($"CallId: {callInProgress.CallId}");
            Console.WriteLine($"Call Type: {callInProgress.callType}");
            Console.WriteLine($"Address: {callInProgress.Address}");
            Console.WriteLine($"Opening Time: {callInProgress.OpeningTime}");
            Console.WriteLine($"Description: {callInProgress.Description ?? "N/A"}");
            Console.WriteLine($"Max Ending Time: {callInProgress.maxEndingTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A"}");
            Console.WriteLine($"Entry Time: {callInProgress.EntryTime}");
            Console.WriteLine($"Call Distance From Volunteer: {callInProgress.CallDistanceFromVolunteer}");
            Console.WriteLine($"Call Status: {callInProgress.callStatus}");
        }



    }
}