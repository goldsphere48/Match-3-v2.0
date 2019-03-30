using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.StageComponents
{
    class Transform
    {
        public Vector2 Position = new Vector2();
        public Vector2 Size = new Vector2();
        public Vector2 Origin = new Vector2();
        public float Angle;

        public virtual float X
        {
            get => Position.X;
            set => Position.X = value;
        }

        public virtual float Y
        {
            get => Position.Y;
            set => Position.Y = value;
        }

        public int Width
        {
            get => (int)Size.X;
            set => Size.X = value;
        }

        public int Height
        {
            get => (int)Size.Y;
            set => Size.Y = value;
        }

        public bool IsHit(float x, float y)
        {
            return (x >= X && x <= X + Width) && (y >= Y && y <= Y + Height);
        }
    }
}
