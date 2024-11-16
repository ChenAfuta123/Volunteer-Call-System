using Dal;
using DalApi;
using DalList;
using DO;
using System;
using System.Security.Cryptography.X509Certificates;
using static DalTest.Program;

namespace DalTest;

/// <summary>
/// Entry point of the application. Contains the main program logic and menu navigation.
/// </summary>
internal class Program
{
    static readonly IDal s_dal = new DalList();
    /// <summary>
    /// Enum for the main menu options.
    /// </summary>
    public enum MainMenu
    {
        ExitMainMenu = 0, // Exit the main menu.
        VolunteerMenu,    // Navigate to volunteer menu.
        CallMenu,         // Navigate to call menu.
        AssignmentMenu,   // Navigate to assignment menu.
        DataInitialization, // Initialize data in the system.
        ViewAllData,      // View all data in the system.
        ConfigMenu,       // Navigate to configuration menu.
        Database_and_configuration_reset // Reset database and configuration.
    }

    /// <summary>
    /// Enum for the entity-specific menu options.
    /// </summary>
    public enum EntityMenu
    {
        Exit,      // Exit the entity menu.
        Add,       // Add a new entity.
        View,      // View a specific entity.
        ViewAll,   // View all entities.
        Update,    // Update an existing entity.
        Delete,    // Delete an entity.
        DeleteAll  // Delete all entities.
    }

    /// <summary>
    /// Enum representing the types of entities managed in the system.
    /// </summary>
    public enum EntityType
    {
        Volunteer, // Volunteer entity type.
        Call,      // Call entity type.
        Assignment // Assignment entity type.
    }

    /// <summary>
    /// Enum for the configuration menu options.
    /// </summary>
    public enum ConfigMenu
    {
        Exit = 0,           // Exit the configuration menu.
        AdvanceClockByMinute, // Advance the system clock by one minute.
        AdvanceClockByHour,   // Advance the system clock by one hour.
        AdvanceClockByDay,    // Advance the system clock by one day.
        DisplayClock,         // Display the current system clock.
        SetConfigVariable,    // Set a configuration variable.
        DisplayConfigVariable, // Display a configuration variable.
        ResetConfig           // Reset configuration settings to default.
    }

    /// <summary>
    /// Handles the creation of a new volunteer.
    /// </summary>
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
        double Latitude = double.TryParse(Console.ReadLine(), out double lat) ? lat : 0.0;

        Console.WriteLine("Enter volunteer's MaxDistance:");
        double MaxDistance = double.TryParse(Console.ReadLine(), out double dis) ? dis : 0.0;

        Volunteer newVolunteer = new(Id, name, phoneNumber, email, Password, Address, Latitude, MaxDistance);
        s_dal!.Volunteer.Create(newVolunteer);
        Console.WriteLine("Volunteer added successfully.");
    }

    /// <summary>
    /// Handles the creation of a new call.
    /// </summary>
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

        int newCallId = s_dal!.Config.NextCallId;
        Call newCall = new(newCallId, address, Latitude, Longitude, s_dal!.Config.Clock, description, s_dal!.Config.Clock.AddHours(24));
        s_dal!.Call.Create(newCall);
        Console.WriteLine("Call added successfully.");
    }

    /// <summary>
    /// Handles the creation of a new assignment.
    /// </summary>
    void CreateAssignment()
    {
        int newAssignmentId = s_dal!.Config.NextAssignmentId;
        Console.WriteLine("Enter volunteer ID:");
        int.TryParse(Console.ReadLine(), out int volunteerId);

        Console.WriteLine("Enter call ID:");
        int.TryParse(Console.ReadLine(), out int callId);

        Assignment newAssignment = new(newAssignmentId, callId, volunteerId, s_dal!.Config.Clock, null, null);
        s_dal!.Assignment.Create(newAssignment);
        Console.WriteLine("Assignment added successfully.");
    }

    /// <summary>
    /// Displays the details of a specific volunteer by ID.
    /// </summary>
    /// <param name="id">The ID of the volunteer to view.</param>
    void ViewVolunteer(int id)
    {
        var volunteer = s_dal!.Volunteer.Read(id);
        if (volunteer != null)
            Console.WriteLine(volunteer);
        else
            Console.WriteLine("Volunteer not found.");
    }

    /// <summary>
    /// Displays the details of a specific call by ID.
    /// </summary>
    /// <param name="id">The ID of the call to view.</param>
    void ViewCall(int id)
    {
        var call = s_dal!.Call.Read(id);
        if (call != null)
            Console.WriteLine(call);
        else
            Console.WriteLine("Call not found.");
    }

    /// <summary>
    /// Displays the details of a specific assignment by ID.
    /// </summary>
    /// <param name="id">The ID of the assignment to view.</param>
    void ViewAssignment(int id)
    {
        var assignment = s_dal!.Assignment.Read(id);
        if (assignment != null)
            Console.WriteLine(assignment);
        else
            Console.WriteLine("Assignment not found.");
    }




    /// <summary>
    /// Displays and manages the entity-specific menu for performing CRUD operations and other actions
    /// such as viewing all entries, updating, or deleting entries of a specified entity type.
    /// </summary>
    /// <param name="entityType">The type of the entity (Volunteer, Call, Assignment) for which the menu operates.</param>

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
                          
                            List<Volunteer> volunteers = s_dal!.Volunteer.ReadAll();

                            foreach (var volunteer in volunteers)
                            {
                                ViewVolunteer(volunteer.Id);

                            }
                            break;
                        case EntityType.Call:

                            List<Call> calls = s_dal!.Call.ReadAll();
                            foreach (var call in calls)
                            {
                                ViewCall(call.Id);
                            }
                            break;

                        case EntityType.Assignment:

                            List<Assignment> assignments = s_dal!.Assignment.ReadAll();
                            foreach (var assignment in assignments)
                            {
                                ViewAssignment(assignment.Id);
                            }
                            break;
                    }
                    break;

                case EntityMenu.Update:

                    switch (entityType)
                        {
                            case EntityType.Volunteer:


                                // בקשה למשתמש להזין את ה-ID של המתנדב
                                Console.WriteLine("Enter volunteer ID:");
                                int.TryParse(Console.ReadLine(), out int volunteerId);

                                // קריאה לקריאת המתנדב לפי ה-ID
                                Volunteer? exist = s_dal!.Volunteer.Read(volunteerId);

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


                                s_dal!.Volunteer.Update(updatedVolunteer);

                                Console.WriteLine("Volunteer updated successfully.");

                                break;



                            case EntityType.Call:

                                // Prompt user to enter the ID of the call
                                Console.WriteLine("Enter call ID:");
                                int.TryParse(Console.ReadLine(), out int callId);

                                // Fetch the existing call by ID
                                Call? existingCall = s_dal!.Call.Read(callId);

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
                                s_dal!.Call.Update(updatedCall);
                                Console.WriteLine("Call updated successfully.");
                                break;


                            case EntityType.Assignment:

                                Console.WriteLine("Unable to update an Assignment.");

                                break;
                        }
 
                  
                    break;
                case EntityMenu.Delete:
                   
                        switch (entityType)
                        {
                            case EntityType.Volunteer:
                                Console.WriteLine("Enter the volunteer's ID to delete");
                                int.TryParse(Console.ReadLine(), out int volunteerId);
                                s_dal!.Volunteer.Delete(volunteerId);
                                Console.WriteLine("The volunteer deleted successfully");
                                break;
                            case EntityType.Call:
                                Console.WriteLine("Enter the call's ID to delete");
                                int.TryParse(Console.ReadLine(), out int callId);
                                s_dal!.Call.Delete(callId);
                                Console.WriteLine("The call deleted successfully");
                                break;
                            case EntityType.Assignment:
                                Console.WriteLine("Enter the assignment's ID to delete");
                                int.TryParse(Console.ReadLine(), out int assignmentId);
                                s_dal!.Assignment.Delete(assignmentId);
                                Console.WriteLine("The assignment deleted successfully");
                                break;
                        }
                    
                    break;
                case EntityMenu.DeleteAll:

                    switch (entityType)
                    {
                        case EntityType.Volunteer:
                           
                            s_dal!.Volunteer.DeleteAll();

                            break;
                        case EntityType.Call:
                           
                            s_dal!.Call.DeleteAll();
                
                            break;
                        case EntityType.Assignment:
                            
                            s_dal!.Assignment.DeleteAll();
                           
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
    /// <summary>
    /// Displays and manages the configuration menu, allowing the user to perform actions
    /// such as advancing the system clock, managing configuration variables, and resetting configurations.
    /// </summary>
    public void configMenu()
    {
        // A flag to control the menu loop.
        bool continueMenu = true;

        // Main loop for the Configuration Menu.
        while (continueMenu)
        {
            // Display the configuration menu options.
            Console.WriteLine("Select an option in the Configuration Menu:");
            Console.WriteLine("0 - Exit Configuration Menu");
            Console.WriteLine("1 - Advance system clock by one minute");
            Console.WriteLine("2 - Advance system clock by one hour");
            Console.WriteLine("3 - Advance system clock by one day");
            Console.WriteLine("4 - Display current system clock value");
            Console.WriteLine("5 - Set a new value for a configuration variable");
            Console.WriteLine("6 - Display current value of a configuration variable");
            Console.WriteLine("7 - Reset all configuration values");

            // Read and parse the user's choice.
            int.TryParse(Console.ReadLine(), out int choice);
            ConfigMenu selectedOption = (ConfigMenu)choice;

            // Execute the selected option.
            switch (selectedOption)
            {
                case ConfigMenu.Exit:
                    // Exit the configuration menu.
                    continueMenu = false;
                    break;

                case ConfigMenu.AdvanceClockByMinute:
                    // Advance the system clock by one minute.
                    s_dal!.Config.Clock = s_dal!.Config.Clock.AddMinutes(1);
                    Console.WriteLine("System clock advanced by one minute.");
                    break;

                case ConfigMenu.AdvanceClockByHour:
                    // Advance the system clock by one hour.
                    s_dal!.Config.Clock = s_dal!.Config.Clock.AddHours(1);
                    Console.WriteLine("System clock advanced by one hour.");
                    break;

                case ConfigMenu.AdvanceClockByDay:
                    // Advance the system clock by one day.
                    s_dal!.Config.Clock = s_dal!.Config.Clock.AddDays(1);
                    Console.WriteLine("System clock advanced by one day.");
                    break;

                case ConfigMenu.DisplayClock:
                    // Display the current value of the system clock.
                    Console.WriteLine($"Current system clock value: {s_dal!.Config.Clock}");
                    break;

                case ConfigMenu.SetConfigVariable:
                    // Set a new value for a specified configuration variable.
                    Console.Write("Enter the name of the configuration variable to set: ");
                    string? variableName = Console.ReadLine();
                    Console.Write("Enter the new value for the configuration variable: ");
                    string newValue = Console.ReadLine()!;

                    if (!string.IsNullOrWhiteSpace(variableName) && !string.IsNullOrWhiteSpace(newValue))
                    {
                        s_dal!.Config.SetConfigValue(variableName, newValue);
                        Console.WriteLine($"Configuration variable '{variableName}' set to '{newValue}'.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Variable name and value cannot be empty.");
                    }
                    break;

                case ConfigMenu.DisplayConfigVariable:
                    // Display the value of a specified configuration variable.
                    Console.Write("Enter the name of the configuration variable to display: ");
                    string configVarName = Console.ReadLine()!;

                    if (!string.IsNullOrWhiteSpace(configVarName))
                    {
                        string value = s_dal!.Config.GetConfigValue(configVarName);
                        Console.WriteLine($"Value of configuration variable '{configVarName}': {value}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Variable name cannot be empty.");
                    }
                    break;

                case ConfigMenu.ResetConfig:
                    // Reset all configuration variables to their default values.
                    s_dal!.Config.Reset();
                    Console.WriteLine("All configuration variables have been reset.");
                    break;

                default:
                    // Handle invalid menu options.
                    Console.WriteLine("Invalid option. Please select a valid option.");
                    break;
            }
        }

        // Indicate that the configuration menu has been exited.
        Console.WriteLine("Exited Configuration Menu.");
    }


    /// <summary>
    /// Displays and manages the main menu, where the user can perform various system actions
    /// such as navigating to sub-menus, viewing data, or resetting configurations.
    /// </summary>
    public void mainMenu()
    {
        // A flag to control the menu loop.
        bool continueMenu = true;

        // Main loop for the Main Menu.
        while (continueMenu)
        {
            // Display the main menu options.
            Console.WriteLine("Select a main menu option (0-Exit, 1-Volunteer, 2-Call, 3-Assignment," +
                              " 4-Data Init, 5-View All, 6-Config, 7-Reset):");

            // Read and parse the user's choice.
            int.TryParse(Console.ReadLine(), out int choice);
            MainMenu selectedMenu = (MainMenu)choice;

            // Execute the selected option.
            switch (selectedMenu)
            {
                case MainMenu.ExitMainMenu:
                    // Exit the main menu.
                    continueMenu = false;
                    break;

                case MainMenu.VolunteerMenu:
                    // Navigate to the Volunteer Menu.
                    entityMenu(EntityType.Volunteer);
                    break;

                case MainMenu.CallMenu:
                    // Navigate to the Call Menu.
                    entityMenu(EntityType.Call);
                    break;

                case MainMenu.AssignmentMenu:
                    // Navigate to the Assignment Menu.
                    entityMenu(EntityType.Assignment);
                    break;

                case MainMenu.DataInitialization:
                    // Initialize the data.
                    Console.WriteLine("Initializing data.");
                    Initialization.Do(s_dal);
                    break;

                case MainMenu.ViewAllData:
                    // View all data of Volunteers, Calls, and Assignments.
                    List<Volunteer> volunteers = s_dal!.Volunteer.ReadAll();
                    foreach (var volunteer in volunteers)
                    {
                        Console.WriteLine($"{volunteer}");
                    }

                    List<Call> calls = s_dal!.Call!.ReadAll();
                    foreach (var call in calls)
                    {
                        Console.WriteLine($"{call}");
                    }

                    List<Assignment> assignments = s_dal!.Assignment.ReadAll();
                    foreach (var assignment in assignments)
                    {
                        Console.WriteLine($"{assignment}");
                    }

                    Console.WriteLine("Viewing all data.");
                    break;

                case MainMenu.ConfigMenu:
                    // Navigate to the Configuration Menu.
                    configMenu();
                    break;

                case MainMenu.Database_and_configuration_reset:
                    // Reset the database and configuration.
                    Console.WriteLine("Resetting database and configuration.");

                    // Delete all records in the database tables.
                    s_dal!.Assignment.DeleteAll(); // Delete all assignments
                    s_dal.Call.DeleteAll(); // Delete all calls
                    s_dal!.Volunteer.DeleteAll(); // Delete all volunteers
                                                 // Optionally reset the configuration values.
                                                 // s_dalConfig!.ResetConfig();
                    break;

                default:
                    // Handle invalid menu options.
                    Console.WriteLine("Invalid menu option.");
                    break;
            }
        }

        // Indicate that the program has exited.
        Console.WriteLine("Program has exited.");
    }



    /// <summary>
    /// The entry point for the Dal Management System application.
    /// Initializes the system and handles any unexpected errors that may occur during execution.
    /// </summary>
    static void Main(string[] args)
    {
        try
        {
            // Display a welcome message to the user.
            Console.WriteLine("Welcome to the Dal Management System!");

            // Create an instance of the Program class to access the main menu.
            Program program = new Program();

            // Call the main menu method to start the program.
            program.mainMenu();
        }
        catch (Exception ex)
        {
            // If an unexpected error occurs, display an error message.
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
        finally
        {
            // This block is executed regardless of whether an exception occurred or not.
            // Display a farewell message to the user.
            Console.WriteLine("Thank you for using the Dal Management System. Goodbye!");
        }
    }




}


