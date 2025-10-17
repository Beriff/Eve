using Microsoft.Xna.Framework;

namespace Eve.Model
{
    public struct LayoutUnit(Vector2 rel, Vector2 abs)
    {
        public Vector2 Relative = rel;
        public Vector2 Absolute = abs;

        public static LayoutUnit FromRel(Vector2 rel) => new(rel, Vector2.Zero);
        public static LayoutUnit FromRel(float x, float y) => new(new(x, y), Vector2.Zero);
        public static LayoutUnit FromRel(float v) => new(new(v, v), Vector2.Zero);
        public static LayoutUnit FromAbs(Vector2 abs) => new(Vector2.One, abs);
        public static LayoutUnit FromAbs(float x, float y) => new(Vector2.Zero, new(x, y));
        public static LayoutUnit FromAbs(float v) => new(Vector2.Zero, new(v, v));
        public static LayoutUnit Full { get => FromRel(1); }
        public static LayoutUnit Zero { get => new(0,0,0,0); }
        public readonly Vector2 Normalize(Vector2 metric) => metric * Relative + Absolute;
        public LayoutUnit(float rx, float ry, float ax, float ay)
            : this(new(rx, ry), new(ax, ay)) { }

        public override string ToString()
        {
            return $"R({Relative}) A({Absolute})";
        }
    }
}
