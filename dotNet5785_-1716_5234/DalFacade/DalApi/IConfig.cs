
namespace DalApi;

public interface IConfig
{
    DateTime Clock { get; set; }
    TimeSpan RiskRange { get; set; }
    int NextCallId { get; }
    int NextAssignmentId { get; }

    void Reset();
    private static Dictionary<string, string> configVariables = new Dictionary<string, string>();
    void SetConfigValue(string variableName, string newValue);

    string GetConfigValue(string variableName);

}