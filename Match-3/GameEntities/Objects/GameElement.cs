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
    class GameElement : AnimatedActor
    {
        public ElementColor ElementColor;
        public BonusType BonusType = BonusType.None;
        public bool IsFalling = false;
        public Vector2 Index = new Vector2();
        private Bonus bonus;
        private Grid parent;
        private static Random random = new Random();

        private static ElementColor[] colorsSet = {
            ElementColor.Blue,
            ElementColor.Brown,
            ElementColor.Gold,
            ElementColor.Green,
            ElementColor.Purple
        };

        public override bool Active
        {
            get => active;
            set
            {
                active = value;
                if (value == false)
                {
                    ElementColor = ElementColor.Empty;
                    BonusType = BonusType.None;
                    ClearActions();
                    IsFalling = false;
                    StopAnimation();
                    Color.A = 0;
                    bonus?.Activate?.Invoke();
                    bonus = null;
                }
                else
                {
                    Color.A = 255;
                    ElementColor = colorsSet[random.Next(colorsSet.Length)];
                    texture = TexturePool.Get(ElementColor.ToString());
                }
            }
        }

        public GameElement(Grid grid)
        {
            this.parent = grid;
            Active = true;
            Init(80, 80, 4f);
        }

        public GameElement(ElementColor color, Grid grid)
        {
            this.parent = grid;
            ElementColor = color;
            texture = TexturePool.Get(ElementColor.ToString());
            Init(80, 80, 4f);
        }

        public void InitLineBonus(Orientation orientation)
        {
            bonus = new LineBonus(orientation);
            BonusType = BonusType.Line;
            bonus.Activate = () =>
            {
                parent.SpawnDestroyers(orientation, Index);
            };
        }

        public void InitBombBonus()
        {
            bonus = new BombBonus();
            BonusType = BonusType.Bomb;
            bonus.Activate = () =>
            {
                Vector2 leftTop = new Vector2(Index.X - 1 < 0 ? 0 : Index.X - 1, Index.Y - 1 < 0 ? 0 : Index.Y - 1);
                Vector2 rightBottom = new Vector2(Index.X + 1 >= parent.Columns ? parent.Columns - 1 : Index.X + 1, 
                                                  Index.Y + 1 >= parent.Rows ? parent.Rows - 1 : Index.Y + 1);
                for(int i = (int)leftTop.X; i <= rightBottom.X; ++i)
                {
                    for(int j = (int)leftTop.Y; j <= rightBottom.Y; ++j)
                    {
                        if(parent.Cells[i, j] != this && parent.Cells[i, j].Active)
                            parent.Cells[i, j].Active = false;
                    }
                }
            };
        }

        public override void Draw(SpriteBatch batch)
        {
            if (Active)
            {
                base.Draw(batch);
                bonus?.Draw(batch, Position, Color, Origin);
            }
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

    }
}
