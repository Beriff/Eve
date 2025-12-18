using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eve.UI.Effects
{
    public class EffectGroup
    {
        public readonly List<TimedEffect> Effects = [];
        public EffectGroup() { }

        public void Add(params TimedEffect[] effects)
        {
            var effectsList = effects.ToList();
            foreach (TimedEffect effect in effects)
            {
                var foundEffect = Effects.Find(e => e.Name == effect.Name);
                if (foundEffect != null)
                {
                    switch(effect.QueueBehavior)
                    {
                        case EffectEnqueuing.Append: Effects.Add(effect); break;
                        case EffectEnqueuing.Discard: break;
                        case EffectEnqueuing.Replace: 
                            Effects.Remove(foundEffect); 
                            Effects.Add(effect); 
                            break; 
                    }
                } else
                {
                    Effects.Add(effect);
                }
            }
        }

        public void Update(float dt)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                var effect = Effects[i];
                effect.OnTick(effect);
                effect.Lifetime += dt;
                if (effect.Lifetime >= effect.Lifespan) 
                {
                    effect.OnTick(effect);
                    effect.OnExpire(effect); 
                    Effects.Remove(effect); 
                }
            }
        }
    }
}
