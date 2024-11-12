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
        // שמות לדוגמה עבור מתנדבים
        string[] names = { "Dani Levy", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof", "Lior Mazor", "Gal Ben David", "Nir Avrahami", "Nofar Shani", "Tomer Klein", "Avi Mordechai", "Or Eliyahu", "Roni Levi", "Noa Shahar", "Dana Alon", "Itay Cohen", "Yafit Azulay", "Moran Levy", "Ofir Segal" };

        // יצירת מתנדבים, כאשר הראשון מוגדר כמנהל
        for (int i = 0; i < 20; i++)
        {
            int id;

            // יצירת מזהה ייחודי עבור המתנדב
            do
            {
                id = s_rand.Next(200000000, 400000000);
            }
            while (s_dalVolunteer!.Read(id) != null);

            // בחירת נתונים עבור המתנדב
            string name = names[i % names.Length];
            string email = $"{name.ToLower().Replace(" ", ".")}@example.com";
            string phoneNumber = $"05{s_rand.Next(0, 10)}-{s_rand.Next(1000000, 9999999)}";

            // כתובת ונתוני Geocoding רנדומליים
            string address = $"City {s_rand.Next(1, 10)}, Street {s_rand.Next(1, 50)}";
            double latitude = s_rand.NextDouble() * 2 + 31;  // תחום קווי רוחב בישראל
            double longitude = s_rand.NextDouble() * 2 + 34; // תחום קווי אורך בישראל
            double maxDistance = s_rand.Next(1, 50);

            // הגדרת המתנדב הראשון כמנהל
            bool isAdmin = i == 0;

            // יצירת אובייקט Volunteer חדש
            Volunteer newVolunteer = new(id, name, phoneNumber, email, isAdmin, address, latitude, longitude, maxDistance, true);

            s_dalVolunteer!.Create(newVolunteer);
        }
    }

    /// <summary>
    /// Creates a list of calls with random attributes and adds them to the data source.
    /// </summary>
    private static void create_call()
    {
        string[] descriptions = { "Medical Assistance", "Fire Alert", "Roadside Assistance", "Noise Complaint", "Suspicious Activity" };

        // יצירת 55 קריאות
        for (int i = 0; i < 55; i++)
        {
            int id;

            // יצירת מזהה ייחודי עבור הקריאה
            do
            {
                id = s_rand.Next(1000, 9999);
            }
            while (s_dalCall!.Read(id) != null);

            // פרטי הקריאה
            string description = descriptions[s_rand.Next(descriptions.Length)];
            string address = $"City {s_rand.Next(1, 10)}, Street {s_rand.Next(1, 50)}";
            double latitude = s_rand.NextDouble() * 2 + 31;
            double longitude = s_rand.NextDouble() * 2 + 34;

            // זמן פתיחה
            DateTime openingTime = s_dalConfig!.Clock.AddMinutes(-s_rand.Next(10, 1000));

            // זמן סיום עם סיכוי להשאר null
            DateTime? closingTime = i < 15 ? null : openingTime.AddMinutes(s_rand.Next(30, 200));
            bool isExpired = closingTime != null && closingTime < DateTime.Now && i < 5;

            // יצירת אובייקט Call חדש
            Call newCall = new(id, address, description, latitude, longitude, openingTime, closingTime, isExpired);

            s_dalCall!.Create(newCall);
        }
    }

    /// <summary>
    /// Creates assignments linking volunteers to calls.
    /// </summary>
    private static void create_assignment()
    {
        var calls = s_dalCall!.ReadAll();
        var volunteers = s_dalVolunteer!.ReadAll();

        // יצירת 50 הקצאות
        for (int i = 0; i < 50; i++)
        {
            int id;

            // יצירת מזהה ייחודי עבור המשימה
            do
            {
                id = s_rand.Next(1000, 9999);
            }
            while (s_dalAssignment!.Read(id) != null);

            int callId = calls[s_rand.Next(calls.Count)].Id;
            int volunteerId = volunteers[s_rand.Next(volunteers.Count)].Id;

            DateTime openingTime = calls.Find(c => c.Id == callId)!.OpeningTime;
            DateTime entryTime = openingTime.AddMinutes(s_rand.Next(10, 60));
            DateTime? exitTime = entryTime.AddMinutes(s_rand.Next(20, 180));

            // סטטוס סיום אקראי
            string endStatus = exitTime != null && exitTime < DateTime.Now ? "Completed" : "Pending";

            // יצירת אובייקט Assignment חדש
            Assignment newAssignment = new(id, callId, volunteerId, entryTime, exitTime, endStatus);

            s_dalAssignment!.Create(newAssignment);
        }
    }

    public static void Do(IVolunteer? dalVolunteer, ICall? dalCall, IAssignment? dalAssignment, IConfig? dalConfig)
    {
        s_dalVolunteer = dalVolunteer ?? throw new NullReferenceException("DAL object cannot be null!");
        s_dalAssignment = dalAssignment ?? throw new NullReferenceException("DAL object cannot be null!");
        s_dalCall = dalCall ?? throw new NullReferenceException("DAL object cannot be null!");
        s_dalConfig = dalConfig ?? throw new NullReferenceException("DAL object cannot be null!");

        Console.WriteLine("Reset Configuration values and List values...");
        s_dalConfig.Reset();
        s_dalVolunteer.DeleteAll();
        s_dalAssignment.DeleteAll();
        s_dalCall.DeleteAll();

        Console.WriteLine("Initializing Volunteers list...");
        create_volunteer();

        Console.WriteLine("Initializing Calls list...");
        create_call();

        Console.WriteLine("Initializing Assignment list...");
        create_assignment();
    }
}
