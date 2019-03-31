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
        public bool IsNewBonus = false;
        private Bonus bonus;
        private Grid parent;
        private int animSpeed = 6;
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
                if (active && !value)
                    User.Score++;

                active = value;
                if (value == false)
                {
                    ElementColor = ElementColor.Empty;
                    BonusType = BonusType.None;
                    ClearActions();
                    IsFalling = false;
                    StopAnimation();
                    bonus?.Activate();
                    bonus = null;
                }
                else
                {
                    Randomize();
                }
            }
        }

        public void Randomize()
        {
            ElementColor = colorsSet[random.Next(colorsSet.Length)];
            texture = TexturePool.Get(ElementColor.ToString());
        }

        public GameElement(Grid grid)
        {
            this.parent = grid;
            Active = true;
            Init(parent.CellSize - 2, parent.CellSize - 2, animSpeed);
        }

        public GameElement(ElementColor color, Grid grid)
        {
            this.parent = grid;
            ElementColor = color;
            texture = TexturePool.Get(ElementColor.ToString());
            Init(parent.CellSize - 2, parent.CellSize - 2, animSpeed);
        }

        public void InitLineBonus(Orientation orientation)
        {
            bonus = new LineBonus(orientation, parent, this);
            BonusType = BonusType.Line;
        }

        public void InitBombBonus()
        {
            bonus = new BombBonus(parent, this);
            BonusType = BonusType.Bomb;
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
