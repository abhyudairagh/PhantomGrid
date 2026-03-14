using System;
using System.Collections.Generic;

namespace PhantomGrid.Events
{
    public class EventBus : IEventBus
    {
        
        private readonly Dictionary<Type, List<Action<EventPayload>>> _mappedEvents = new();

        public void RegisterEvent<TPayload>(Action<TPayload> listener) where TPayload : EventPayload
        {
            var type = typeof(TPayload);

            if (!_mappedEvents.TryGetValue(type, out var handlers))
            {
                handlers = new List<Action<EventPayload>>();
                _mappedEvents[type] = handlers;
            }

            handlers.Add(eventPayload => listener((TPayload)eventPayload));
        }

        public void UnregisterEvent<TPayload>(Action<TPayload> listener) where TPayload : EventPayload
        {
            var type = typeof(TPayload);

            if (!_mappedEvents.TryGetValue(type, out var handlers))
                return;

            handlers.RemoveAll(action => action.Target == listener.Target);
        }

        public void FireEvent<TPayload>(TPayload payload) where TPayload : EventPayload
        {
            var type = payload.GetType();

            foreach (var events in _mappedEvents)
            {
                if (events.Key.IsAssignableFrom(type))
                {
                    foreach (var handler in events.Value)
                        handler(payload);
                }
            }
        }
        
    }

    public interface IEventBus
    {
        void RegisterEvent<TPayload>(Action<TPayload> action) where TPayload : EventPayload;

        void UnregisterEvent<TPayload>(Action<TPayload> action) where TPayload : EventPayload;

        void FireEvent<TPayload>(TPayload payload) where TPayload : EventPayload;
    }

    public class EventPayload
    {
    }
}