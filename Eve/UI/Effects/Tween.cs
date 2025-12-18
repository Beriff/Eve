using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI.Effects
{
    public static class Tween
    {
        public static float Lerp(float a, float b, float t) => a + (b - a) * t;
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => new(
            Lerp(a.X, b.X, t), Lerp(a.Y, b.Y, t), Lerp(a.Z, b.Z, t)
            );
        public static Color Lerp(Color a, Color b, float t)
        {
            var av = a.ToVector3();
            var bv = b.ToVector3();
            return new Color(
                Lerp(av.X, bv.X, t),
                Lerp(av.Y, bv.Y, t),
                Lerp(av.Z, bv.Z, t)
            );
        }
    }
}
