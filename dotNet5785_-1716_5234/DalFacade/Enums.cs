namespace DO;
/// <summary>
/// Enum representing general categories of assistance for evacuees.
/// </summary>
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

/// <summary>
/// Enumeration representing the various types of end times for an event or process.
/// This enum defines the reasons or methods by which an event can be terminated.
/// </summary>
/// 

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



