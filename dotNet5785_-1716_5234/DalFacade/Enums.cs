namespace DalFacade { }

    /// <summary>
    /// Enumeration representing the various types of end times for an event or process.
    /// This enum defines the reasons or methods by which an event can be terminated.
    /// </summary>
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

