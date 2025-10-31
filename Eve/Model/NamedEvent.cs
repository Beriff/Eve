using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.Model
{
    /// <summary>
    /// An event wrapper whose event handlers are named and stored internally
    /// </summary>
    public class NamedEvent<T>
    {
        protected event Action<T> Event = new(_ => { });
        protected readonly Dictionary<string, Action<T>> Handlers = [];

        public int HandlerCount => Handlers.Count;

        public void operator += ((string name, Action<T> handler) handler_tuple)
        {
            if (Handlers.ContainsKey(handler_tuple.name)) RemoveHandler(handler_tuple.name);

            Handlers.Add(handler_tuple.name, handler_tuple.handler);
            Event += handler_tuple.handler;
        }

        protected void RemoveHandler(string name)
        {
            if (!Handlers.TryGetValue(name, out Action<T>? value)) return;

            Event -= value;
            Handlers.Remove(name);
        }

        public void operator -= (string name) => RemoveHandler(name);

        public void ClearHandlers()
        {
            foreach (var (name, _) in Handlers)
            {
                RemoveHandler(name);
            }
        }

        public void Invoke(T args)
        {
            Event.Invoke(args);
        }

        public override string ToString()
        {
            StringBuilder s = new ($"<NamedEvent>({HandlerCount}) {{");

            foreach (var handler in Handlers.Keys) s.Append($"[{handler.ToString()}] ");
            s.Append('}');
            return s.ToString();
        }
    }

    public class NamedEvent : NamedEvent<EventArgs>
    {
        public void operator +=((string name, Action handler) handler_tuple)
        {
            if (Handlers.ContainsKey(handler_tuple.name)) RemoveHandler(handler_tuple.name);

            Handlers.Add(handler_tuple.name, _ => handler_tuple.handler());
            Event += _ => handler_tuple.handler();
        }

        public void Invoke() => Invoke(new());
    }
}
