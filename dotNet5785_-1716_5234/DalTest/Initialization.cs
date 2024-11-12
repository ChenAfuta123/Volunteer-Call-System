

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
        string[] addresses = {  "Tel Aviv, Rothschild Blvd 15",  "Jerusalem, Jaffa St 23", "Haifa, Ben Gurion Blvd 33", "Beer Sheva, Rager Blvd 12", "Eilat, Derech Yotam 4"};
        double[] latitudes = { 32.0655,31.7833, 32.8191,31.2529, 29.5581 };

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
            int index = s_rand.Next(addresses.Length);
            string address = addresses[index];
            double latitude = latitudes[index];

            // יצירת מספר טלפון רנדומלי
            string phoneNumber = $"05{s_rand.Next(0, 10)}-{s_rand.Next(1000000, 9999999)}";

            // יצירת מרחק מקסימלי רנדומלי
            double maxDistance = s_rand.Next(1, 50); // למשל 1-50 ק"מ

            // יצירת אובייקט `Volunteer` חדש
            Volunteer newVolunteer = new(id, name, phoneNumber, email, null, address, latitude, maxDistance, true);

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
        string[] addresses = { "Ashdod, Herzl St 30", "Netanya, Sderot Ben Gurion 18", "Rishon LeZion, Rothschild St 45", "Holon, HaHistadrut St 12", "Bat Yam, Balfour St 50" };
        string[] descriptions = { "Emergency", "Routine Check", "Suspicious Activity", "Medical Assistance", "Noise Complaint" };
        double[] longitudes = { 34.6403, 34.8600, 34.8044, 34.7722, 34.7454 };
        double[] latitudes = { 31.8014, 32.3215, 31.9701, 32.0153, 32.0161 };


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

            // בחירת נתונים רנדומליים לכתובת ולתיאו
            string description = descriptions[s_rand.Next(descriptions.Length)];
            int index = s_rand.Next(addresses.Length);

            // שליפת כתובת עם קואורדינטות מסונכרנות
            string address = addresses[index];
            double latitude = latitudes[index];
            double longitude = longitudes[index];


            // קבלת זמן פתיחה באמצעות שעון המערכת
            DateTime openingTime = s_dalConfig!.Clock;

            // יצירת אובייקט `Call` חדש
            Call newCall = new(id, address, description, latitude, longitude)
            {
                // עדכון זמן פתיחה
                //openingTime = openingTime
                

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
        Console.WriteLine("Initializing Volunteers list ...");
        create_volunteer();
        Console.WriteLine("Initializing Calls list ...");
        create_call();
        Console.WriteLine("Initializing Assignment list ...");
        create_assignment();
      
    }


}
