using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, Action<Dictionary<string, object>>> eventDictionary;

    private static EventManager eventManager;

    public enum Event
    {
        onStartGame,
        onReset
    }

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject");
                } else
                {
                    eventManager.Init();
                    // Sets this to not be destroyed when reloading scene
                    DontDestroyOnLoad(eventManager);
                }
            }
            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<Dictionary<string, object>>>();
        }
    }

    public static void StartListening(Event eventName, Action<Dictionary<string, object>> listener)
    {
        if (instance.eventDictionary.TryGetValue(GetEventValue(eventName), out Action<Dictionary<string, object>> thisEvent))
        {
            thisEvent += listener;
            instance.eventDictionary[GetEventValue(eventName)] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            instance.eventDictionary.Add(GetEventValue(eventName), thisEvent);
        }
    }

    public static void StopListening(Event eventName, Action<Dictionary<string, object>> listener)
    {
        if (eventManager == null) return;
        if (instance.eventDictionary.TryGetValue(GetEventValue(eventName), out Action<Dictionary<string, object>> thisEvent))
        {
            thisEvent -= listener;
            instance.eventDictionary[GetEventValue(eventName)] = thisEvent;
        }
    }

    public static void TriggerEvent(Event eventName, Dictionary<string, object> message)
    {
        if (instance.eventDictionary.TryGetValue(GetEventValue(eventName), out Action<Dictionary<string, object>> thisEvent))
        {
            thisEvent.Invoke(message);
        }
    }

    private static string GetEventValue(Event eventValue)
    {
        return eventValue switch
        {
            Event.onStartGame => "onStartGame",
            Event.onReset => "onReset",
            _ => "",
        };
    }
}
