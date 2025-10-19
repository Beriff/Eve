using Eve.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI.Controls
{
    /// <summary>
    /// A panel control consisting of 9 tiles: 4 for corners, 4 for sides and one for the inside
    /// Each part is stretched accordingly.
    /// </summary>
    public class TilePanel : Control
    {
        public Observable<Color> PanelColor { get => field; set => field = GetLocalObservable(value.Value); } = Color.White;
        public Observable<int> BorderRadius { get => field; set => field = GetLocalObservable(value.Value); } = 10;

        protected override void DrawControl(SpriteBatch sb)
        {
            var corner = ControlResources.GetRoundedCorner(1f);

            // top left corner
            sb.Draw(corner,
                new Rectangle(0, 0, BorderRadius, BorderRadius), null, PanelColor, 0, Vector2.Zero,
                SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically, 0);

            // bottom left corner
            sb.Draw(corner,
                new Rectangle(0, (int)PixelSize.Y - BorderRadius, BorderRadius, BorderRadius), 
                null, PanelColor, 0, Vector2.Zero,
                SpriteEffects.FlipHorizontally, 0);

            // top right corner
            sb.Draw(corner,
                new Rectangle((int)PixelSize.X - BorderRadius, 0, BorderRadius, BorderRadius),
                null, PanelColor, 0, Vector2.Zero,
                SpriteEffects.FlipVertically, 0);

            // bottom right corner
            sb.Draw(
                corner, 
                new Rectangle(
                    (int)PixelSize.X - BorderRadius, 
                    (int)PixelSize.Y - BorderRadius,
                    BorderRadius, BorderRadius), 
                null,
                PanelColor
            );

            // top line
            sb.Draw(
                ControlResources.UnitTexture,
                new Rectangle(BorderRadius, 0, (int)PixelSize.X-BorderRadius*2, BorderRadius), null,
                PanelColor
                );

            // bottom line
            sb.Draw(
                ControlResources.UnitTexture,
                new Rectangle(BorderRadius, (int)PixelSize.Y - BorderRadius, (int)PixelSize.X - BorderRadius*2, BorderRadius), 
                null,
                PanelColor
                );

            // left line
            sb.Draw(
                ControlResources.UnitTexture,
                new Rectangle(0, BorderRadius, BorderRadius, (int)PixelSize.Y - BorderRadius * 2), null,
                PanelColor
                );

            // right line
            sb.Draw(
                ControlResources.UnitTexture,
                new Rectangle((int)PixelSize.X - BorderRadius, BorderRadius, BorderRadius, (int)PixelSize.Y - BorderRadius * 2), null,
                PanelColor
                );

            // middle
            sb.Draw(
                ControlResources.UnitTexture,
                new Rectangle(BorderRadius, BorderRadius, (int)PixelSize.X - BorderRadius * 2, (int)PixelSize.Y - BorderRadius * 2), 
                null,
                PanelColor
                );
        }
    }
}
