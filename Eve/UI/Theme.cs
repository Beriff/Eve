using FontStashSharp;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace Eve.UI
{
    public static class Theme
    {
        public static FontSystem Font;
        private static Dictionary<int, DynamicSpriteFont> FontBases;

        public static void Initialize(ContentManager content)
        {
            Font = new FontSystem();
            Font.AddFont(File.ReadAllBytes(@"UI/Resources/Fonts/OpenSans.ttf"));
        }

        public static DynamicSpriteFont GetFont(int size)
        {
            if (FontBases.TryGetValue(size, out var font)) { return font; }
            return FontBases[size] = Font.GetFont(size);
        }
    }
}
