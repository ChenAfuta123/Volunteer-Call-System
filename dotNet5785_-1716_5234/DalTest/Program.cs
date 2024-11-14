using Dal;
using DalApi;
using DalList;
using DO;
using System;
using static DalTest.Program;

namespace DalTest;

internal class Program
{
    private static IAssignment? s_dalAssignment = new AssignmentImplementation(); //stage 1
    private static ICall? s_dalCall = new CallImplementation(); //stage 1
    private static IVolunteer? s_dalVolunteer = new VolunteerImplementation(); //stage 1
    private static IConfig? s_dalConfig = new ConfigImplementation(); //stage 1
    public enum MainMenu
    {
        ExitMainMenu = 0,
        VolunteerMenu,
        CallMenu,
        AssignmentMenu,
        DataInitialization,
        ViewAllData,
        ConfigMenu,
        Database_and_configuration_reset
    }
    public enum EntityMenu
    {

        Exit,
        Add,
        View,
        ViewAll,
        Update,
        Delete,
        DeleteAll


    }
    public enum EntityType
    {
        Volunteer,
        Call,
        Assignment
    }
    public enum ConfigMenu
    {
        Exit = 0,
        AdvanceClockByMinute,
        AdvanceClockByHour,
        AdvanceClockByDay,
        DisplayClock,
        SetConfigVariable,
        DisplayConfigVariable,
        ResetConfig
    }

    void CreateVolunteer()
    {
        Console.WriteLine("Enter volunteer's Id:");
        int.TryParse(Console.ReadLine(), out int Id);

        Console.WriteLine("Enter volunteer's name:");
        string name = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter volunteer's phone number:");
        string phoneNumber = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter volunteer's email:");
        string email = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter volunteer's Password:");
        string Password = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter volunteer's Address:");
        string Address = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter volunteer's Latitude:");
        double Latitude = double.TryParse(Console.ReadLine(), out double lat) ? lat : 0.0 ;

        Console.WriteLine("Enter volunteer's MaxDistance:");
        double MaxDistance = double.TryParse(Console.ReadLine(), out double dis) ? dis : 0.0;

        Volunteer newVolunteer = new(Id, name, phoneNumber, email, Password,Address,Latitude, MaxDistance);
        s_dalVolunteer!.Create(newVolunteer);
        Console.WriteLine("Volunteer added successfully.");
    }

    void CreateCall()
    {
       

        Console.WriteLine("Enter call address:");
        string address = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter call description:");
        string description = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter call Latitude:");
        double Latitude = double.TryParse(Console.ReadLine(), out double lat) ? lat : 0.0;

        Console.WriteLine("Enter call Longitude:");
        double Longitude = double.TryParse(Console.ReadLine(), out double lon) ? lon : 0.0;
        Console.WriteLine("Enter call opening time:");

        Console.WriteLine("Enter call ending time:");

        int newCallId = s_dalConfig!.NextCallId;
        Call newCall = new(newCallId, address, Latitude, Longitude,s_dalConfig.Clock ,description);
        s_dalCall!.Create(newCall);
        Console.WriteLine("Call added successfully.");
    }

    void CreateAssignment()
    {
        int newAssignmentId = s_dalConfig!.NextAssignmentId;
        Console.WriteLine("Enter volunteer ID:");
        int.TryParse(Console.ReadLine(), out int volunteerId);

        Console.WriteLine("Enter call ID:");
        int.TryParse(Console.ReadLine(), out int callId);



        Assignment newAssignment = new(newAssignmentId,callId, volunteerId,s_dalConfig.Clock,null,null);
        s_dalAssignment!.Create(newAssignment);
        Console.WriteLine("Assignment added successfully.");
    }
    void ViewVolunteer(int id)
    {
       
        var volunteer = s_dalVolunteer!.Read(id);
        if (volunteer != null)
            Console.WriteLine(volunteer);
        else
            Console.WriteLine("Volunteer not found.");
    }

    void ViewCall(int id)
    {
       
        var call = s_dalCall!.Read(id);
        if (call != null)
            Console.WriteLine(call);
        else
            Console.WriteLine("Call not found.");
    }

    void ViewAssignment(int id)
    {

        var assignment = s_dalAssignment!.Read(id);
        if (assignment != null)
            Console.WriteLine(assignment);
        else
            Console.WriteLine("Assignment not found.");
    }






    public void entityMenu(EntityType entityType)
    {


        bool continueMenu = true;

        while (continueMenu)
        {
            Console.WriteLine("Select an option (0-Exit, 1-Add, 2-View, 3-ViewAll, 4-Update" +
                ", 5-Delete, 6-DeleteAll):");
            int.TryParse(Console.ReadLine(), out int choice);
            EntityMenu selectedMenu = (EntityMenu)choice;
             
            switch (selectedMenu)
            {
                case EntityMenu.Exit:
                    continueMenu = false;
                    break;
                case EntityMenu.Add:
                    try
                    {
                        switch (entityType)
                        {
                            case EntityType.Volunteer:
                                CreateVolunteer();
                                break;
                            case EntityType.Call:
                                CreateCall();
                                break;
                            case EntityType.Assignment:
                                CreateAssignment();
                                break;
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
                    break;
                case EntityMenu.View:
                    Console.WriteLine($"Enter {entityType} ID:");
                    int.TryParse(Console.ReadLine(), out int id);

                    switch (entityType)
                    {
                        case EntityType.Volunteer:
                            ViewVolunteer(id);
                            break;
                        case EntityType.Call:
                            ViewCall(id);
                            break;
                        case EntityType.Assignment:
                            ViewAssignment(id);
                            break;
                    }
                    break;

                case EntityMenu.ViewAll:

                    switch (entityType)
                    {
                        case EntityType.Volunteer:
                          
                            List<Volunteer> volunteers = s_dalVolunteer!.ReadAll();

                            foreach (var volunteer in volunteers)
                            {
                                ViewVolunteer(volunteer.Id);

                            }
                            break;
                        case EntityType.Call:

                            List<Call> calls = s_dalCall!.ReadAll();
                            foreach (var call in calls)
                            {
                                ViewCall(call.Id);
                            }
                            break;

                        case EntityType.Assignment:

                            List<Assignment> assignments = s_dalAssignment!.ReadAll();
                            foreach (var assignment in assignments)
                            {
                                ViewAssignment(assignment.Id);
                            }
                            break;
                    }
                    break;

                case EntityMenu.Update:

                    try
                    {
                        switch (entityType)
                        {
                            case EntityType.Volunteer:


                                // בקשה למשתמש להזין את ה-ID של המתנדב
                                Console.WriteLine("Enter volunteer ID:");
                                int.TryParse(Console.ReadLine(), out int volunteerId);

                                // קריאה לקריאת המתנדב לפי ה-ID
                                Volunteer? exist = s_dalVolunteer!.Read(volunteerId);

                                if (exist == null)
                                {
                                    Console.WriteLine("Volunteer not found.");
                                    break; // אם המתנדב לא קיים, יוצאים מהשיטה
                                }

                                // בקשה מהמשתמש להכניס את הנתונים החדשים לעדכון
                                Console.WriteLine("Enter new data to update:");

                                // קבלת שם המתנדב
                                Console.WriteLine($"Enter new Name (current: {exist.Name}):");
                                string newName = Console.ReadLine() ?? exist.Name; // אם המשתמש לא הכניס שם חדש, נשאיר את השם הקיים

                                // קבלת מספר הטלפון של המתנדב
                                Console.WriteLine($"Enter new PhoneNumber (current: {exist.PhoneNumber}):");
                                string newPhoneNumber = Console.ReadLine() ?? exist.PhoneNumber;

                                // קבלת כתובת האימייל של המתנדב
                                Console.WriteLine($"Enter new Email (current: {exist.Email}):");
                                string newEmail = Console.ReadLine() ?? exist.Email;

                                // קבלת הסיסמה של המתנדב (אפשרי להשאיר את הסיסמה הקיימת)
                                Console.WriteLine($"Enter new Password (current: {exist.Password ?? "N/A"}):");
                                string? newPassword = Console.ReadLine();
                                if (string.IsNullOrEmpty(newPassword))
                                {
                                    newPassword = exist.Password; // אם המשתמש לא הכניס סיסמה חדשה, נשאיר את הקיימת
                                }

                                // קבלת הכתובת של המתנדב
                                Console.WriteLine($"Enter new Address (current: {exist.Address ?? "N/A"}):");
                                string? newAddress = Console.ReadLine();
                                if (string.IsNullOrEmpty(newAddress))
                                {
                                    newAddress = exist.Address; // אם המשתמש לא הכניס כתובת חדשה, נשאיר את הקיימת
                                }

                                // קבלת המיקום של המתנדב (Latitude)
                                Console.WriteLine($"Enter new Latitude (current: {exist.Latitude?.ToString() ?? "N/A"}):");
                                double? newLatitude = double.TryParse(Console.ReadLine(), out double lat) ? lat : exist.Latitude;

                                // קבלת המרחק המקסימלי של המתנדב (MaxDistance)
                                Console.WriteLine($"Enter new MaxDistance (current: {exist.MaxDistance?.ToString() ?? "N/A"}):");
                                double? newMaxDistance = double.TryParse(Console.ReadLine(), out double maxDist) ? maxDist : exist.MaxDistance;

                                // קבלת מצב המתנדב (Active)
                                Console.WriteLine($"Enter new Active status (current: {(exist.Active ? "Yes" : "No")}) - Enter true or false:");
                                bool newActive = bool.TryParse(Console.ReadLine(), out bool activeStatus) ? activeStatus : exist.Active;

                                // יצירת אובייקט מתנדב חדש עם הנתונים שהוזנו
                                Volunteer updatedVolunteer = new Volunteer(
                                    volunteerId, // לא משתנה
                                    newName,
                                    newPhoneNumber,
                                    newEmail,
                                    newPassword,
                                    newAddress,
                                    newLatitude,
                                    newMaxDistance,
                                    newActive
                                );


                                s_dalVolunteer.Update(updatedVolunteer);

                                Console.WriteLine("Volunteer updated successfully.");

                                break;



                            case EntityType.Call:

                                // Prompt user to enter the ID of the call
                                Console.WriteLine("Enter call ID:");
                                int.TryParse(Console.ReadLine(), out int callId);

                                // Fetch the existing call by ID
                                Call? existingCall = s_dalCall!.Read(callId);

                                if (existingCall == null)
                                {
                                    Console.WriteLine("Call not found.");
                                    break; // Exit if the call doesn't exist
                                }

                                // Ask the user to input the new data for the update
                                Console.WriteLine("Enter new data to update:");

                                // Get the call address
                                Console.WriteLine($"Enter new Address (current: {existingCall.Address}):");
                                string newaddress = Console.ReadLine() ?? existingCall.Address;

                                // Get the call description
                                Console.WriteLine($"Enter new Description (current: {existingCall.Description ?? "N/A"}):");
                                string? newDescription = Console.ReadLine();
                                if (string.IsNullOrEmpty(newDescription))
                                {
                                    newDescription = existingCall.Description; // Keep the current description if none provided
                                }

                                // Get the latitude
                                Console.WriteLine($"Enter new Latitude (current: {existingCall.Latitude.ToString() ?? "N/A"}):");
                                double newlatitude = double.TryParse(Console.ReadLine(), out double latitude) ? latitude : existingCall.Latitude;

                                // Get the longitude
                                Console.WriteLine($"Enter new Longitude (current: {existingCall.Longitude.ToString() ?? "N/A"}):");
                                double newLongitude = double.TryParse(Console.ReadLine(), out double longitude) ? longitude : existingCall.Longitude;

                                Console.WriteLine($"Enter new OpeningTime (current: {existingCall.OpeningTime.ToString("g") ?? "N/A"}) - Format: yyyy-MM-dd HH:mm:");
                                string OpeningTimeInput = Console.ReadLine()!;
                                DateTime newOpeningTime = !string.IsNullOrEmpty(OpeningTimeInput) && DateTime.TryParse(OpeningTimeInput, out DateTime openTime) ? openTime : existingCall.OpeningTime;


                                // Get the ending time of the call
                                Console.WriteLine($"Enter new EndTime (current: {existingCall.maxEndingTime?.ToString("g") ?? "N/A"}) - Format: yyyy-MM-dd HH:mm:");
                                string? endTimeInput = Console.ReadLine();
                                DateTime? newMaxEndTime = !string.IsNullOrEmpty(endTimeInput) && DateTime.TryParse(endTimeInput, out DateTime endTime) ? endTime : existingCall.maxEndingTime;

                                // Create a new Call object with the updated data
                                Call updatedCall = new Call(
                                    callId,
                                    newaddress,
                                    newlatitude,
                                    newLongitude,
                                    newOpeningTime,
                                     newDescription,
                                    newMaxEndTime
                                   
                                   
                                    
                                );

                                // Call the update function to update the call
                                s_dalCall.Update(updatedCall);
                                Console.WriteLine("Call updated successfully.");
                                break;


                            case EntityType.Assignment:

                                Console.WriteLine("Unable to update an Assignment.");

                                break;
                        }
 
                    }
                    catch (Exception ex)
                    {
                            // טיפול בחריגה: להדפיס את ההודעה אם יש בעיה
                         Console.WriteLine($"{ex.Message}");
                    }
                    break;
                case EntityMenu.Delete:
                    try
                    {
                        switch (entityType)
                        {
                            case EntityType.Volunteer:
                                Console.WriteLine("Enter the volunteer's ID to delete");
                                int.TryParse(Console.ReadLine(), out int volunteerId);
                                s_dalVolunteer!.Delete(volunteerId);
                                Console.WriteLine("The volunteer deleted successfully");
                                break;
                            case EntityType.Call:
                                Console.WriteLine("Enter the call's ID to delete");
                                int.TryParse(Console.ReadLine(), out int callId);
                                s_dalCall!.Delete(callId);
                                Console.WriteLine("The call deleted successfully");
                                break;
                            case EntityType.Assignment:
                                Console.WriteLine("Enter the assignment's ID to delete");
                                int.TryParse(Console.ReadLine(), out int assignmentId);
                                s_dalAssignment!.Delete(assignmentId);
                                Console.WriteLine("The assignment deleted successfully");
                                break;
                        }
                    }
                    catch(Exception ex) 
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
                    break;
                case EntityMenu.DeleteAll:

                    switch (entityType)
                    {
                        case EntityType.Volunteer:
                           
                            s_dalVolunteer!.DeleteAll();

                            break;
                        case EntityType.Call:
                           
                            s_dalCall!.DeleteAll();
                
                            break;
                        case EntityType.Assignment:
                            
                            s_dalAssignment!.DeleteAll();
                           
                            break;
                    }


                    Console.WriteLine($"Deleting all {entityType} entries.");
                    break;
                default:
                    Console.WriteLine("Invalid menu option.");
                    break;
            }
        }

        Console.WriteLine($"Exited {entityType} menu.");
    }
    public void configMenu()
    {
        bool continueMenu = true;

        while (continueMenu)
        {
            Console.WriteLine("Select an option in the Configuration Menu:");
            Console.WriteLine("0 - Exit Configuration Menu");
            Console.WriteLine("1 - Advance system clock by one minute");
            Console.WriteLine("2 - Advance system clock by one hour");
            Console.WriteLine("3 - Advance system clock by one hour");
            Console.WriteLine("4 - Display current system clock value");
            Console.WriteLine("5 - Set a new value for a configuration variable");
            Console.WriteLine("6 - Display current value of a configuration variable");
            Console.WriteLine("7 - Reset all configuration values");

            int.TryParse(Console.ReadLine(), out int choice);
            ConfigMenu selectedOption = (ConfigMenu)choice;

            switch (selectedOption)
            {
                case ConfigMenu.Exit:
                    continueMenu = false;
                    break;
                case ConfigMenu.AdvanceClockByMinute:
                    s_dalConfig!.Clock = s_dalConfig.Clock.AddMinutes(1);
                    Console.WriteLine("System clock advanced by one minute.");
                    break;
                case ConfigMenu.AdvanceClockByHour:
                    s_dalConfig!.Clock = s_dalConfig.Clock.AddHours(1);
                    Console.WriteLine("System clock advanced by one hour.");
                    break;
                case ConfigMenu.AdvanceClockByDay:
                    s_dalConfig!.Clock = s_dalConfig.Clock.AddDays(1);
                    Console.WriteLine("System clock advanced by one Day.");
                    break;
                case ConfigMenu.DisplayClock:
                    Console.WriteLine(value:$"Current system clock value: {s_dalConfig.Clock}");
                    break;
                case ConfigMenu.SetConfigVariable:
                    Console.Write("Enter the name of the configuration variable to set: ");
                    string? variableName = Console.ReadLine();
                    Console.Write("Enter the new value for the configuration variable: ");
                    string newValue = Console.ReadLine()!;
                    //s_dalConfig!.SetConfigValue(variableName, newValue);
                    Console.WriteLine($"{variableName} has been updated to the new value.");
                    break;
                case ConfigMenu.DisplayConfigVariable:
                    Console.Write("Enter the name of the configuration variable to display: ");
                    string configVarName = Console.ReadLine()!;
                    //string currentValue = s_dalConfig!.GetConfigValue(configVarName);
                    Console.WriteLine($"Current value of {configVarName}: {currentValue}");
                    break;
                case ConfigMenu.ResetConfig:
                    s_dalConfig!.Reset();
                    Console.WriteLine("All configuration variables have been reset.");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please select a valid option.");
                    break;
            }
        }

        Console.WriteLine("Exited Configuration Menu.");
    }

    public void mainMenu()
    {
        bool continueMenu = true;

        while (continueMenu)
        {
            Console.WriteLine("Select a main menu option (0-Exit, 1-Volunteer, 2-Call, 3-Assignment," +
                " 4-Data Init, 5-View All, 6-Config, 7-Reset):");
            int.TryParse(Console.ReadLine(), out int choice);
            MainMenu selectedMenu = (MainMenu)choice;

            switch (selectedMenu)
            {
                case MainMenu.ExitMainMenu:
                    continueMenu = false;
                    break;
                case MainMenu.VolunteerMenu:
                    entityMenu(EntityType.Volunteer);
                    break;
                case MainMenu.CallMenu:
                    entityMenu(EntityType.Call);
                    break;
                case MainMenu.AssignmentMenu:
                    entityMenu(EntityType.Assignment);
                    break;
                case MainMenu.DataInitialization:
                    Console.WriteLine("Initializing data.");
                    Initialization.Do(s_dalVolunteer, s_dalCall, s_dalAssignment, s_dalConfig);
                    break;
                case MainMenu.ViewAllData:
                    List<Volunteer> volunteers = s_dalVolunteer!.ReadAll();
                    foreach (var volunteer in volunteers)
                    {
                        Console.WriteLine($"{volunteer} ");
                    }
                    List<Call> calls = s_dalCall!.ReadAll();
                    foreach (var call in calls)
                    {
                        Console.WriteLine($"{call} ");
                    }
                    List<Assignment> assignments = s_dalAssignment!.ReadAll();
                    foreach (var assignment in assignments)
                    {
                        Console.WriteLine($"{assignment} ");
                    }
                    

    Console.WriteLine("Viewing all data.");
                    break;
                case MainMenu.ConfigMenu:
                    configMenu();
                    break;
                case MainMenu.Database_and_configuration_reset:
                    Console.WriteLine("Resetting database and configuration.");

                    s_dalAssignment!.DeleteAll(); //stage 1
                    s_dalCall!.DeleteAll();
                    s_dalVolunteer!.DeleteAll();
                    //s_dalConfig!.ResetConfig();


                    break;
                default:
                    Console.WriteLine("Invalid menu option.");
                    break;
            }
        }

        Console.WriteLine("Program has exited.");
    }


    static void Main(string[] args)
    {
        
        

    }



}
    

