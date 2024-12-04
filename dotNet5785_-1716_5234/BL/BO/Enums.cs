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
    LegalAndAdministrativeSupport
}
public enum CallStatus
{
    Open,           // לא בטיפול כרגע, גם אם הייתה בטיפול בעבר ונעצרה
    InProgress,     // בטיפול כרגע על ידי מתנדב
    Closed,         // סגורה - המתנדב סיים לטפל בה
    Expired,        // פג תוקף - לא הסתיימה בזמן או לא נבחרה לטיפול
    OpenAtRisk,     // קריאה פתוחה שמתקרבת לזמן סיום, במרחק זמן סיכון
    InProgressAtRisk // קריאה בטיפול שמתקרבת לזמן סיום, במרחק זמן סיכון
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