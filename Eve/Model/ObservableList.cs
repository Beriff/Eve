using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Eve.Model
{
    public class ObservableList<T> : IEnumerable<T>
    {
        protected List<T> Values = [];
        public NamedEvent Updated = new();

        public IEnumerator<T> GetEnumerator() => Values.GetEnumerator();

        public int Count => Values.Count;

        public T this[int index] => Values[index];

        public void Add(T item) { Values.Add(item); Updated.Invoke(); }
        public void AddRange(IEnumerable<T> items) { Values.AddRange(items); Updated.Invoke(); }
        public void Remove(T item) { Values.Remove(item); Updated.Invoke(); }
        public void Clear() { Values.Clear(); Updated.Invoke(); }

        public ReadOnlyCollection<T> AsReadonly() => Values.AsReadOnly();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
