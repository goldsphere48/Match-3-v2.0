using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match_3.GameEntities.Objects
{
    class BombBonus : Bonus
    {

        public BombBonus() : base("BombBonus")
        {

        }

        public override void Draw(SpriteBatch batch, Vector2 Position, Color Color, Vector2 Origin)
        {
            batch.Draw(bonusTexture, Position, new Rectangle(0, 0, bonusTexture.Width, bonusTexture.Height), Color, 0, Origin, 1, SpriteEffects.None, 1);
        }
    }
}
