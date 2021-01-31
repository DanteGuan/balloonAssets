using System;

public static class EventUtil
{
    private static EventSender<Enum, object> senders = new EventSender<Enum, object>();

    public static void AddListener(Enum eventType, Action<object> eventHandler)
    {
        senders.AddListener(eventType, eventHandler);
    }

    public static void RemoveListener(Enum eventType, Action<object> eventHandler)
    {
        senders.RemoveListener(eventType, eventHandler);
    }

    public static bool HasListener(Enum eventType)
    {
        return senders.HasListener(eventType);
    }

    public static void SendMessage(Enum eventType)
    {
        senders.SendMessage(eventType, null);
    }

    public static void SendMessage(Enum eventType, object eventArg)
    {
        senders.SendMessage(eventType, eventArg);
    }

    public static void Clear()
    {
        senders.Clear();
    }

}