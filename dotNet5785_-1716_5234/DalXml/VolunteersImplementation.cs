namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Xml.Linq;

internal class VolunteerImplementation : IVolunteer
{
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

    public void Create(Volunteer item)
    {
        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);

        Volunteer? volunteer = Read(item.Id);

        if (volunteer != null) throw new DO.DalDoesNotExistsException($"Volunteer with ID={item.Id} does not exist");


        volunteersRootElem.Add(createVolunteerElement(item));
        XMLTools.SaveListToXMLElement(volunteersRootElem, Config.s_volunteers_xml);


    }

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

    public void DeleteAll()
    {
        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);

        volunteersRootElem.Elements().Remove();

        XMLTools.SaveListToXMLElement(volunteersRootElem, Config.s_volunteers_xml);
    }

    

    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null)
    {
      
        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);
        return volunteersRootElem.Elements().Select(getVolunteer);
    }


    static Volunteer getVolunteer(XElement s)
    {
        return new DO.Volunteer()
        {
         
            Id = s.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
            Name = (string?)s.Element("Name") ?? "",
            PhoneNumber = (string?)s.Element("PhoneNumber") ?? "",
            Email = (string?)s.Element("Email") ?? "",
            Password = (string?)s.Element("Password") ?? "",
            Address = (string?)s.Element("Address") ?? "",
            Active = (bool?)s.Element("Active") ?? false,
            role = s.ToEnumNullable<Role>("role") ?? Role.volunteer,
            distanceType = s.ToEnumNullable<DistanceType>("distanceType") ?? DistanceType.AirDistance,
            Latitude = s.ToDoubleNullable("Latitude") ?? throw new FormatException("can't convert Latitude"),
            Longitude = s.ToDoubleNullable("Longitude") ?? throw new FormatException("can't convert  Longitude"),
            MaxDistance = s.ToDoubleNullable("MaxDistance") ?? throw new FormatException("can't convert MaxDistance"),
        };
    }

    public Volunteer? Read(int id)
    {
        XElement? volunteerElem =
    XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml).Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        return volunteerElem is null ? null : getVolunteer(volunteerElem);
    }

    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml).Elements().Select(s => getVolunteer(s)).FirstOrDefault(filter);
    }

    public void Update(Volunteer item)
    {
        XElement volunteersRootElem = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml);

        (volunteersRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == item.Id)
        ?? throw new DO.DalDoesNotExistsException($"volunteers with ID={item.Id} does Not exist"))
                .Remove();

        volunteersRootElem.Add(new XElement("Volunteer", createVolunteerElement(item)));

        XMLTools.SaveListToXMLElement(volunteersRootElem, Config.s_volunteers_xml);
    }

}
