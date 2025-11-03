using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI
{
    public class UIException : Exception
    {
        public UIException() { }
        public UIException(string message) : base(message) { }
    }
}
