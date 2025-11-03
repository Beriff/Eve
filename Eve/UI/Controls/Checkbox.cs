using Eve.Model;
using Eve.UI.ControlModules.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI.Controls
{
    public class Checkbox : Control
    {
        public Observable<bool> Checked { get; set => field = GetLocalObservable(value.Value); }
        public Observable<Panel> Background { get; set => field = GetLocalObservable(value.Value); }
        public Observable<Panel> CheckTick { get; set => field = GetLocalObservable(value.Value); }

        public Checkbox(Panel? background = null, Panel? tick = null)
        {
            Checked = GetLocalObservable(true);

            Background = GetLocalObservable(background?.Clone() as Panel ?? 
                new Panel() 
                { 
                    Size = LayoutUnit.Full, 
                    Name = "CheckboxBackground" ,
                    PanelColor = Color.White
                }
            );
            CheckTick = GetLocalObservable(tick?.Clone() as Panel ??
                new Panel()
                {
                    Size = LayoutUnit.FromRel(.5f),
                    Origin = new Vector2(.5f),
                    Position = LayoutUnit.FromRel(.5f),
                    PanelColor = Color.Black,
                    Name = "CheckboxTick"
                }
            );
            // checkbox -> background -> tick
            Background.Value.WithChildren(CheckTick.Value);
            WithChildren(Background.Value);

            Checked.Updated += ("Checkbox_Update", v => { CheckTick.Value.Visible.Value = v; });
            Background.Value.InputModules.Add(
                new ClickInputModule(
                    tunnel: true, 
                    lmbHook: (mInfo) =>
                    {
                        Checked.Value = !Checked.Value;
                        
                    }
                )
            );
        }

        public override object Clone()
        {
            var checkbox = new Checkbox(Background, CheckTick);
            checkbox.Checked = Checked.Value;
            //CloneBaseProperties(checkbox);

            return checkbox;
        }
    }
}
