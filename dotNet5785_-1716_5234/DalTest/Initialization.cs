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
            int id = ids[i];
            string name = names[i];
            string email = emails[i];
            string address = addresses[i];
            double latitude = latitudes[i];
            double longitude = longitudes[i];
            string phoneNumber = $"05{s_rand.Next(0, 10)}-{s_rand.Next(1000000, 9999999)}";
            double maxDistance = s_rand.Next(1, 50);
            Role role = (i == 0) ? Role.volunteer : Role.manager;
            DistanceType distanceType = (DistanceType)(i % Enum.GetValues(typeof(DistanceType)).Length);
            /// <summary>
            /// Creates a new Volunteer object.
            /// </summary>

            Volunteer newVolunteer = new(id, distanceType, role,name, phoneNumber, email, null, address, latitude, longitude, maxDistance, true);

            s_dal!.Volunteer.Create(newVolunteer);
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
            DateTime start = new DateTime(s_dal!.Config.Clock.Year - 2, 1, 1);
            int range = (s_dal!.Config.Clock - start).Days;
            DateTime openingTime = start.AddDays(s_rand.Next(range));
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
        // Assuming s_dal.Call.Read() returns a list of calls, and s_dal.Volunteer.Read() returns a list of volunteers
        var calls = s_dal!.Call.ReadAll(); // Get all calls
        var volunteers = s_dal!.Volunteer.ReadAll(); // Get all volunteers

        int callsCount = calls.Count();
        int volunteersCount = volunteers.Count();

        for (int i = 0; i < 50; i++) // Generating 50 assignments
        {
            // Select a call based on i
            var randomCall = calls.ElementAt(i % callsCount);

            // Select a volunteer based on i
            var randomVolunteer = volunteers.ElementAt(i % volunteersCount);

            // Generate entry time: It should be between call opening and max ending time
            DateTime entryTime = randomCall.OpeningTime.AddMinutes(i % Math.Max(1, (int)((randomCall.maxEndingTime ?? DateTime.Now) - randomCall.OpeningTime).TotalMinutes));

            // Randomly determine the end time for the assignment
            DateTime? endTime = null;
            EndTimeType? endTimeType = null;

            // Decide on treatment type and whether the call was treated or expired
            double treatmentChance = (i % 100) / 100.0; // Normalized chance based on i
            if (treatmentChance < 0.7) // 70% chance to be treated
            {
                endTime = entryTime.AddMinutes(15 + (i % 106)); // Time for treatment (15 to 120 minutes)
                endTimeType = EndTimeType.Treated;
            }
            else if (treatmentChance < 0.85) // 15% chance to be canceled by the volunteer
            {
                endTimeType = EndTimeType.SelfCancel;
                endTime = entryTime.AddMinutes(1 + (i % 60)); // Self-cancel time (1 to 60 minutes after entry)
            }
            else if (treatmentChance < 0.95) // 10% chance to be canceled by the manager
            {
                endTimeType = EndTimeType.ManagerCancel;
                endTime = entryTime.AddMinutes(1 + (i % 60)); // Manager cancel time
            }
            else // 5% chance for the call to expire
            {
                endTimeType = EndTimeType.Expired;
                endTime = randomCall.maxEndingTime; // Call expiration time
            }

            // Create a new Assignment object
            Assignment newAssignment = new(
                0,
                randomCall.Id,
                randomVolunteer.Id,
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

