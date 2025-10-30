using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Eve.UI
{
    /// <summary>
    /// <para>
    /// Represents a class that generates instances of a defined composite control (a subtree).
    /// 
    /// For example, a button is a composite control: it is a label parented to a panel, with
    /// hovering and clicking event handling. A ButtonFactory would allow hooking to
    /// click events and changing the label.
    /// </para>
    /// </summary>
    public class CompositeControlBlueprint(Control root)
    {
        protected Control CompositeControlRoot = root;
        public event Action<Control> Instantiated = new(_ => { });
        public Control GetInstance() 
        {
            var root = (CompositeControlRoot.Clone() as Control)!;
            Instantiated.Invoke(root);
            return root; 
        }
    }

    /// <summary>
    /// Represents a class that creates an <see cref="CompositeControlBlueprint"/> for further
    /// instantiation of composite controls.
    /// </summary>
    public interface IControlAbstractFactory
    {
        public CompositeControlBlueprint GetBlueprint();
    }
}
