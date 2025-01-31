namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

internal class VolunteerImplementation : IVolunteer
{
    /// <summary>
    /// Creates an XML element representing a Volunteer object.
    /// </summary>
    /// <param name="volunteer">The volunteer object to be converted.</param>
    /// <returns>An XElement representing the Volunteer.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    private XElement createVolunteerElement(Volunteer volunteer)
    {
        return new XElement("Volunteer",
            new XElement("Id", volunteer.Id),
            new XElement("DistanceType", volunteer.distanceType),
            new XElement("Role", volunteer.role),
            new XElement("Name", volunteer.Name),
            new XElement("PhoneNumber", volunteer.PhoneNumber),
            new XElement("Email", volunteer.Email),
            new XElement("Password", volunteer.Password),
            new XElement("Address", volunteer.Address),
            new XElement("Latitude", volunteer.Latitude),
            new XElement("Longitude", volunteer.Longitude),
            new XElement("MaxDistance", volunteer.MaxDistance),
            new XElement("Active", volunteer.Active)
        );
    }

    /// <summary>
    /// Creates a new volunteer record in the data source.
    /// </summary>
    /// <param name="item">The volunteer object to be created.</param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Create(Volunteer item)
    {
        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);

        Volunteer? volunteer = Read(item.Id);

        if (volunteer != null) throw new DO.DalAlreadyExistsException($"Volunteer with ID={item.Id} already exist");

        volunteersRootElem.Add(createVolunteerElement(item));
        XMLTools.SaveListToXMLElement(volunteersRootElem, Config.s_volunteers_xml);
    }

    /// <summary>
    /// Deletes a volunteer record by its ID from the data source.
    /// </summary>
    /// <param name="id">The ID of the volunteer to be deleted.</param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        Volunteer? volunteer = Read(id);

        if (volunteer == null) throw new DO.DalDoesNotExistsException($"Volunteer with ID={id} does not exist");

        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);
        XElement? volunteerElem = volunteersRootElem.Elements()
            .FirstOrDefault(st => (int?)st.Element("Id") == id);

        volunteerElem?.Remove();

        XMLTools.SaveListToXMLElement(volunteersRootElem, Config.s_volunteers_xml);

        Console.WriteLine($"Volunteer with ID={id} was successfully deleted.");
    }

    /// <summary>
    /// Deletes all volunteer records from the data source.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void DeleteAll()
    {
        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);

        volunteersRootElem.Elements().Remove();

        XMLTools.SaveListToXMLElement(volunteersRootElem, Config.s_volunteers_xml);
    }

    /// <summary>
    /// Retrieves all volunteers, optionally filtered by a given criteria.
    /// </summary>
    /// <param name="filter">An optional filter function to filter the list of volunteers.</param>
    /// <returns>An enumerable list of Volunteer objects.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null)
    {
        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);
        return volunteersRootElem.Elements().Select(getVolunteer);
    }

    /// <summary>
    /// Converts an XElement into a Volunteer object.
    /// </summary>
    /// <param name="s">The XElement to convert.</param>
    /// <returns>The Volunteer object.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
   
    static Volunteer getVolunteer(XElement s)
    {
        // קבלת הערך של distanceType כ- string ישירות מ-XML
        var distanceTypeStr = (string?)s.Element("DistanceType");
        DistanceType distanceType = DistanceType.DrivingDistance; // ערך ברירת מחדל במקרה שאין ערך

        // המרה ידנית אם קיים ערך ב- XML
        if (!string.IsNullOrEmpty(distanceTypeStr) && Enum.TryParse(distanceTypeStr, true, out DistanceType result))
        {
            distanceType = result;
        }

        // קבלת הערך של role כ- string
        var roleStr = (string?)s.Element("Role");
        Role role = Role.manager; // ערך ברירת מחדל במקרה שאין ערך

        // המרה ידנית אם קיים ערך ב- XML
        if (!string.IsNullOrEmpty(roleStr) && Enum.TryParse(roleStr, true, out Role roleResult))
        {
            role = roleResult;
        }

        return new DO.Volunteer()
        {
            Id = s.ToIntNullable("Id") ?? throw new FormatException("Can't convert Id"),
            Name = (string?)s.Element("Name") ?? "",
            PhoneNumber = (string?)s.Element("PhoneNumber") ?? "",
            Email = (string?)s.Element("Email") ?? "",
            Password = (string?)s.Element("Password") ?? "",
            Address = (string?)s.Element("Address") ?? "",
            Active = (bool?)s.Element("Active") ?? false,
            role = role,  // שמירת הערך כ- Role
            distanceType = distanceType,  // שמירת הערך כ- DistanceType
            Latitude = s.ToDoubleNullable("Latitude") ?? 0,
            Longitude = s.ToDoubleNullable("Longitude") ?? 0,
            MaxDistance = s.ToDoubleNullable("MaxDistance") ?? 0,
        };
    }



    /// <summary>
    /// Retrieves a volunteer by its ID from the data source.
    /// </summary>
    /// <param name="id">The ID of the volunteer to retrieve.</param>
    /// <returns>The Volunteer object if found, otherwise null.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Volunteer? Read(int id)
    {
        XElement? volunteerElem =
            XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml).Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        return volunteerElem is null ? null : getVolunteer(volunteerElem);
    }

    /// <summary>
    /// Retrieves a volunteer based on a custom filter function.
    /// </summary>
    /// <param name="filter">A function to filter the volunteers.</param>
    /// <returns>The first volunteer matching the filter, or null if no match is found.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml).Elements().Select(s => getVolunteer(s)).FirstOrDefault(filter);
    }

    /// <summary>
    /// Updates an existing volunteer record in the data source.
    /// </summary>
    /// <param name="item">The volunteer object with updated information.</param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Volunteer item)
    {
        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);

        (volunteersRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == item.Id)
        ?? throw new DO.DalDoesNotExistsException($"volunteers with ID={item.Id} does Not exist"))
            .Remove();

        volunteersRootElem.Add(createVolunteerElement(item));

        XMLTools.SaveListToXMLElement(volunteersRootElem, Config.s_volunteers_xml);
    }
}
