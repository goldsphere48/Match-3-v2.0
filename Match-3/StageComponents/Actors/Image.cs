using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.StageComponents.Actors
{
    class Image : Actor
    {
        protected Texture2D texture;

        public Image(string textureName)
        {
            texture = TexturePool.Get(textureName);
            Width = texture.Width;
            Height = texture.Height;
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, Position, new Rectangle(0, 0, texture.Width, texture.Height), Color, Angle, Origin, 1, SpriteEffects.None, 1);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle sourceRectangle)
        {
            spriteBatch.Draw(texture, Position, sourceRectangle, Color, Angle, Origin, 1, SpriteEffects.None, 1);
        }
    }
}
