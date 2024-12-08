namespace BO;
/// <summary>
/// Enum representing the role of the volunteer within the system.
/// </summary>
public enum Role
{
    volunteer,
    manager
}

public enum CallType
{
    /// <summary>Providing food, drinks, clothing, and essential supplies to evacuees.</summary>
    EssentialSupplies,

    /// <summary>Helping evacuees find temporary housing and assisting with relocation and transport.</summary>
    HousingAndRelocation,

    /// <summary>Offering emotional support, trauma counseling, and organizing social activities.</summary>
    EmotionalAndSocialSupport,

    /// <summary>Delivering medications and assisting with access to medical services.</summary>
    MedicalAndPharmaceuticalAid,

    /// <summary>Providing legal advice and guidance on accessing government aid and support.</summary>
    LegalAndAdministrativeSupport,
    /// <summary>call does not exist in volunteer's treatment.</summary>
    None
}
public enum CallStatus
{
    Open,           
    InProgress,     
    Closed,        
    Expired,      
    OpenAtRisk,    
    InProgressAtRisk 
}
public enum TimeUnit
{
    MINUTE,
    HOUR,
    DAY,
    MONTH,
    YEAR
}
public enum EndTimeType
{
    /// <summary>
    /// Indicates that the event or process was successfully treated.
    /// </summary>
    Treated,

    /// <summary>
    /// Indicates that the event or process was canceled by the user or subject itself.
    /// </summary>
    SelfCancel,

    /// <summary>
    /// Indicates that the event or process was canceled by a manager or administrator.
    /// </summary>
    ManagerCancel,

    /// <summary>
    /// Indicates that the event or process ended because it expired.
    /// </summary>
    Expired
}
public enum Filter
{
    Name,
    HandledCallId,
    TotalHandledCalls
}
public enum CallInListField
{
    Id,               // מזהה קריאה
    CallId,           // מזהה הקריאה (המכיל את מזהה הקריאה המקורי)
    CallType,         // סוג הקריאה
    OpeningTime,      // זמן פתיחת הקריאה
    RemainingCallTime,// הזמן שנותר לקריאה
    LastVolunteerName,// שם המתנדב האחרון
    TotalHandlingTime,// זמן הטיפול הכולל
    CallStatus,       // סטטוס הקריאה
    TotalAllocations  // מספר ההקצאות הכולל
}
