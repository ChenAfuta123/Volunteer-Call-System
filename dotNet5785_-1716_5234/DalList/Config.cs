using System.Runtime.CompilerServices;

namespace Dal;

/// <summary>
/// Configuration class for managing IDs and default settings for various entities in the system.
/// </summary>
static internal class Config 
{
    /// <summary>
    /// Initial ID for Call entities, set to 0.
    /// </summary>
    internal const int startCallId = 0;

    /// <summary>
    /// Holds the next available unique ID for Call entities, auto-incremented.
    /// </summary>
    private static int nextCallId = startCallId;

    /// <summary>
    /// Property to retrieve the next available Call ID, which increments each time it's accessed.
    /// </summary>
    internal static int NextCallId { [MethodImpl(MethodImplOptions.Synchronized)] get => nextCallId++; }

    /// <summary>
    /// Initial ID for Assignment entities, set to 0.
    /// </summary>
    internal const int startAssignmentId = 0;

    /// <summary>
    /// Holds the next available unique ID for Assignment entities, auto-incremented.
    /// </summary>
    private static int next_assignment_id = startAssignmentId;

    /// <summary>
    /// Property to retrieve the next available Assignment ID, which increments each time it's accessed.
    /// </summary>
    internal static int NextAssignmentId { [MethodImpl(MethodImplOptions.Synchronized)] get => next_assignment_id++; }

    /// <summary>
    /// Represents the current date and time in the system, initialized to the current date and time.
    /// </summary>
    internal static DateTime Clock { [MethodImpl(MethodImplOptions.Synchronized)] get; 
        [MethodImpl(MethodImplOptions.Synchronized)] set; } = DateTime.Now;

    /// <summary>
    /// Time range within which a call is considered at risk, set to 12 hours by default.
    /// </summary>
    internal static TimeSpan RiskRange = TimeSpan.FromHours(12);
    
    private static Dictionary<string, string> configVariables = new Dictionary<string, string>();

    [MethodImpl(MethodImplOptions.Synchronized)]
    internal static void SetConfigValue(string variableName, string newValue)
    {
        configVariables[variableName] = newValue;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    internal static string GetConfigValue(string variableName)
    {
        return configVariables.TryGetValue(variableName, out var value) ? value : "Variable not found";
    }

    /// <summary>
    /// Resets configuration values to their initial states, including IDs, clock, and risk range.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    internal static void Reset()
    {
        nextCallId = startCallId;
        next_assignment_id = startAssignmentId;
        Clock = DateTime.Now;
        RiskRange = TimeSpan.FromHours(12);
    }
}
