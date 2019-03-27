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
        public enum ElementColor
        {
            Brown,
            Blue,
            Purple,
            Green,
            Gold,
            Empty
        }

        public enum ElementType
        {
            Regular,
            Line,
            Bomb
        }

        public ElementColor GameElementColor;
        public ElementType GameElementType = ElementType.Regular;

        private bool active = true;
        public bool Active
        {
            get => active;
            set
            {
                if (value == false)
                    GameElementColor = ElementColor.Empty;
                active = value;
            }
        }

        public Vector2 Index = new Vector2();

        public GameElement(string texture) : base(texture, 80, 80, 4f)
        {

        }

        public override void Draw(SpriteBatch batch)
        {
            if(Active)
                base.Draw(batch);
        }

        public override void Update(GameTime gameTime)
        {
            if(Active)
                base.Update(gameTime);
        }

    }
}
