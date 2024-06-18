using System.Collections.Generic;
using System;
using UnityEngine;
using Singleton;

namespace Events
{
    public delegate void EventDelegate(params object[] args);

    [DisallowMultipleComponent]
    public abstract class EventManager<IdT> : MonoBehaviourSingleton<EventManager<IdT>>
    {
        private Dictionary<IdT, EventDelegate> _events = new();
        public void InvokeEvent(IdT eventIdentifier, params object[] args)
        {
            if (_events.TryGetValue(eventIdentifier, out var eventDelegate))
            {
                eventDelegate?.Invoke(args);
                Debug.Log($"{name}: Event ({eventIdentifier}) invoked");
            }
            else
                Debug.LogWarning($"{name}: Event id ({eventIdentifier}) had no subscribers!");
        }

        public void SubscribeToEvent(IdT eventIdentifier, EventDelegate handler)
        {
            if (!_events.TryAdd(eventIdentifier, handler))
                _events[eventIdentifier] += handler;
            else
                Debug.Log($"{name}: Event id ({eventIdentifier}) was added");
        }

        public void UnsubscribeFromEvent(IdT eventIdentifier, EventDelegate handler)
        {
            if (_events.ContainsKey(eventIdentifier))
            {
                _events[eventIdentifier] -= handler;
                if (_events[eventIdentifier] == null)
                {
                    _events.Remove(eventIdentifier);
                    Debug.Log($"{name}: Event id ({eventIdentifier}) was removed");
                }
            }
        }
    }
}
