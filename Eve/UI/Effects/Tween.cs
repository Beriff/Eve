using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI.Effects
{
    public static class Tween
    {
        public static float Lerp(float a, float b, float t) => b + (b - a) * t;
        public static Color Lerp(Color a, Color b, float t)
        {
            return new Color(
                Lerp(a.R, b.R, t),
                Lerp(a.G, b.G, t),
                Lerp(a.B, b.B, t),
                Lerp(a.A, b.A, t)
            );
        }
    }
}
