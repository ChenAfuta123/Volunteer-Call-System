using Dal;
using DalApi;
using DalList;
using DO;
using static DalTest.Program;

namespace DalTest;

internal class Program
{
    private static IAssignment? s_dalAssignment = new AssignmentImplementation(); //stage 1
    private static ICall? s_dalCall = new CallImplementation(); //stage 1
    private static IVolunteer? s_dalVolunteer = new VolunteerImplementation(); //stage 1
    private static IConfig? s_dalConfig = new ConfigImplementation(); //stage 1
    public enum MainMenu{
        ExitMainMenu=0, 
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
    void CreateVolunteer()
    {
        //...
    }
    void CreateCall()
    {            
        //...
    }
    void CreateAssignment() 
    {

        //...

    }
    void ViewVolunteer()
    {
        //...
    }


    void  ViewCall()
    {
        //...
    }


    void  ViewAssignment()
    {
        //...
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
                    break;
                case EntityMenu.View:
                    switch (entityType)
                    {
                        case EntityType.Volunteer:
                            ViewVolunteer();
                            break;
                        case EntityType.Call:
                            ViewCall();
                            break;
                        case EntityType.Assignment:
                            ViewAssignment();
                            break;
                    }
                    break;
                    
                case EntityMenu.ViewAll:
                   
                    switch (entityType)
                    {
                        case EntityType.Volunteer:
                            //...
                            break;
                        case EntityType.Call:
                            //...
                            break;
                        case EntityType.Assignment:
                            //...
                            break;
                    }
                    break;
                    
                case EntityMenu.Update:
                    switch (entityType)
                    {
                        case EntityType.Volunteer:
                            //...
                            break;
                        case EntityType.Call:
                            //...
                            break;
                        case EntityType.Assignment:
                            //...
                            break;
                    }
                    break;
                case EntityMenu.Delete:
                    switch (entityType)
                    {
                        case EntityType.Volunteer:
                            //...
                            break;
                        case EntityType.Call:
                            //...
                            break;
                        case EntityType.Assignment:
                            //...
                            break;
                    }
                    break;
                case EntityMenu.DeleteAll:
                    //...
                    Console.WriteLine($"Deleting all {entityType} entries.");
                    break;
                default:
                    Console.WriteLine("Invalid menu option.");
                    break;
            }
        }

        Console.WriteLine($"Exited {entityType} menu.");
    }
   void ConfigMenu()
    {
        //...
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
                    //Initialization.Do(s_dalAssignment, s_dalCall, s_dalVolunteer, s_dalConfig);
                    break;
                case MainMenu.ViewAllData:
                    //...
                    Console.WriteLine("Viewing all data.");
                    break;
                case MainMenu.ConfigMenu:
                    ConfigMenu();
                    break;
                case MainMenu.Database_and_configuration_reset:
                    Console.WriteLine("Resetting database and configuration.");
                   
                    //s_dalAssignment.DeleteAll(); //stage 1
                    //s_dalCall.DeleteAll();
                    //s_dalVolunteer.DeleteAll();
                    //s_dalConfig.ResetConfig(); 


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
        Console.WriteLine("Program has exited.");
        //    mainMenu();
        //
    }
       





    //try
    //{
    //    // קריאה למתודה Delete (החריגה תיזרק אם לא נמצא אובייקט)
    //    /*someClassInstance.Delete(123); */ // 123 הוא ה-ID שאתה רוצה למחוק
    //}
    //catch (Exception ex)
    //{
    //    // טיפול בחריגה: להדפיס את ההודעה אם יש בעיה
    //    Console.WriteLine($"{ex.Message}");
    //}


