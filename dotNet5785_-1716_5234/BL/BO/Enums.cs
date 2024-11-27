namespace BO;
/// <summary>
/// Enum representing the role of the volunteer within the system.
/// </summary>
public enum Role
{
    volunteer,
    manager
}

/// <summary>
/// Enum representing the type of distance measurement used.
/// </summary>
public enum DistanceType
{
    AirDistance,
    WalkingDistance,
    DrivingDistance
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