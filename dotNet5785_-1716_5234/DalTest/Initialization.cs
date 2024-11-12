
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
        string[] names = {
            "Dani Levy", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof",
            "Tamar Avni", "Noam Baruch", "Yael Tzur", "Yonatan Gilad", "Lior Hadad", "Maya Zohar",
            "Idan Sasson", "Avigail Shani", "Eden Bar", "Nadav Shavit", "Gal Mor", "Adi Nir",
            "Tal Shaked", "Ofek Azulai"
        };

        string[] emails = {
            "dani@example.com", "eli@example.com", "yair@example.com", "ariela@example.com", "dina@example.com", "shira@example.com",
            "tamar@example.com", "noam@example.com", "yael@example.com", "yonatan@example.com", "lior@example.com", "maya@example.com",
            "idan@example.com", "avigail@example.com", "eden@example.com", "nadav@example.com", "gal@example.com", "adi@example.com",
            "tal@example.com", "ofek@example.com"
        };

        string[] addresses = {
            "Tel Aviv, Rothschild Blvd 15", "Jerusalem, Jaffa St 23", "Haifa, Ben Gurion Blvd 33",
            "Beer Sheva, Rager Blvd 12", "Eilat, Derech Yotam 4", "Rishon LeZion, Herzl St 22",
            "Netanya, Independence Square 9", "Ashdod, HaShalom St 17", "Herzliya, Ben Yehuda St 10",
            "Holon, Eilat St 5", "Kfar Saba, Weizmann St 8", "Bat Yam, Begin Blvd 14",
            "Ra'anana, HaPark St 7", "Modiin, Ironi Dalet 3", "Beit Shemesh, Savyon Blvd 11",
            "Acre, Old City 2", "Nahariya, Ga'aton Blvd 6", "Tiberias, HaGalil St 18",
            "Safed, HaAri St 4", "Kiryat Shmona, HaBanim St 12"
        };

        double[] latitudes = {
            32.0655, 31.7833, 32.8191, 31.2529, 29.5581, 31.9702,
            32.3294, 31.8066, 32.1643, 32.0153, 32.1750, 32.0248,
            32.1839, 31.8961, 31.7498, 32.9263, 33.0068, 32.7922,
            32.9656, 33.2074
        };

        double[] longitudes = {
            34.7754, 35.2170, 34.9885, 34.7903, 34.9482, 34.8037,
            34.8500, 34.6403, 34.8431, 34.7805, 34.9063, 34.7474,
            34.8708, 35.0094, 34.9934, 35.0713, 35.0948, 35.5372,
            35.4950, 35.5724
        };


        // יצירת מתנדבים לדוגמה
        for (int i = 0; i < 20; i++)
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
            id = s_dalConfig!.NextAssignmentId;


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

             id = s_dalConfig!.NextCallId;
            // בדיקה אם המזהה כבר קיים

            // בחירת נתונים רנדומליים לכתובת ולתיאו
            string description = descriptions[s_rand.Next(descriptions.Length)];
            int index = s_rand.Next(addresses.Length);

            // שליפת כתובת עם קואורדינטות מסונכרנות
            string address = addresses[index];
            double latitude = latitudes[index];
            double longitude = longitudes[index];


            // קבלת זמן פתיחה באמצעות שעון המערכת
            //DateTime openingTime = s_dalConfig!.Clock;
            DateTime start = new DateTime(s_dalConfig!.Clock.Year - 2, 1, 1); //stage 1
            int range = (s_dalConfig.Clock - start).Days; //stage 1
            DateTime openingTime = start.AddDays(s_rand.Next(range));


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

