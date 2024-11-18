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
        string[] names = {
            "Manager", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof",
            "Tamar Avni", "Noam Baruch", "Yael Tzur", "Yonatan Gilad", "Lior Hadad", "Maya Zohar",
            "Idan Sasson", "Avigail Shani", "Eden Bar", "Nadav Shavit", "Gal Mor", "Adi Nir",
            "Tal Shaked", "Ofek Azulai"
        };

        string[] emails = {
            "Manager@example.com", "eli@example.com", "yair@example.com", "ariela@example.com", "dina@example.com", "shira@example.com",
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

      
        for (int i = 0; i < 20; i++)
        {
            int id;

            /// <summary>
            /// Generates a unique ID for each volunteer.
            /// </summary>
            do
            {
                id = s_rand.Next(200000000, 400000000);
            }
            while (s_dal!.Volunteer.Read(id) != null);

            string name = names[s_rand.Next(names.Length)];
            string email = emails[s_rand.Next(emails.Length)];
            int index = s_rand.Next(addresses.Length);
            string address = addresses[index];
            double latitude = latitudes[index];
            double longitude = longitudes[index];
            string phoneNumber = $"05{s_rand.Next(0, 10)}-{s_rand.Next(1000000, 9999999)}";
            double maxDistance = s_rand.Next(1, 50);

            /// <summary>
            /// Creates a new Volunteer object.
            /// </summary>
            Volunteer newVolunteer = new(id, name, phoneNumber, email, null, address, latitude, maxDistance, true);

            s_dal!.Volunteer.Create(newVolunteer);
        }
    }

    /// <summary>
    /// Creates a list of assignments linking volunteers to calls.
    /// </summary>
    private static void create_assignment()
    {
        for (int i = 0; i < 50; i++)
        {
            int id = s_dal!.Config.NextAssignmentId;

            var calls = s_dal!.Call.ReadAll();
            if (calls.Count() == 0)
                throw new InvalidOperationException("There are no calls available to create assignments.");

            int callId = calls.ElementAt(s_rand.Next(calls.Count())).Id;

            var volunteers = s_dal!.Volunteer.ReadAll();
            if (volunteers.Count() == 0)
                throw new InvalidOperationException("There are no volunteers available to create assignments.");

            int volunteerId = volunteers.ElementAt(s_rand.Next(volunteers.Count())).Id;

            var call = calls.First(c => c.Id == callId);
            DateTime entryTime = call.OpeningTime.AddMinutes(s_rand.Next(1, (int)((call.maxEndingTime - call.OpeningTime)?.TotalMinutes ?? 0)));
            DateTime? endTime = null;
            EndTimeType endTimeType = EndTimeType.Treated;

            if (s_rand.NextDouble() < 0.5)
            {
                endTime = entryTime.AddMinutes(s_rand.Next((int)((call.maxEndingTime - entryTime)?.TotalMinutes ?? 0)));
                endTimeType = EndTimeType.Treated;
            }
            else
            {
                if (s_rand.NextDouble() < 0.33)
                {
                    endTime = (call.maxEndingTime ?? DateTime.Now).AddMinutes(s_rand.Next(1, 120));
                    endTimeType = EndTimeType.Expired;
                }
                else if (s_rand.NextDouble() < 0.66)
                {
                    endTime = entryTime.AddMinutes(s_rand.Next(1, 60));
                    endTimeType = EndTimeType.SelfCancel;
                }
                else
                {
                    endTime = entryTime.AddMinutes(s_rand.Next(1, 60));
                    endTimeType = EndTimeType.ManagerCancel;
                }
            }

            /// <summary>
            /// Creates a new Assignment object linking a volunteer to a call.
            /// </summary>
            Assignment newAssignment = new(id, callId, volunteerId, entryTime, null, endTime) { };

            s_dal!.Assignment.Create(newAssignment);
        }
    }

    /// <summary>
    /// Creates a list of emergency calls with random attributes.
    /// </summary>
    private static void create_call()
    {
        string[] addresses = {
    "Tel Aviv, Dizengoff St 50", "Jerusalem, Jaffa St 10", "Haifa, HaNassi Blvd 80", "Beersheba, Rager Blvd 15", "Eilat, HaTmarim Blvd 20",
    "Petah Tikva, Em Hamoshavot Rd 45", "Netanya, Herzl St 12", "Ashdod, HaNamal St 30", "Rehovot, Herzl St 65", "Holon, Sokolov St 22",
    "Bat Yam, Ben Gurion Blvd 15", "Rishon Lezion, Rothschild St 35", "Ramat Gan, Abba Hillel Silver Rd 100", "Herzliya, Ben Yehuda St 55",
    "Kfar Saba, Weizmann St 28", "Ra'anana, Ahuza St 90", "Modiin, Yigal Alon St 17", "Ashkelon, Ben Gurion Blvd 60", "Nahariya, Sokolov St 18",
    "Acre, Ben Ami Blvd 5", "Karmiel, HaGalil St 7", "Dimona, Haim Bar Lev St 40", "Yokneam, HaTzabar St 2", "Beit Shemesh, Nahar Hayarden St 8",
    "Afula, HaRakevet St 10", "Tiberias, HaBanim St 15", "Sderot, HaDekel St 3", "Or Yehuda, HaTamar St 25", "Ramat HaSharon, Sokolov St 40",
    "Giv'atayim, Katznelson St 5", "Shoham, HaAtzmaut St 6", "Even Yehuda, HaShaked St 13", "Gedera, HaZayit St 12", "Yavne, HaGefen St 4",
    "Omer, HaShikma St 9", "Shlomi, HaZayit St 16", "Migdal HaEmek, HaDekel St 7", "Kiryat Shmona, David Elazar St 11",
    "Kiryat Bialik, HaTzafon St 3", "Rosh HaAyin, HaHagana St 1", "Kiryat Ata, HaYovel St 25", "Lod, HaHistadrut St 30",
    "Ramla, Herzl St 27", "Eilot, HaNegev St 2", "Arad, HaShalom St 14", "Ma'alot Tarshiha, HaTeena St 12", "Zichron Yaakov, HaEtrog St 19",
    "Ramat Efal, HaRakevet St 8", "Tel Mond, HaGoren St 15", "Mazkeret Batya, HaDekel St 3"
};

        double[] latitudes = {
    32.0805, 31.7683, 32.7940, 31.2518, 29.5577, 32.084, 32.321, 31.804, 31.894, 32.011,
    32.017, 31.972, 32.082, 32.162, 32.175, 32.185, 31.896, 31.668, 33.008, 32.923,
    32.919, 31.071, 32.656, 31.739, 32.607, 32.792, 31.527, 32.031, 32.145, 32.068,
    32.035, 31.938, 31.888, 31.878, 32.946, 32.802, 33.207, 32.799, 32.813, 32.123,
    32.052, 31.951, 31.246, 31.27, 33.015, 32.573, 32.062, 32.180, 31.861, 31.845
};

        double[] longitudes = {
    34.7818, 35.2137, 34.9896, 34.7915, 34.9501, 34.871, 34.853, 34.645, 34.812, 34.772,
    34.748, 34.804, 34.823, 34.839, 34.906, 34.870, 35.010, 34.599, 35.093, 35.082,
    35.297, 35.034, 35.104, 34.989, 35.289, 35.540, 34.596, 34.839, 34.839, 34.810,
    34.915, 34.779, 34.701, 34.775, 35.302, 35.571, 35.573, 35.033, 35.113, 34.880,
    34.900, 34.873, 34.792, 34.736, 35.276, 35.046, 34.814, 34.935, 34.851, 34.836
};

        string[] descriptions = {
    "Food delivery – providing essential food and drinks to evacuees in shelters.",
    "Temporary housing – assisting an evacuee family in finding temporary accommodation.",
    "Trauma support – escorting evacuees needing mental health assistance.",
    "Children’s activities – organizing activities for children in shelters.",
    "Transport help – assisting with moving personal belongings to safe housing.",
    "Medication delivery – collecting and delivering essential medications.",
    "Mental health support – connecting evacuees to counseling services.",
    "Heating supplies – providing blankets and heaters for cold weather.",
    "Evacuee transport – arranging safe transport to temporary shelters.",
    "Legal aid – offering guidance on government aid and legal support.",

    "Food distribution – delivering meals to evacuees in need.",
    "Temporary shelter arrangement – locating safe temporary housing for families.",
    "Mental health support – providing on-site counseling for evacuees.",
    "Activities for children – creating art and play activities for children.",
    "Moving assistance – helping families relocate their personal belongings.",
    "Essential medication delivery – supplying evacuees with needed medications.",
    "Therapy referrals – connecting evacuees with mental health professionals.",
    "Warm clothing – distributing warm clothing and blankets to evacuees.",
    "Transport to shelters – organizing rides for evacuees.",
    "Legal consultation – helping evacuees understand their legal rights.",

    "Nutrition support – ensuring evacuees receive proper meals and hydration.",
    "Temporary home search – helping locate short-term housing solutions.",
    "Crisis support – providing assistance to evacuees dealing with trauma.",
    "Recreational activities – planning games and crafts for children.",
    "Logistics assistance – organizing personal items for safe transport.",
    "Pharmacy pickups – gathering and delivering necessary medications.",
    "Emotional support – connecting evacuees with mental health support.",
    "Cold-weather supplies – handing out heaters and winter essentials.",
    "Safe transportation – coordinating transport to secure locations.",
    "Legal help – offering support with paperwork and benefits.",

    "Grocery delivery – providing groceries for evacuees.",
    "Accommodation support – helping evacuees find places to stay.",
    "Psychological first aid – escorting evacuees needing emotional support.",
    "Kids' crafts – organizing crafts and fun for children in shelters.",
    "Relocation aid – helping move belongings to temporary housing.",
    "Medical pickups – ensuring evacuees receive their prescriptions.",
    "Mental health hotline – offering 24/7 support access.",
    "Winter gear – distributing coats, blankets, and heaters.",
    "Evacuee transport coordination – arranging group transport to shelters.",
    "Legal advisory – offering evacuees advice on aid programs.",

    "Essential supplies – delivering food and hygiene products.",
    "Refuge assistance – finding temporary refuge for families.",
    "Trauma guidance – providing mental health resources.",
    "Fun activities – organizing play areas for children.",
    "Packing aid – helping evacuees pack personal items.",
    "Prescription delivery – collecting and bringing medications.",
    "Support groups – connecting evacuees to group counseling.",
    "Blanket distribution – providing blankets for warmth.",
    "Emergency transport – facilitating urgent transport needs.",
    "Legal aid services – guiding evacuees on legal resources."
};

        for (int i = 0; i < 50; i++)
        {
            int id=s_dal!.Config.NextCallId;
            string description = descriptions[s_rand.Next(descriptions.Length)];
            int index = s_rand.Next(addresses.Length);
            string address = addresses[index];
            double latitude = latitudes[index];
            double longitude = longitudes[index];
            DateTime start = new DateTime(s_dal!.Config.Clock.Year - 2, 1, 1);
            int range = (s_dal!.Config.Clock - start).Days; 
            DateTime openingTime= start.AddDays(s_rand.Next(range));
            Call newCall = new(id, address, latitude, longitude, openingTime, description);
            s_dal!.Call.Create(newCall);
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
    public static void Do(IDal dal)
    {
        s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); // stage 2


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

