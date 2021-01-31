using System;
using System.Collections.Generic;

public class EventSender<TKey, TValue>
{
    private Dictionary<TKey, Action<TValue>> events = new Dictionary<TKey, Action<TValue>>();

    public void AddListener(TKey eventType, Action<TValue> eventHandler)
    {
        Action<TValue> callbacks;
        if (events.TryGetValue(eventType, out callbacks))
        {
            events[eventType] = callbacks + eventHandler;
        }
        else
        {
            events.Add(eventType, eventHandler);
        }
    }

    public void RemoveListener(TKey eventType, Action<TValue> eventHandler)
    {
        Action<TValue> callbacks;
        if (events.TryGetValue(eventType, out callbacks))
        {
            callbacks = (Action<TValue>)Delegate.RemoveAll(callbacks, eventHandler);
            if (callbacks == null)
            {
                events.Remove(eventType);
            }
            else
            {
                events[eventType] = callbacks;
            }
        }
    }

    public bool HasListener(TKey eventType)
    {
        return events.ContainsKey(eventType);
    }

    public void SendMessage(TKey eventType, TValue eventArg)
    {
        Action<TValue> callbacks;
        if (events.TryGetValue(eventType, out callbacks))
        {
            callbacks.Invoke(eventArg);
        }
    }

    public void Clear()
    {
        events.Clear();
    }

}