
namespace Dal;

static internal class Config
{
internal const int startCallId = 0;
private static int nextCallId = startCallId;
internal static int NextCallId { get => nextCallId++; }

internal const int startAssignmentId = 0;
private static int next_assignment_id = startAssignmentId;
internal static int NextAssignmentId { get => next_assignment_id++; }




    internal static DateTime Clock {  get; set; }=DateTime.Now;
    internal static TimeSpan RiskRange = TimeSpan.FromHours(12);
    //private static Dictionary<string, string> configVariables = new Dictionary<string, string>();
    //internal static void SetConfigValue(string variableName, string newValue)
    //{
    //    configVariables[variableName] = newValue;
    //}
    //internal static string GetConfigValue(string variableName)
    //{
    //    return configVariables.TryGetValue(variableName, out var value) ? value : "Variable not found";
    //}
    // מילון למשתני תצורה
    private static Dictionary<string, string> configVariables = new Dictionary<string, string>();

    // עדכון משתנה תצורה
    internal static void SetConfigValue(string variableName, string newValue)
    {
        configVariables[variableName] = newValue;
    }

    // השגת ערך של משתנה תצורה
    internal static string GetConfigValue(string variableName)
    {
        return configVariables.TryGetValue(variableName, out var value) ? value : "Variable not found";
    }
    internal static void Reset()
    {
        nextCallId = startCallId;
        next_assignment_id = startAssignmentId;
        Clock = DateTime.Now;
        RiskRange= TimeSpan.FromHours(12);
    }
}