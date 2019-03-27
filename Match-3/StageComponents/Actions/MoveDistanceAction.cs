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

        public MoveDistanceAction(Vector2 distance, float speed) : base(distance, speed)
        {

        }

        public override bool IsFinished(Actor actor)
        {
            return calculated && (startPosition.X + vector.X == actor.X && startPosition.Y + vector.Y == actor.Y);
        }

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
            }
            actor.X += direction.X * speed;
            actor.Y += direction.Y * speed;
            if (Math.Abs(startPosition.X + vector.X - actor.X) < speed)
                actor.X = startPosition.X + vector.X;
            if (Math.Abs(startPosition.Y + vector.Y - actor.Y) < speed)
                actor.Y = startPosition.Y + vector.Y;
        }
    }
}
