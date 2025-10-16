using Microsoft.Xna.Framework;

namespace Eve.Model
{
    public struct LayoutUnit(Vector2 rel, Vector2 abs)
    {
        public Vector2 Relative = rel;
        public Vector2 Absolute = abs;

        public static LayoutUnit FromRel(Vector2 rel) => new(rel, Vector2.Zero);
        public static LayoutUnit FromAbs(Vector2 abs) => new(Vector2.One, abs);
        public static LayoutUnit Full { get => FromRel(new(1,1)); }
        public static LayoutUnit Zero { get => FromAbs(Vector2.Zero); }
        public readonly Vector2 Normalize(Vector2 metric) => metric * Relative + Absolute;
        public LayoutUnit(float rx, float ry, float ax, float ay)
            : this(new(rx, ry), new(ax, ay)) { }
    }
}
