using Match_3.StageComponents;
using Match_3.StageComponents.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.GameEntities.Objects
{
    abstract class Bonus
    {
        protected Texture2D bonusTexture;
        public Action Activate;

        public Bonus(string textureName)
        {
            bonusTexture = TexturePool.Get(textureName);
        }

        public abstract void Draw(SpriteBatch batch, Vector2 Position, Color Color, Vector2 Origin);
    }
}
