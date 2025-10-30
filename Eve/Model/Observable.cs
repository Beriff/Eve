using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eve.Model
{
    public class Observable<T>(T val)
    {
        public NamedEvent<T> Updated = new();
        private T _Value = val;
        public T Value { get => _Value; set { _Value = value; Updated.Invoke(_Value); } }

        public void QuietSet(T value) { _Value = value; }
        public void Update() { Updated.Invoke(_Value); }

        public static implicit operator Observable<T>(T val) { return new Observable<T>(val); }
        public static implicit operator T(Observable<T> val) { return val.Value; }

        public override string? ToString() => Value!.ToString();
    }
}
