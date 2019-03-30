using Match_3.StageComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.GameEntities.Objects
{
    class LineBonus : Bonus
    {
        
        private float lineAngle = 0;
        private Vector2 origin;

        private Orientation orientation;
        public Orientation Orientation
        {
            get => orientation;
            set
            {
                orientation = value;
                switch (orientation)
                {
                    case Orientation.Horizontal:
                        lineAngle = 0;
                        break;
                    case Orientation.Vertical:
                        lineAngle = (float)Math.PI / 2;
                        break;
                }
            }
        }

        public LineBonus(Orientation orientation) : base("LineBonus")
        {
            Orientation = orientation;
            origin = new Vector2(bonusTexture.Width/2, bonusTexture.Height/2);
        }

        public override void Draw(SpriteBatch batch, Vector2 Position, Color Color, Vector2 Origin)
        {
            batch.Draw(bonusTexture, new Vector2(Position.X + origin.X, Position.Y + origin.Y), new Rectangle(0, 0, bonusTexture.Width, bonusTexture.Height), Color, lineAngle, origin, 1, SpriteEffects.None, 1);
        }
    }
}
