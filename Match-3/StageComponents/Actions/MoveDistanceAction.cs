using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match_3.StageComponents.Actors;
using Microsoft.Xna.Framework;

namespace Match_3.StageComponents.Actions
{
    class MoveDistanceAction : MoveAction
    {
        private Vector2 startPosition;
        private bool isNeedAccuracy = true;
        private bool xChecked = false;
        private bool yChecked = false;

        public MoveDistanceAction(Vector2 distance, float speed, bool isNeedAccuracy = true) : base(distance, speed)
        {
            this.isNeedAccuracy = isNeedAccuracy;
        }

        public override bool IsFinished(Actor actor)
        {
            return calculated && (xChecked && yChecked);
        }

        private Vector2 step;
        public override void Update(Actor actor)
        {
            if (!calculated)
            {
                var angle = Math.Atan2(vector.Y, vector.X);
                direction.X = (float)Math.Cos(angle);
                direction.Y = (float)Math.Sin(angle);
                direction.Normalize();
                calculated = true;
                startPosition = actor.Position;
                step = new Vector2(direction.X * speed, direction.Y * speed);
            }
           
            actor.X += step.X;
            actor.Y += step.Y;
            if (!xChecked && Math.Abs(startPosition.X + vector.X - actor.X) <= Math.Abs(step.X))
            {
                xChecked = true;
                if (!isNeedAccuracy)
                {
                    return;
                }
                actor.X = startPosition.X + vector.X;
            }
            else if (!yChecked && Math.Abs(startPosition.Y + vector.Y - actor.Y) <= Math.Abs(step.Y))
            {
                yChecked = true;
                if (!isNeedAccuracy)
                {
                    return;
                }
                actor.Y = startPosition.Y + vector.Y;
            }
        }
    }
}
