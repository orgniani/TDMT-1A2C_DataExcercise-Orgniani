using System.Collections.Generic;
using System;
using UnityEngine;
using Singleton;

namespace Events
{
    [DisallowMultipleComponent]
    public abstract class EventManager<IdT> : MonoBehaviourSingleton<EventManager<IdT>>
    {
        private Dictionary<IdT, Action<IdT>> _events = new();

        public void InvokeEvent<T>(IdT eventIdentifier, T param)
        {
            if (_events.TryGetValue(eventIdentifier, out var eventDelegate))
            {
                if (eventDelegate is Action<T> typedDelegate)
                {
                    typedDelegate?.Invoke(param);
                    Debug.Log($"{name}: Event ({eventIdentifier}) invoked with parameter {param}");
                }

                else
                {
                    //TODO: Check if this is whats happening here -SF
                    Debug.LogError($"{name}: Event id ({eventIdentifier}) has a delegate with a different parameter type.");
                }
            }
            else
                Debug.LogWarning($"{name}: Event id ({eventIdentifier}) had no subscribers!");
        }

        public void SubscribeToEvent(IdT eventIdentifier, Action<IdT> handler)
        {
            if (!_events.TryAdd(eventIdentifier, handler))
                _events[eventIdentifier] += handler;
            else
                Debug.Log($"{name}: Event id ({eventIdentifier}) was added");
        }

        public void UnsubscribeFromEvent(IdT eventIdentifier, Action<IdT> handler)
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
