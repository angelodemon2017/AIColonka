using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static readonly Dictionary<Type, List<Action<IEvent>>> __subscribers = new Dictionary<Type, List<Action<IEvent>>>();
    private static readonly Dictionary<Type, List<IEventListener>> _subscribers = new Dictionary<Type, List<IEventListener>>();

    public static void Subscribe<TEvent>(IEventListener listener) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        if (!_subscribers.ContainsKey(eventType))
        {
            _subscribers[eventType] = new List<IEventListener>();
        }

        _subscribers[eventType].Add(listener);
    }

    // Подписка на событие
    public static void Subscribe<T>(Action<T> callback) where T : IEvent
    {
        if (!__subscribers.ContainsKey(typeof(T)))
        {
            __subscribers[typeof(T)] = new List<Action<IEvent>>();
        }

        // Оборачиваем в делегат, чтобы привести тип
        __subscribers[typeof(T)].Add(e => callback((T)e));
    }

    public static void Unsubscribe<TEvent>(IEventListener listener) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        if (_subscribers.ContainsKey(eventType))
        {
            _subscribers[eventType].Remove(listener);
        }
    }

    // Отписка от события
    public static void Unsubscribe<T>(Action<T> callback) where T : IEvent
    {
        if (__subscribers.ContainsKey(typeof(T)))
        {
            __subscribers[typeof(T)].Remove(e => callback((T)e));
        }
    }

    internal static void ResetSubs()
    {
        __subscribers.Clear();
    }

    internal static void CheckSubs()
    {
        Debug.LogWarning($"CheckSubs:{__subscribers.Count}");
        foreach (var sub in __subscribers)
        {
            Debug.LogWarning($"CheckUnderSubs:{sub.Value.Count}");
        }
    }

    // Отправка события
    public static void Publish<T>(T eventToPublish) where T : IEvent
    {
        if (__subscribers.ContainsKey(eventToPublish.GetType()))
        {
            foreach (var subscriber in __subscribers[eventToPublish.GetType()])
            {
                subscriber(eventToPublish);
            }
        }
    }

    public static void Publish(IEvent eventItem)
    {
        var eventType = eventItem.GetType();
        if (_subscribers.ContainsKey(eventType))
        {
            foreach (var subscriber in _subscribers[eventType])
            {
                subscriber.OnEvent(eventItem);
            }
        }
    }
}