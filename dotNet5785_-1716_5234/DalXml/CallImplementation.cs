namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;

internal class CallImplementation : ICall
{
    /// <summary>
    /// Creates a new call and saves it to the XML file.
    /// </summary>
    public void Create(Call item)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        int id = Config.NextCallId;
        Call copy = item with { Id = id };
        Calls.Add(copy);
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }

    /// <summary>
    /// Deletes a call by ID from the XML file.
    /// </summary>
    public void Delete(int id)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (Calls.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistsException($"Call with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }

    /// <summary>
    /// Deletes all calls from the XML file.
    /// </summary>
    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Call>(), Config.s_calls_xml);
    }

    /// <summary>
    /// Reads a call from the XML file that matches the given filter.
    /// </summary>
    public Call? Read(Func<Call, bool> filter)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        return Calls.FirstOrDefault(item => filter(item));
    }

    /// <summary>
    /// Reads a call by its ID from the XML file.
    /// </summary>
    public Call? Read(int id)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (!Calls.Any(it => it.Id == id))
            throw new DalDoesNotExistsException($"Call with ID={id} does Not exist");
        return Calls.FirstOrDefault(it => it.Id == id);
    }

    /// <summary>
    /// Reads all calls from the XML file, with an optional filter.
    /// </summary>
    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        return filter == null
            ? Calls.Select(item => item)
            : Calls.Where(filter);
    }

    /// <summary>
    /// Updates an existing call in the XML file.
    /// </summary>
    public void Update(Call item)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (Calls.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistsException($"Call with ID={item.Id} does Not exist");
        Calls.Add(item);
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }
}
