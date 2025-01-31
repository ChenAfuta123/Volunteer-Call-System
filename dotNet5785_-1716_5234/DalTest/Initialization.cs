namespace DalTest;
using DalApi;
using DO;

public static class Initialization
{

    private static IDal? s_dal;

    private static readonly Random s_rand = new();

    /// <summary>                                           
    /// Generates a random date within a given range.
    /// </summary>
    /// <param name="startDate">The start date of the range.</param>
    /// <param name="endDate">The end date of the range.</param>
    /// <returns>A randomly selected date within the specified range.</returns>
    static DateTime GetRandomDate(DateTime startDate, DateTime endDate)
    {
        Random random = new Random();
        int range = (endDate - startDate).Days;
        int randomDays = random.Next(range);
        return startDate.AddDays(randomDays);
    }

    /// <summary>
    /// Creates a list of volunteers with random attributes and adds them to the data source.
    /// </summary>
    private static void create_volunteer()
    {
        int[] ids =  {
            200123456, 201234567, 202345678, 203456789, 204567890,
            205678901, 206789012, 207890123, 208901234, 209012345,
            210123456, 211234567, 212345678, 213456789, 214567890,
            215678901, 216789012, 217890123, 218901234, 219012345
        };

        string[] names = {
            "ChenAfuta", "HallelOchana", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof",
            "Tamar Avni", "Noam Baruch", "Yael Tzur", "Yonatan Gilad", "Lior Hadad", "Maya Zohar",
            "Idan Sasson", "Avigail Shani", "Eden Bar", "Nadav Shavit", "Gal Mor", "Adi Nir",
            "Tal Shaked", "Ofek Azulai"
        };

        string[] emails = {
            "ChenAfuta@gmail.com", "HallelOchana@gmail.com", "yair@example.com", "ariela@example.com", "dina@example.com", "shira@example.com",
            "tamar@example.com", "noam@example.com", "yael@example.com", "yonatan@example.com", "lior@example.com", "maya@example.com",
            "idan@example.com", "avigail@example.com", "eden@example.com", "nadav@example.com", "gal@example.com", "adi@example.com",
            "tal@example.com", "ofek@example.com"
        };

        string[] addresses = {
            "Kfar Saba, Weizmann St 30", "Sderot, HaDekel St 9", "Lod, HaHistadrut St 20",
            "Beer Sheva, Rager Blvd 12", "Mazkeret Batya, HaDekel St 15", "Rishon LeZion, Herzl St 22",
            "Petach Tikva, HaHistadrut St 18", "Lod, HaHistadrut St 28", "Herzliya, Ben Yehuda St 10",
            "Holon, Eilat St 5", "Kfar Saba, Weizmann St 8", "Bat Yam, Begin Blvd 14",
            "Ra'anana, HaPark St 7", "Ramat Gan, Abba Hillel Silver Rd 95", "Jerusalem, Hertzel St 20",
            "Kiryat Shmona, HaBanim St 10", "Petach Tikva, HaHistadrut St 18", "Tiberias, HaGalil St 18",
            "Safed, HaAri St 4", "Kiryat Shmona, HaBanim St 12"
        };

        double[] latitudes = {
    32.175500, 31.523400, 31.951400,
    31.252200, 31.853500, 31.967100,
    32.0858939, 31.951700, 32.162200,
    32.013900, 32.178200, 32.016700,
    32.183300, 32.090000, 31.775800,
    33.207700, 32.086094, 32.793500,
    32.966800, 33.208500
};

        double[] longitudes = {
    34.905500, 34.593900, 34.895200,
    34.794800, 34.836800, 34.807700,
    34.8838329, 34.895400, 34.843800,
    34.771700, 34.903800, 34.748600,
    34.870800, 34.804100, 35.179800,
    35.570400, 34.886269, 35.539700,
    35.495000, 35.570000
};


        for (int i = 0; i < 20; i++)
        {
            int id = ids[i];
            string name = names[i];
            string email = emails[i];
            string address = addresses[i];
            double latitude = latitudes[i];
            double longitude = longitudes[i];
            string password;
            if (i == 0)
                password = BCrypt.Net.BCrypt.HashPassword("AB123");
            if (i == 1)
                password = BCrypt.Net.BCrypt.HashPassword("CD123");
            if (i == 2)
                password = BCrypt.Net.BCrypt.HashPassword("EF123");

            else
                password = BCrypt.Net.BCrypt.HashPassword($"{(char)s_rand.Next('A', 'Z' + 1)}{(char)s_rand.Next('A', 'Z' + 1)}{s_rand.Next(100, 999)}");
            string phoneNumber = $"05{s_rand.Next(0, 10)}{s_rand.Next(1000000, 9999999)}";
            double maxDistance = s_rand.Next(10, 150);
            Role role = (i == 0) ? Role.manager : Role.volunteer;
            DistanceType distanceType =  (DistanceType)(i % Enum.GetValues(typeof(DistanceType)).Length);
            /// <summary>
            /// Creates a new Volunteer object.
            /// </summary>

            Volunteer newVolunteer = new(id, distanceType, role, name, phoneNumber, email, password, address, latitude, longitude, maxDistance, true);

            s_dal!.Volunteer.Create(newVolunteer);
        }
    }

    /// <summary>
    /// Creates a list of emergency calls with random attributes.
    /// </summary>
    private static void create_call()
    {
        string[] addresses = {
    "Tel Aviv, Dizengoff St 50", "Jerusalem, Jaffa St 10", "Rishon LeZion, Herzl St 28", "Tel Aviv, Dizengoff St 50", "Bat Yam, Ben Gurion Blvd 15",
    "Petah Tikva, Em Hamoshavot Rd 45", "Netanya, Herzl St 12", "Safed, HaAri St 20", "Jerusalem, Jaffa St 20", "Holon, Sokolov St 1",
    "Bat Yam, Ben Gurion Blvd 15","Rishon LeZion, Herzl St 28", "Ramat Gan, Abba Hillel Silver Rd 100", "Herzliya, Ben Yehuda St 55",
    "Kfar Saba, Weizmann St 28", "Herzliya, Ben Yehuda St 20", "TelAviv, Yigal Alon St 50", "Yavne, HaGefen St 15", "Nahariya, Sokolov St 18",
    "Safed, HaAri St 15", "Karmiel, HaGalil St 7", "Tiberias, HaGalil St 20", "Tel Aviv, Dizengoff St 48", "Jerusalem, Hertzel St 10",
    "Kfar Saba, Weizmann St 20", "Tiberias, HaBanim St 15", "Sderot, HaDekel St 3", "Ramat HaSharon, Sokolov St 38", "Ramat HaSharon, Sokolov St 40",
    "Holon, Sokolov St 10", "Even Yehuda, HaShaked St 17", "Even Yehuda, HaShaked St 13", "Gedera, HaZayit St 12", "Yavne, HaGefen St 4",
    "Kiryat Shmona, HaBanim St 14", "Petah Tikva, Em Hamoshavot Rd 40", "Karmiel, HaGalil St 10", "Bet Shemesh, HaGefen St 20",
    "Rosh HaAyin, HaHagana St 3", "Rosh HaAyin, HaHagana St 1", "Jerusalem, Hillel St 20", "Lod, HaHistadrut St 30",
    "Ramla, Herzl St 27", "Bet Shemesh, HaGalil St 10", "Bet Shemesh, HaGalil St 20", "Ma'alot Tarshiha, HaTeena St 12", "Ma'alot Tarshiha, HaTeena St 19",
     "Tel Mond, HaGoren St 10", "Tel Mond, HaGoren St 15", "Mazkeret Batya, HaDekel St 3"
};

    double[] latitudes = {
    32.080480, 31.783180, 31.964600, 32.080480, 32.017136,
    32.091800, 32.332900, 32.968000, 31.783600, 32.015200,
    32.017136, 31.964600, 32.090800, 32.165000, 32.175000,
    32.164000, 32.069000, 31.876000, 33.005000, 32.968000,
    32.917000, 32.792000, 32.080400,31.786207,
    32.174000, 32.792000, 31.522000, 32.147000, 32.147000,
    32.015500, 32.270000, 32.270000, 31.814000, 31.876000,
    33.209000, 32.091500, 32.917000, 31.749000, 32.095000,
    32.095000, 31.780000, 31.951000, 31.929000, 31.749000,
    31.749000, 33.016000, 33.016000, 32.249000, 32.249000,
    31.853000
    };
        double[] longitudes = {
    34.775610, 35.219360, 34.804400, 34.775610, 34.745441,
    34.887500, 34.859900, 35.494000, 35.219800, 34.774800,
    34.745441, 34.804400, 34.804000, 34.842000, 34.906000,
    34.841000, 34.794000, 34.738000, 35.094000, 35.494000,
    35.305000, 35.540000, 34.775500, 35.1984677,
    34.905000, 35.537000, 34.595000, 34.841000, 34.841000,
    34.774900, 34.888000, 34.888000, 34.779000, 34.737000,
    35.570000, 34.887200, 35.305000, 34.987000, 34.950000,
    34.950000, 35.220000, 34.895000, 34.869000, 34.987000,
    34.987000, 35.275000, 35.275000, 34.918000, 34.918000,
    34.836000
    };

        string[] descriptions =
         {

            "Providing hot meals to evacuees in shelters, ensuring nutritional needs are met.",
            "Delivering snacks, such as pastries and cakes, to brighten the evacuees' day.",
            "Distributing bottled water to evacuees to ensure hydration in shelters.",
            "Providing ready-to-eat dry food packs for evacuees in temporary shelters.",
            "Distributing blankets, sweaters, and socks to evacuees for warmth.",
            "Delivering tables and chairs to temporary housing to improve comfort.",
            "Transporting food supplies to remote shelter locations for easier access.",
            "Distributing thermoses with hot tea or coffee to evacuees for warmth.",
            "Bringing groceries and hygiene products to shelters for immediate use.",
            "Providing nutrition packages that cater to specific dietary needs (e.g., gluten-free).",


            "Helping evacuees find safe and comfortable temporary accommodation.",
            "Assisting with the relocation of evacuees' personal belongings to new housing.",
            "Coordinating transport for evacuees from temporary shelters to more secure locations.",
            "Arranging temporary shelters for families, ensuring privacy and security.",
            "Providing support in finding accessible housing for evacuees with disabilities.",
            "Helping evacuees set up basic living spaces in temporary shelters.",
            "Organizing transportation for evacuees to move to safer regions within the country.",
            "Setting up community kitchens in temporary housing to foster a sense of normalcy.",
            "Helping evacuees navigate local housing systems and rental assistance.",
            "Providing relocation assistance for evacuees needing to move to a permanent home.",


            "Providing trauma counseling services to evacuees affected by the war.",
            "Organizing recreational activities such as games and crafts for children in shelters.",
            "Offering group therapy sessions to evacuees dealing with emotional stress.",
            "Connecting evacuees with support groups for shared experiences and healing.",
            "Facilitating discussions and workshops about mental health coping strategies.",
            "Providing social workers and counselors on-site in shelters to assist evacuees.",
            "Setting up quiet spaces for evacuees to rest and de-stress after traumatic experiences.",
            "Bringing in volunteer groups to engage evacuees in creative arts and self-expression.",
            "Coordinating with local religious leaders to offer spiritual support to evacuees.",
            "Arranging buddy systems in shelters to ensure evacuees have someone to rely on.",

            "Coordinating medical teams to provide basic health services in evacuation centers.",
            "Delivering essential medications to evacuees who have lost access to pharmacies.",
            "Arranging transportation for evacuees needing urgent medical care.",
            "Providing emergency first aid and triage for injured evacuees in shelters.",
            "Facilitating doctor consultations for evacuees with chronic health conditions.",
            "Ensuring the timely delivery of prescription medications to evacuees in need.",
            "Setting up mobile health clinics in temporary shelters for on-site medical assistance.",
            "Providing basic medical supplies such as bandages and pain relief to evacuees.",
            "Coordinating with local pharmacies to ensure medical needs are met for evacuees.",
            "Offering mental health consultations in shelters for those affected by the trauma.",


            "Providing legal guidance on obtaining government assistance and financial support.",
            "Assisting evacuees in filling out forms for emergency relief and aid programs.",
            "Helping evacuees understand their rights regarding property and compensation claims.",
            "Providing assistance with the submission of claims for damages to property.",
            "Advising evacuees on how to navigate legal processes to apply for housing benefits.",
            "Coordinating with legal aid organizations to offer free consultations to evacuees.",
            "Helping evacuees access social security benefits and other state-sponsored programs.",
            "Offering advice on how to obtain identity documents and replacement papers after losing them.",
            "Providing legal counseling to evacuees who need to resolve employment or housing issues.",
            "Helping evacuees understand the process for applying for long-term housing support.",


         };


        for (int i = 0; i < 50; i++)
        {
            string description = descriptions[i];
            int j = i / 10;
            CallType callType = (CallType)j;
            string address = addresses[i];
            double latitude = latitudes[i];
            double longitude = longitudes[i];

            // Base the opening time around the current system clock
            DateTime now = s_dal!.Config.Clock;
            int maxOffsetInHours = 24; // Set max offset to 24 hours
            TimeSpan randomOffset = TimeSpan.FromHours(s_rand.Next(maxOffsetInHours) - maxOffsetInHours / 2);
            DateTime openingTime = now + randomOffset;

            // Set maximum ending time to 12 hours after opening
            DateTime? maxEndingTime = openingTime.AddHours(12);

            Call newCall = new(0, callType, address, latitude, longitude, openingTime, description, maxEndingTime);
            s_dal!.Call.Create(newCall);
        }

    }
    /// <summary>
    /// Creates a list of assignments linking volunteers to calls.
    /// </summary>



    private static void create_assignment()
    {
        var calls = s_dal!.Call.ReadAll().ToList(); // Get all calls
        var volunteers = s_dal!.Volunteer.ReadAll().ToList(); // Get all volunteers

        int callsCount = calls.Count;
        int volunteersCount = volunteers.Count;
        
        if (callsCount == 0 || volunteersCount == 0)
            throw new InvalidOperationException("No calls or volunteers available.");

        Random random = new Random();

        // Shuffle calls and take 50% for assignment
        var shuffledCalls = calls.OrderBy(_ => random.Next()).ToList();
        var assignedCalls = shuffledCalls.Take(callsCount / 2).ToList(); // 50% assigned
        var unassignedCalls = shuffledCalls.Skip(callsCount / 2).ToList(); // Remaining calls

        foreach (var call in assignedCalls)
        {
            // Filter volunteers to ensure no volunteer has more than one active assignment
            var availableVolunteers = volunteers.Where(v =>
            {
                var assignments = s_dal.Assignment.ReadAll(a => a.VolunteerId == v.Id);
                return assignments.All(a => a.EndTimeType != null); // Volunteer has no active call
            }).ToList();

            if (availableVolunteers.Count == 0)
                break; // No available volunteers left for assignment

            // Select a random volunteer
            var volunteer = availableVolunteers[random.Next(availableVolunteers.Count)];

            // Generate entry time between call opening and max ending time
            DateTime entryTime = call.OpeningTime.AddMinutes(random.Next(1,
                Math.Max(1, (int)((call.maxEndingTime ?? DateTime.Now) - call.OpeningTime).TotalMinutes)));

            // Randomly determine if EndTimeType and EndTime should be null
            bool isNullAssignment = random.NextDouble() < 0.5; // 50% chance for null assignment

            DateTime? endTime = null;
            EndTimeType? endTimeType = null;

            if (!isNullAssignment)
            {
                double treatmentChance = random.NextDouble();
                if (treatmentChance < 0.7) // 70% chance to be treated
                {
                    endTime = entryTime.AddMinutes(15 + random.Next(106)); // Time for treatment (15 to 120 minutes)
                    endTimeType = EndTimeType.Treated;
                }
                else if (treatmentChance < 0.85) // 15% chance to be self-canceled
                {
                    endTime = entryTime.AddMinutes(1 + random.Next(60)); // Self-cancel time (1 to 60 minutes after entry)
                    endTimeType = EndTimeType.SelfCancel;
                }
                else if (treatmentChance < 0.95) // 10% chance to be manager-canceled
                {
                    endTime = entryTime.AddMinutes(1 + random.Next(60)); // Manager cancel time
                    endTimeType = EndTimeType.ManagerCancel;
                }
                else // 5% chance to expire
                {
                    endTime = call.maxEndingTime; // Expiration time
                    endTimeType = EndTimeType.Expired;
                }
            }

            // Create a new Assignment object
            Assignment newAssignment = new(
                0,
                call.Id,
                volunteer.Id,
                entryTime,
                endTimeType,
                endTime
            );

            // Create the assignment in the database
            s_dal!.Assignment.Create(newAssignment);
        }
    }


    /// <summary>
    /// Initializes the DAL objects, resets configuration values, and populates lists of volunteers, calls, and assignments.
    /// </summary>
    /// <param name="dalVolunteer">Instance for volunteer data access layer.</param>
    /// <param name="dalcall">Instance for call data access layer.</param>
    /// <param name="dalAssignment">Instance for assignment data access layer.</param>
    /// <param name="dalConfig">Instance for configuration data access layer.</param>
    /// <exception cref="NullReferenceException">Thrown if any of the DAL objects are null.</exception>
    public static void Do()
    {
        s_dal = DalApi.Factory.Get;

        Console.WriteLine("Resetting configuration values and clearing lists...");

        /// <summary>
        /// Resets configuration values to defaults and deletes all existing entries in volunteers, assignments, and calls lists.
        /// </summary>
        s_dal.ResetDB();

        Console.WriteLine("Reset Configuration values and List values...");
        create_volunteer();
        create_call();
        create_assignment();
    }
    public static void Bo()
    {
        s_dal = DalApi.Factory.Get;

        Console.WriteLine("Resetting configuration values and clearing lists...");

        /// <summary>
        /// Resets configuration values to defaults and deletes all existing entries in volunteers, assignments, and calls lists.
        /// </summary>
        s_dal.ResetDB();

        Console.WriteLine("Reset Configuration values and List values...");
        create_volunteer();
        create_call();
        create_assignment();
    }

}
