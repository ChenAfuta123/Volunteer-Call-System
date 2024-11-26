namespace Dal;

internal static class Config
{
    /// <summary>
    /// The file path for storing the data configuration XML.
    /// </summary>
    internal const string s_data_config_xml = "data-config.xml";

    /// <summary>
    /// The file path for storing the volunteers data XML.
    /// </summary>
    internal const string s_volunteers_xml = "volunteers.xml";

    /// <summary>
    /// The file path for storing the calls data XML.
    /// </summary>
    internal const string s_calls_xml = "calls.xml";

    /// <summary>
    /// The file path for storing the assignments data XML.
    /// </summary>
    internal const string s_assignments_xml = "assignments.xml";

    /// <summary>
    /// The ID for the next call, managed by the configuration file.
    /// </summary>
    internal static int NextCallId
    {
        get => XMLTools.GetAndIncreaseConfigIntVal(s_data_config_xml, "NextCallId");
        private set => XMLTools.SetConfigIntVal(s_data_config_xml, "NextCallId", 0);
    }

    /// <summary>
    /// The ID for the next assignment, managed by the configuration file.
    /// </summary>
    internal static int NextAssignmentId
    {
        get => XMLTools.GetAndIncreaseConfigIntVal(s_data_config_xml, "NextAssignmentId");
        private set => XMLTools.SetConfigIntVal(s_data_config_xml, "NextAssignmentId", value);
    }

    /// <summary>
    /// The system clock time, stored and retrieved from the configuration file.
    /// </summary>
    internal static DateTime Clock
    {
        get => XMLTools.GetConfigDateVal(s_data_config_xml, "Clock");
        set => XMLTools.SetConfigDateVal(s_data_config_xml, "Clock", value);
    }

    /// <summary>
    /// The risk range (in time) for calls, stored in the configuration file.
    /// </summary>
    internal static TimeSpan RiskRange
    {
        get => XMLTools.GetConfigTimeSpanVal(s_data_config_xml, "RiskRange");
        set => XMLTools.SetConfigTimeSpanVal(s_data_config_xml, "RiskRange", value);
    }

    /// <summary>
    /// Resets the configuration values to their default settings.
    /// </summary>
    internal static void Reset()
    {
        NextCallId = 0;
        NextAssignmentId = 0;
        Clock = DateTime.Now;
        RiskRange = TimeSpan.FromHours(12);
    }
}
