using BlApi;
using BO;
using DalApi;
using DO;
using Helpers;
using Microsoft.VisualBasic;
using System;
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
                        ManageAssignment();
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
            Console.WriteLine("3 - Assignment Operations");
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
            var volunteer = VolunteerDetailsFromUser(true, 0, BO.Role.volunteer, 0,0,0);
            s_bl.Volunteer.Add(volunteer);
            Console.WriteLine("Volunteer added successfully");
        }

        private static void LoginVolunteer()
        {
            Console.WriteLine("Enter Volunteer name:"); // בקשה למזהה
            string name = Console.ReadLine()!;
            Console.WriteLine("Enter Volunteer password:"); // בקשה למזהה
            string password = Console.ReadLine()!;
            DO.Role role = s_bl.Volunteer.LoginUser(name, password);
            Console.WriteLine($"Welcome {role}");
        }

        private static void ReadAllVolunteers()
        {
            Console.Write("Filter by active volunteers? (true/false): ");
            bool? filterByActive = bool.Parse(Console.ReadLine()!);

            // הצגת אפשרויות Enum
            Console.WriteLine("Choose a field to sort the list by:");
            foreach (var field in Enum.GetValues(typeof(VolunteerInListFields)))
            {
                Console.WriteLine($"- {field} ({(int)field})");
            }

            // בקשה להזנת Enum
            Console.Write("Enter the number corresponding to the field: ");
            VolunteerInListFields? sortField = (VolunteerInListFields)Enum.Parse(typeof(VolunteerInListFields), Console.ReadLine()!);
            var volunteerList = s_bl.Volunteer.ReadAll(filterByActive, sortField);
            if (volunteerList == null)
            throw new BO.BlObjectNotFoundException("volunteer in list is not found"); 
           PrintvolunteerInList(volunteerList);
        }

        private static void ReadSpecificVolunteer()
        {
            int id; // כרזת על משתנה
            Console.WriteLine("Enter Volunteer ID:"); // בקשה למזהה
            string input = Console.ReadLine()!; // קבלת קלט

            if (!int.TryParse(input, out id)) // ניסיון להמיר למספר שלם
            {
                throw new BO.BlValidationException("Invalid ID entered");
            }

            var BOVolunteer = s_bl.Volunteer.Read(id);
            PrintVolunteer(BOVolunteer);
        }

        private static void UpdateVolunteer()
        {
            int ID; // כרזת על משתנה
            Console.WriteLine("Enter your ID:"); // בקשה למזהה
            string input1 = Console.ReadLine()!; // קבלת קלט

            if (!int.TryParse(input1, out ID)) // ניסיון להמיר למספר שלם
            {
                throw new BO.BlValidationException("Invalid ID entered");
            }
            int id; // כרזת על משתנה
            Console.WriteLine("Enter volunteer ID to update:"); // בקשה למזהה
            string input2 = Console.ReadLine()!; // קבלת קלט

            if (!int.TryParse(input2, out id)) // ניסיון להמיר למספר שלם
            {
                throw new BO.BlValidationException("Invalid ID entered");
            }

            var Volunteer = s_bl.Volunteer.Read(id);
            var volunteer = VolunteerDetailsFromUser(false, Volunteer.Id, Volunteer.role, Volunteer.TotalHandledCalls, Volunteer.TotalCanceledCalls, Volunteer.TotalExpiredCalls);
            s_bl.Volunteer.Update(ID, volunteer);
            Console.WriteLine("Volunteer is update successfully");
        }

        private static void DeleteVolunteer()
        {
            int id;
            Console.WriteLine("Enter volunteer ID deletion.");
            string input = Console.ReadLine()!; // קבלת קלט

            if (!int.TryParse(input, out id)) // ניסיון להמיר למספר שלם
            {
                throw new BO.BlValidationException("Invalid ID entered");
            }
            s_bl.Volunteer.Delete(id);
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

        // Placeholder for each method implementation
        private static void GetCallQuantities()
        {
            int[] statusCounts = s_bl.Call.CallQuantities();

            // קבלת שמות הסטטוסים
            string[] statusNames = Enum.GetNames(typeof(CallStatus));

            Console.WriteLine("Call Quantities by Status:");

            for (int i = 0; i < statusCounts.Length; i++)
            {
                Console.WriteLine($"{statusNames[i]}: {statusCounts[i]}");
            }

        }

        private static void ReadAllCalls()
        {
        }

        private static void ReadSpecificCall()
        {
            int id; // כרזת על משתנה
            Console.WriteLine("Enter call ID:"); // בקשה למזהה
            string input = Console.ReadLine()!; // קבלת קלט

            if (!int.TryParse(input, out id)) // ניסיון להמיר למספר שלם
            {
                throw new BO.BlValidationException("Invalid ID entered");
            }

            var BOcall = s_bl.Call.Read(id);
            PrintCall(BOcall);
        }

        private static void UpdateCall()
        {
            Console.Write("Enter ID: ");
            int id = int.Parse(Console.ReadLine()!);
            var call = s_bl.Call.Read(id);
            if (call == null)
            {
                throw new BO.BlObjectNotFoundException("Call not found");
            }
            var callToupdate = UserInput(call.Id);
            s_bl.Call.Update(callToupdate);
        }

        private static void DeleteCall()
        {
            Console.Write("Enter ID of call to delete ");
            int id = int.Parse(Console.ReadLine()!);
            s_bl.Call.Delete(id);
            Console.Write("Deleted successfully");
        }

        private static void AddCall()
        {
            Console.WriteLine("Enter call details ");
            int id;
            Console.Write("Enter ID: ");
            id = int.Parse(Console.ReadLine()!);
            var call=UserInput(id);
            s_bl.Call.Add(call);

        }

        private static void OpenCallsByVolunteer()
        {
            // Logic for calling ICall.OpenCallsByVolunteer
        }

        private static void EndOfTreatmentUpdate()
        {
            // Logic for calling ICall.EndOftreatmentUpdate
        }

        private static void CancelTreatmentUpdate()
        {
            // Logic for calling ICall.CanceltreatmentUpdate
        }

        private static void ChooseCallForTreatment()
        {
            int Vid,Cid;
            Console.Write("Enter Volunteer ID: ");
            Vid = int.Parse(Console.ReadLine()!);
            Console.Write("Enter call ID: ");
            Cid = int.Parse(Console.ReadLine()!);
            s_bl.Call.ChooseCallForTreatment(Vid,Cid);
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
            var closedCallInList = s_bl.Call.GetClosedCallsByVolunteer(id, callTypeFilter, sortField);
            printClosedCallInList(closedCallInList);
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
        }
        private static BO.Volunteer VolunteerDetailsFromUser(bool flag, int ID, BO.Role R, int t1, int t2, int t3)
        {
            int id = 0;
            BO.Role role;
            int totalHandledCalls = 0;
            int totalCanceledCalls = 0;
            int totalExpiredCalls = 0;
            if (flag)
            {
                Console.Write("Enter ID: ");
                id = int.Parse(Console.ReadLine()!);

                Console.Write("Enter Role: ");
                role = Enum.Parse<BO.Role>(Console.ReadLine()!);
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

            Console.Write("Enter Distance Type: ");
            BO.DistanceType distanceType = Enum.Parse<BO.DistanceType>(Console.ReadLine()!);

            if (flag)
            {
                Console.Write("Enter Total Handled Calls: ");
                totalHandledCalls = int.Parse(Console.ReadLine()!);

                Console.Write("Enter Total Canceled Calls: ");
                totalCanceledCalls = int.Parse(Console.ReadLine()!);

                Console.Write("Enter Total Expired Calls: ");
                totalExpiredCalls = int.Parse(Console.ReadLine()!);
            }
            else
            {
                id = ID;
                role = R;
                totalHandledCalls = t1;
                totalCanceledCalls = t2;
                totalExpiredCalls = t3;
            }
        

            return new BO.Volunteer
            {
                Id = id ,
                role =role,
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
            if (volunteerList == null)
            {
                Console.WriteLine("The volunteer list is null or empty.");
                return;
            }
            else
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
                    Console.WriteLine(); // שורה ריקה להפרדה בין מתנדבים
                }
            }
        }


        private static void PrintCall(BO.Call call)
        {
            Console.WriteLine($"ID: {call.Id}");
            Console.WriteLine($"Call Type: {call.callType}");
            Console.WriteLine($"Description: {call.Description ?? "N/A"}");
            Console.WriteLine($"Address: {call.Address ?? "N/A"}");
            Console.WriteLine($"Latitude: {call.Latitude?.ToString() ?? "N/A"}");
            Console.WriteLine($"Longitude: {call.Longitude?.ToString() ?? "N/A"}");
            Console.WriteLine($"Opening Time: {call.OpeningTime}");
            Console.WriteLine($"Max Ending Time: {call.MaxEndingTime?.ToString() ?? "N/A"}");
            Console.WriteLine($"Call Status: {call.CallStatus}");
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
        private static void printClosedCallInList(IEnumerable<ClosedCallInList> closedCallInList)
        {
            if (closedCallInList == null)
            {
              ///אולי חריגה
            }
            else
            {
                foreach (var call in closedCallInList)
                {
                    Console.WriteLine($"- ID: {call.Id}");
                    Console.WriteLine($"- Call Type: {call.callType}");
                    Console.WriteLine($"- Address: {call.Address}");
                    Console.WriteLine($"- Opening Time: {call.OpeningTime}");
                    Console.WriteLine($"- Entry Time: {call.EntryTime}");
                    Console.WriteLine($"- End Time: {(call.EndTime.HasValue ? call.EndTime.ToString() : "Not Ended")}");
                    Console.WriteLine($"- End Time Type: {(call.EndTimeType.HasValue ? call.EndTimeType.ToString() : "N/A")}");
                    Console.WriteLine(new string('-', 40)); // קו מפריד בין אובייקטים
                }
            }
        }
        public static BO.Call UserInput(int id)
        {
            var call = new BO.Call { Id = id};

            Console.Write("Enter Call Type: ");
            call.callType = (BO.CallType)Enum.Parse(typeof(BO.CallType), Console.ReadLine()!);

            Console.Write("Enter Description: ");
            call.Description = Console.ReadLine();

            Console.Write("Enter Address: ");
            call.Address = Console.ReadLine();

            Console.Write("Enter Max Ending Time (yyyy-MM-dd HH:mm:ss): ");
            call.MaxEndingTime = DateTime.Parse(Console.ReadLine()!);

            Console.Write("Enter Call Status: ");
            call.CallStatus = (CallStatus)Enum.Parse(typeof(CallStatus), Console.ReadLine()!);

            Console.Write("Enter Call Assign List (comma-separated): ");
            var assignListInput = Console.ReadLine();
            call.CallAssignList = assignListInput!.Split(',')
                .Select(assign => new BO.CallAssignInList { /* Fill properties if needed */ })
                .ToList();

            return call;
        }

    }
}