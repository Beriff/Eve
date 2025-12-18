using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI.Effects
{
    /// <summary>
    /// Encodes the way effect addition to the EffectGroup is handled based on it's name
    /// </summary>
    public enum EffectEnqueuing
    {
        /// <summary>
        /// If the effect exists, current one gets discarded
        /// </summary>
        Discard,
        /// <summary>
        /// If the effect exists, current one replaces it
        /// </summary>
        Replace,
        /// <summary>
        /// If the effect exists, current one gets added anyways
        /// </summary>
        Append
    }

    public class TimedEffect
        (Action<TimedEffect> action, string name = "", float lifespan = 1f, EffectEnqueuing behavior = EffectEnqueuing.Discard)
    {
        public string Name { get; set; } = name;
        public EffectEnqueuing QueueBehavior { get; set; } = behavior;
        public float Lifespan { get; set; } = lifespan;
        public float Lifetime { get; set; } = 0;
        public Action<TimedEffect> OnTick { get; set; } = action;
        public Action<TimedEffect> OnExpire { get; set; } = new(_ => { });

        public float Progress => Math.Clamp(Lifetime / Lifespan, 0, 1);

    }
}
