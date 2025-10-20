using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Eve.UI
{
    /// <summary>
    /// Static control resources such as textures for backgrounds and rounded corners.
    /// Generated automatically upon initial request (given <see cref="Control.MainGraphicsDevice">MainGraphicsDevice</see>
    /// was set beforehand)
    /// </summary>
    public static class ControlResources
    {
        private const int ELEMENT_TEXTURE_SIZE = 1024;
        private static Dictionary<float, Texture2D> RoundedCorners = [];

        public static Texture2D UnitTexture;
        
        static ControlResources()
        {
            // generate unit texture (a single white pixel)
            UnitTexture = new(Control.MainGraphicsDevice, 1, 1);
            UnitTexture.SetData([Color.White]);
        }

        public static Texture2D GetRoundedCorner(float radius)
        {
            if(RoundedCorners.TryGetValue(radius, out var value)) return value;

            // generate a new rounded corner texture
            Texture2D corner = new(Control.MainGraphicsDevice, ELEMENT_TEXTURE_SIZE, ELEMENT_TEXTURE_SIZE);
            Color[] data = new Color[ELEMENT_TEXTURE_SIZE * ELEMENT_TEXTURE_SIZE];

            float multiplier = radius * ELEMENT_TEXTURE_SIZE;
            float area = multiplier * multiplier;

            for (int x = 0; x < ELEMENT_TEXTURE_SIZE; x++)
            {
                for (int y = 0; y < ELEMENT_TEXTURE_SIZE; y++)
                {
                    if(x*x + y*y < area) 
                    {
                        data[x + y*ELEMENT_TEXTURE_SIZE] = Color.White; 
                    }
                    else { data[x + y * ELEMENT_TEXTURE_SIZE] = Color.Transparent; }
                }
            }

            corner.SetData(data);

            return RoundedCorners[radius] = corner;
        }
    }
}
