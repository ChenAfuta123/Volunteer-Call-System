namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

public class AssignmentImplementation : IAssignment
{
    /// <summary>
    /// Creates a new assignment and saves it to the XML file.
    /// </summary>
    public void Create(Assignment item)
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        int id = Config.NextAssignmentId;
        Assignment copy = item with { Id = id };
        Assignments.Add(copy);
        XMLTools.SaveListToXMLSerializer(Assignments, Config.s_assignments_xml);
    }

    /// <summary>
    /// Deletes an assignment by ID from the XML file.
    /// </summary>
    public void Delete(int id)
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        if (Assignments.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistsException($"Assignment with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Assignments, Config.s_assignments_xml);
    }

    /// <summary>
    /// Deletes all assignments from the XML file.
    /// </summary>
    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Assignment>(), Config.s_assignments_xml);
    }

    /// <summary>
    /// Reads an assignment from the XML file that matches the given filter.
    /// </summary>
    public Assignment? Read(Func<Assignment, bool> filter)
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        return Assignments.FirstOrDefault(item => filter(item));
    }

    /// <summary>
    /// Reads an assignment by its ID from the XML file.
    /// </summary>
    public Assignment? Read(int id)
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        if (!Assignments.Any(it => it.Id == id))
            throw new DalDoesNotExistsException($"Assignment with ID={id} does Not exist");
        return Assignments.FirstOrDefault(it => it.Id == id);
    }

    /// <summary>
    /// Reads all assignments from the XML file, with an optional filter.
    /// </summary>
    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null)
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        return filter == null
            ? Assignments.Select(item => item)
            : Assignments.Where(filter);
    }

    /// <summary>
    /// Updates an existing assignment in the XML file.
    /// </summary>
    public void Update(Assignment item)
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        if (Assignments.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistsException($"Assignment with ID={item.Id} does Not exist");
        Assignments.Add(item);
        XMLTools.SaveListToXMLSerializer(Assignments, Config.s_assignments_xml);
    }
}
