using Match_3.StageComponents;
using Match_3.StageComponents.Actions;
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
    class Destroyer : Image
    {
        private Grid parent;
        private float rotateSpeed = (float)Math.PI / 10f;
        private readonly int speed = 14;
        public Destroyer(Grid parent)
        {
            this.parent = parent;
            texture = TexturePool.Get("Destroyer");
            active = true;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public void Activate(Direction direction, Vector2 Index)
        {
            active = true;
            X = parent.X + Index.X * parent.CellSize + Origin.X;
            Y = parent.Y + Index.Y * parent.CellSize + Origin.Y;
            Vector2 vDirection = new Vector2();
            switch (direction)
            {
                case Direction.Up:
                    vDirection.Y = -parent.CellSize;
                    break;
                case Direction.Down:
                    vDirection.Y = parent.CellSize;
                    break;
                case Direction.Left:
                    vDirection.X = -parent.CellSize;
                    break;
                case Direction.Right:
                    vDirection.X = parent.CellSize;
                    break;
            }
            switch (direction)
            {
                case Direction.Left:
                    for (int i = (int)Index.X; i >= 0; --i)
                    {
                        var index = i;
                        AddAction(new MoveDistanceAction(vDirection, speed, false));
                        AddAction(new RunableAction((self) => {
                            parent.Cells[index, (int)Index.Y].Active = false;
                        }));
                    }
                    break;
                case Direction.Right:
                    for (int i = (int)Index.X; i < parent.Columns; ++i)
                    {
                        var index = i;
                        AddAction(new MoveDistanceAction(vDirection, speed, false));
                        AddAction(new RunableAction((self) => {
                            parent.Cells[index, (int)Index.Y].Active = false;
                        }));
                    }
                    break;
                case Direction.Up:
                    for (int i = (int)Index.Y; i >= 0; --i)
                    {
                        var index = i;
                        AddAction(new MoveDistanceAction(vDirection, speed, false));
                        AddAction(new RunableAction((self) => {
                            parent.Cells[(int)Index.X, index].Active = false;
                        }));
                    }
                    break;
                case Direction.Down:
                    for (int i = (int)Index.Y; i < parent.Rows; ++i)
                    {
                        var index = i;
                        AddAction(new MoveDistanceAction(vDirection, speed, false));
                        AddAction(new RunableAction((self) =>
                        {
                            parent.Cells[(int)Index.X, index].Active = false;
                        }));
                    }
                    break;
            }

           
            AddAction(new RunableAction((self) =>
            {
                Deactivate();
            }));
        }

        public void Deactivate()
        {
            ClearActions();
            active = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (active)
            {
                base.Update(gameTime);
                Angle += rotateSpeed;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if(active)
                base.Draw(batch);
        }
    }
}
