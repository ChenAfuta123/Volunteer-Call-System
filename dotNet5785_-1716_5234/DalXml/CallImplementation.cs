
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;

internal class CallImplementation : ICall

{
    public void Create(Call item)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (Calls.Any(it => it.Id == item.Id))
            throw new DalAlreadyExistsException($"Call with ID={item.Id} already exist");
        Calls.Add(item);
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }

    public void Delete(int id)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (Calls.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistsException($"Call with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }

    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Call>(), Config.s_calls_xml);
    }

    public Call? Read(Func<Call, bool> filter)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        return Calls.FirstOrDefault(item => filter(item));
    }

    public Call? Read(int id)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (!Calls.Any(it => it.Id == id))
            throw new DalDoesNotExistsException($"Call with ID={id} does Not exist");
        return Calls.FirstOrDefault(it => it.Id == id);
    }

    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        return filter == null
            ? Calls.Select(item => item)
            : Calls.Where(filter);
    }
    public void Update(Call item)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (Calls.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistsException($"Call with ID={item.Id} does Not exist");
        Calls.Add(item);
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }
}
