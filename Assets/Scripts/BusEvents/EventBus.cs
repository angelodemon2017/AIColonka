using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static readonly Dictionary<Type, List<Action<IEvent>>> _subscribers = new Dictionary<Type, List<Action<IEvent>>>();

    // Подписка на событие
    public static void Subscribe<T>(Action<T> callback) where T : IEvent
    {
        if (!_subscribers.ContainsKey(typeof(T)))
        {
            _subscribers[typeof(T)] = new List<Action<IEvent>>();
        }

        // Оборачиваем в делегат, чтобы привести тип
        _subscribers[typeof(T)].Add(e => callback((T)e));
    }

    // Отписка от события
    public static void Unsubscribe<T>(Action<T> callback) where T : IEvent
    {
        if (_subscribers.ContainsKey(typeof(T)))
        {
            _subscribers[typeof(T)].Remove(e => callback((T)e));
        }
    }

    internal static void ResetSubs()
    {
        _subscribers.Clear();
    }

    internal static void CheckSubs()
    {
        Debug.LogWarning($"CheckSubs:{_subscribers.Count}");
        foreach (var sub in _subscribers)
        {
            Debug.LogWarning($"CheckUnderSubs:{sub.Value.Count}");
        }
    }

    // Отправка события
    public static void Publish<T>(T eventToPublish) where T : IEvent
    {
        if (_subscribers.ContainsKey(eventToPublish.GetType()))
        {
            foreach (var subscriber in _subscribers[eventToPublish.GetType()])
            {
                subscriber(eventToPublish);
            }
        }
    }
}