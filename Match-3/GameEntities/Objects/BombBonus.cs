using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match_3.GameEntities.Objects
{
    class BombBonus : Bonus
    {
        public BombBonus(Grid grid, GameElement element) : base("Bomb", grid, element)
        {

        }

        public override void Activate()
        {
            var oldState = grid.GameState;
            grid.GameState = GameState.WaitingForBombExplosionEnd;
            Task.Run(() => {
                Thread.Sleep(250);
                grid.GameState = oldState;
                Vector2 leftTop = new Vector2(element.Index.X - 1 < 0 ? 0 : element.Index.X - 1, element.Index.Y - 1 < 0 ? 0 : element.Index.Y - 1);
                Vector2 rightBottom = new Vector2(element.Index.X + 1 >= grid.Columns ? grid.Columns - 1 : element.Index.X + 1,
                                                  element.Index.Y + 1 >= grid.Rows ? grid.Rows - 1 : element.Index.Y + 1);
                for (int i = (int)leftTop.X; i <= rightBottom.X; ++i)
                {
                    for (int j = (int)leftTop.Y; j <= rightBottom.Y; ++j)
                    {
                        if (grid.Cells[i, j] != element && grid.Cells[i, j].Active)
                            grid.Cells[i, j].Active = false;
                    }
                }
            });
           
        }

        public override void Draw(SpriteBatch batch, Vector2 Position, Color Color, Vector2 Origin)
        {
            batch.Draw(bonusTexture, Position, new Rectangle(0, 0, bonusTexture.Width, bonusTexture.Height), Color, 0, Origin, 1, SpriteEffects.None, 1);
        }
    }
}
