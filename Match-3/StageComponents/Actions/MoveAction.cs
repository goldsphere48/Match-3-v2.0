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

        private Vector2 step;
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
                step = new Vector2(direction.X * speed, direction.Y * speed);
            }
            if (checkX)
                actor.X += step.X;
            if(checkY)
                actor.Y += step.Y;
            if (Math.Abs(actor.X - vector.X) <= Math.Abs(step.X))
                actor.X = vector.X;
            if (Math.Abs(actor.Y - vector.Y) <= Math.Abs(step.Y))
                actor.Y = vector.Y;
        }
    }
}
