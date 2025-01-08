namespace BO;

/// <summary>Enum representing the role of the volunteer within the system.</summary>
public enum Role
{
    volunteer,
    manager
}

/// <summary>Enum representing the type of calls in the system.</summary>
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

    /// <summary>Call does not exist in volunteer's treatment.</summary>
    None
}

/// <summary>Enum representing the status of a call.</summary>
public enum CallStatus
{
    Open,
    InProgress,
    Closed,
    Expired,
    OpenAtRisk,
    InProgressAtRisk
}

/// <summary>Enum representing units of time for clock operations.</summary>
public enum TimeUnit
{
    MINUTE,
    HOUR,
    DAY,
    MONTH,
    YEAR
}

/// <summary>Enum representing distance measurement types.</summary>
public enum DistanceType
{
    AirDistance,
    WalkingDistance,
    DrivingDistance
}

/// <summary>Fields used for sorting or filtering volunteers in a list.</summary>
public enum VolunteerInListFields
{
    Name,
    HandledCallId,
    TotalHandledCalls,
    TotalExpiredCalls,
    TotalCanceledCalls,
     None
}

/// <summary>Types of end times for calls.</summary>
public enum EndTimeType
{
    Treated,
    SelfCancel,
    ManagerCancel,
    Expired
}

/// <summary>Fields used for sorting or filtering calls in a list.</summary>
public enum CallInListField
{
    Id,
    CallId,
    CallType,
    OpeningTime,
    RemainingCallTime,
    LastVolunteerName,
    TotalHandlingTime,
    CallStatus,
    TotalAllocations,
    None
}

/// <summary>Fields used for open call listings.</summary>
public enum OpenCallInListField
{
    Id,
    callType,
    description,
    Address,
    OpeningTime,
    maxEndingTime,
    CallDistanceFromVolunteer
}

/// <summary>Fields used for closed call listings.</summary>
public enum ClosedCallInListField
{
    Id,
    CallType,
    Address,
    OpeningTime,
    EntryTime,
    EndTime,
    EndTimeType
}
public enum IsActiveFilter
{
    Active,
   Not_Active,
    None
}
public enum ClosedCallInListFilter
{
    Id,
    //callType,
    Address,
    OpeningTime,
    EntryTime,
    EndTime,
    //EndTimeType
}
