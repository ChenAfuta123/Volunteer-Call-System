
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
    internal static void Reset()
    {
        nextCallId = startCallId;
        next_assignment_id = startAssignmentId;
        Clock = DateTime.Now;
        RiskRange= TimeSpan.FromHours(12);
    }
}