using Match_3.StageComponents.Actions.Behaviors;
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
        private Vector2 target;
        private Vector2 direction = new Vector2();
        private float speed;
        private bool calculated = false;

        public MoveAction(Vector2 target, float speed)
        {
            this.target = target;
            this.speed = speed;

            refreshableBehavior = new Refresh(() =>
            {
                calculated = false;
            });
        }

        public override bool IsFinished()
        {
            return Parent.X == target.X && Parent.Y == target.Y;
        }

        public override void Update()
        {
            if (!calculated)
            {
                var angle = Math.Atan2(target.Y - Parent.Y, target.X - Parent.X);
                direction.X = (float)Math.Cos(angle);
                direction.Y = (float)Math.Sin(angle);
                direction.Normalize();
                calculated = true;
            }
            Parent.X += direction.X * speed;
            Parent.Y += direction.Y * speed;
            if (Math.Abs(Parent.X - target.X) < speed)
                Parent.X = target.X;
            if (Math.Abs(Parent.Y - target.Y) < speed)
                Parent.Y = target.Y;
        }
    }
}
