using Match_3.StageComponents;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.StageComponents
{
    abstract class InputListener
    {
        private MouseState oldState = Mouse.GetState();

        public void HandleInput()
        {
            MouseState newState = Mouse.GetState();
            if (newState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed)
            {
                OnMouseClick(MouseButton.Left, newState.Position.X, newState.Position.Y);
            }
            if (newState.MiddleButton == ButtonState.Released && oldState.MiddleButton == ButtonState.Pressed)
            {
                OnMouseClick(MouseButton.Middle, newState.Position.X, newState.Position.Y);
            }
            if (newState.RightButton == ButtonState.Released && oldState.RightButton == ButtonState.Pressed)
            {
                OnMouseClick(MouseButton.Right, newState.Position.X, newState.Position.Y);
            }
            oldState = newState;
        }

        public abstract void OnMouseClick(MouseButton key, float x, float y);
    }
}
