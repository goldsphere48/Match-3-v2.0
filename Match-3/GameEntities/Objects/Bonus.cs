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
        protected Grid grid;
        protected GameElement element;

        public Bonus(string textureName, Grid grid, GameElement element)
        {
            this.grid = grid;
            this.element = element;
            bonusTexture = TexturePool.Get(textureName);
        }

        public abstract void Activate();

        public abstract void Draw(SpriteBatch batch, Vector2 Position, Color Color, Vector2 Origin);
    }
}
