using Match_3.StageComponents.Actors;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.StageComponents.Actions
{
    class MoveAction : BaseAction
    {
        protected Vector2 vector;
        protected Vector2 direction = new Vector2();
        protected float speed;
        protected bool calculated = false;
        private bool checkX;
        private bool checkY;

        public MoveAction(Vector2 target, float speed)
        {
            this.vector = target;
            this.speed = speed;
        }

        public override bool IsFinished(Actor actor)
        {
            return calculated && ((actor.X == vector.X && actor.Y == vector.Y) || (checkX && checkY));
        }

        public override void Refresh()
        {
            calculated = false;
        }

        public override void Update(Actor actor)
        {
            if (!calculated)
            {
                var angle = Math.Atan2(vector.Y - actor.Y, vector.X - actor.X);
                direction.X = (float)Math.Cos(angle);
                direction.Y = (float)Math.Sin(angle);
                direction.Normalize();
                calculated = true;
                checkX = vector.X != actor.X;
                checkY = vector.Y != actor.Y;
            }
            if(checkX)
                actor.X += direction.X * speed;
            if(checkY)
                actor.Y += direction.Y * speed;
            if (Math.Abs(actor.X - vector.X) < speed)
                actor.X = vector.X;
            if (Math.Abs(actor.Y - vector.Y) < speed)
                actor.Y = vector.Y;
        }
    }
}
