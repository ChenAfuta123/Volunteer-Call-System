using System;
using System.Collections;
using System.Collections.Generic;

namespace PL
{
    internal class CallsCollection : IEnumerable
    {
        static readonly IEnumerable<BO.CallInListField> s_enums =
            (Enum.GetValues(typeof(BO.CallInListField)) as IEnumerable<BO.CallInListField>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    internal class CallTypeCollection : IEnumerable
    {
        // יצירת IEnumerable עבור הערכים של ה-enum DistanceType
        static readonly IEnumerable<Enums.CallType> s_enums =
            Enum.GetValues(typeof(Enums.CallType)).Cast<Enums.CallType>();

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    internal class StatusCollection : IEnumerable
    {
        // יצירת IEnumerable עבור הערכים של ה-enum Role
        static readonly IEnumerable<Enums.Status> s_enums =
            Enum.GetValues(typeof(Enums.Status)).Cast<Enums.Status>();

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    internal class VolunteersCollection : IEnumerable
    {
        static readonly IEnumerable<BO.VolunteerInListFields> s_enums =
            (Enum.GetValues(typeof(BO.VolunteerInListFields)) as IEnumerable<BO.VolunteerInListFields>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    internal class DistanceTypeCollection : IEnumerable
    {
        // יצירת IEnumerable עבור הערכים של ה-enum DistanceType
        static readonly IEnumerable<Enums.DistanceType> s_enums =
            Enum.GetValues(typeof(Enums.DistanceType)).Cast<Enums.DistanceType>();

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    internal class RoleCollection : IEnumerable
    {
        // יצירת IEnumerable עבור הערכים של ה-enum Role
        static readonly IEnumerable<Enums.Role> s_enums =
            Enum.GetValues(typeof(Enums.Role)).Cast<Enums.Role>();

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    internal class IsActiveFilterCollection : IEnumerable
    {
        // יצירת IEnumerable עבור הערכים של ה-enum Role
        static readonly IEnumerable<BO.IsActiveFilter> s_enums =
            Enum.GetValues(typeof(BO.IsActiveFilter)).Cast<BO.IsActiveFilter>();

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    internal class ClosedCallInListFieldCollection : IEnumerable
    {
        // יצירת IEnumerable עבור הערכים של ה-enum Role
        static readonly IEnumerable<BO.ClosedCallInListField> s_enums =
            Enum.GetValues(typeof(BO.ClosedCallInListField)).Cast<BO.ClosedCallInListField>();

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    internal class OpenCallsCollection : IEnumerable
    {
        static readonly IEnumerable<BO.OpenCallInListField> s_enums =
            (Enum.GetValues(typeof(BO.OpenCallInListField)) as IEnumerable<BO.OpenCallInListField>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    internal class CallType : IEnumerable
    {
        static readonly IEnumerable<BO.CallType> s_enums =
            (Enum.GetValues(typeof(BO.CallType)) as IEnumerable<BO.CallType>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    internal class OpenCallInListField : IEnumerable
    {
        static readonly IEnumerable<BO.OpenCallInListField> s_enums =
            (Enum.GetValues(typeof(BO.OpenCallInListField)) as IEnumerable<BO.OpenCallInListField>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
    public class Enums
    {

        public enum Role
        {
            volunteer,
            manager
        }
        public enum DistanceType
        {
            AirDistance,
            WalkingDistance,
            DrivingDistance
        }
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
        public enum Status
        {
            Open,
            InProgress,
            Closed,
            Expired,
            OpenAtRisk,
            InProgressAtRisk
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

            /// <summary>Call does not exist in volunteer's treatment.</summary>
            None
        }
        public enum OpenCallInListField
        {
            Id,
            callType,
            description,
            Address,
            OpeningTime,
            maxEndingTime,
            CallDistanceFromVolunteer,
            None
        }
    }
}