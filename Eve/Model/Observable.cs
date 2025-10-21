using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eve.Model
{
    public class Observable<T>(T val)
    {
        public event Action<T> Updated = new(_ => { });
        public T Value { get => field; set { field = value; Updated.Invoke(field); } } = val;

        public static implicit operator Observable<T>(T val) { return new Observable<T>(val); }
        public static implicit operator T(Observable<T> val) { return val.Value; }

        public override string? ToString() => Value!.ToString();
    }
}
