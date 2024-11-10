

namespace DalTest;
using DalApi;
using DO;
public static class Initialization
{
    private static IVolunteer? s_dalVolunteer;
    private static ICall? s_dalCall;
    private static IAssignment? s_dalAssignment;
    private static IConfig? s_dalConfig;

    private static readonly Random s_rand = new();

    /// <summary>
    /// Creates a list of volunteers with random attributes and adds them to the data source.
    /// </summary>
    private static void create_volunteer()
    {
        string[] names = { "Dani Levy", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof" };
        string[] emails = { "dani@example.com", "eli@example.com", "yair@example.com", "ariela@example.com", "dina@example.com", "shira@example.com" };
        string[] addresses = { "Tel Aviv", "Jerusalem", "Haifa", "Ashdod", "Beersheba" };

        // יצירת 5 מתנדבים לדוגמה
        for (int i = 0; i < 5; i++)
        {
            int id;

            // יצירת מזהה ייחודי עבור המתנדב
            do
            {
                id = s_rand.Next(200000000, 400000000); // הגרלת ת"ז בטווח 200000000-400000000
            }
            while (s_dalVolunteer!.Read(id) != null);

            // בחירת נתונים רנדומליים עבור שם, כתובת, ואימייל
            string name = names[s_rand.Next(names.Length)];
            string email = emails[s_rand.Next(emails.Length)];
            string address = addresses[s_rand.Next(addresses.Length)];

            // יצירת מספר טלפון רנדומלי
            string phoneNumber = $"05{s_rand.Next(0, 10)}-{s_rand.Next(1000000, 9999999)}";

            // יצירת מרחק מקסימלי רנדומלי
            double maxDistance = s_rand.Next(1, 50); // למשל 1-50 ק"מ

            // יצירת אובייקט `Volunteer` חדש
            Volunteer newVolunteer = new(id, name, phoneNumber, email, null, address, null, maxDistance, true);

            // הוספת המתנדב החדש לרשימה באמצעות מתודת ה-CRUD המתאימה
            s_dalVolunteer!.Create(newVolunteer);
        }
    }

    /// <summary>
    /// Creates a list of assignments linking volunteers to calls.
    /// </summary>
    private static void create_assignment()
    {
        // יצירת 5 משימות לדוגמה
        for (int i = 0; i < 5; i++)
        {
            int id;

            // יצירת מזהה ייחודי עבור המשימה
            do
            {
                id = s_rand.Next(1000, 9999); // מזהה רנדומלי
            }
            while (s_dalAssignment!.Read(id) != null);

            // בחירת קריאה ומתנדב רנדומליים מתוך הקריאות והמתנדבים הקיימים
            var calls = s_dalCall!.ReadAll();
            var volunteers = s_dalVolunteer!.ReadAll();

            if (calls.Count == 0 || volunteers.Count == 0)
                throw new InvalidOperationException("There are no calls or volunteers available to create a task\n");

            int callId = calls[s_rand.Next(calls.Count)].Id;
            int volunteerId = volunteers[s_rand.Next(volunteers.Count)].Id;

            // יצירת אובייקט `Assignment` חדש
            Assignment newAssignment = new(id, callId, volunteerId);

            // הוספת המשימה החדשה לרשימה באמצעות מתודת ה-CRUD המתאימה
            s_dalAssignment!.Create(newAssignment);
        }
}

private static void create_call()
    {
        // מערכים לדוגמאות עבור כתובות ותיאורים
        string[] addresses = { "123 Main St", "456 Maple Ave", "789 Oak Dr", "101 Pine Rd", "202 Birch Blvd" };
        string[] descriptions = { "Emergency", "Routine Check", "Suspicious Activity", "Medical Assistance", "Noise Complaint" };

        // יצירת 5 קריאות לדוגמה
        for (int i = 0; i < 5; i++)
        {
            int id;

            // יצירת מזהה ייחודי עבור הקריאה
            do
            {
                id = s_rand.Next(1000,9999 ); // יצירת מזהה רנדומלי בטווח מסוים
            }
            while (s_dalCall!.Read(id) != null); // בדיקה אם המזהה כבר קיים

            // בחירת נתונים רנדומליים לכתובת ולתיאור
            string address = addresses[s_rand.Next(addresses.Length)];
            string description = descriptions[s_rand.Next(descriptions.Length)];

            // יצירת קואורדינטות רנדומליות (לדוגמה: בישראל)
            double latitude = s_rand.NextDouble() * (33.3 - 29.5) + 29.5;   // טווח קווי הרוחב של ישראל
            double longitude = s_rand.NextDouble() * (35.6 - 34.2) + 34.2; // טווח קווי האורך של ישראל

            // קבלת זמן פתיחה באמצעות שעון המערכת
            DateTime openingTime = s_dalConfig!.Clock;

            // יצירת אובייקט `Call` חדש
            Call newCall = new(id, address, description, latitude, longitude)
            {
                // עדכון זמן פתיחה
                openingTime = openingTime

            }; 

            // הוספת הקריאה החדשה לרשימה באמצעות מתודת ה-CRUD המתאימה
            s_dalCall!.Create(newCall);
        }
    }
    public static void Do(IVolunteer? dalVolunteer, ICall? dalcall, IAssignment? dalAssignment, IConfig? dalConfig) 
    {
        s_dalVolunteer = dalVolunteer ?? throw new NullReferenceException("DAL object can not be null!"); //stage 1
        s_dalAssignment = dalAssignment ?? throw new NullReferenceException("DAL object can not be null!");
        s_dalCall = dalcall ?? throw new NullReferenceException("DAL object can not be null!");
        s_dalConfig = dalConfig ?? throw new NullReferenceException("DAL object can not be null!");

        Console.WriteLine("Reset Configuration values and List values...");
        s_dalConfig.Reset(); //stage 1
        s_dalVolunteer.DeleteAll(); //stage 1
        s_dalAssignment.DeleteAll();
        s_dalCall.DeleteAll();
        Console.WriteLine("Initializing Students list ...");
        create_volunteer();
        create_assignment();
        create_call();
    }


}
