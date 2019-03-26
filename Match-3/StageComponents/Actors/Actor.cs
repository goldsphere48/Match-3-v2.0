﻿using Match_3.StageComponents.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.StageComponents.Actors
{
    class Actor : Transform
    {
        public Action<MouseButton, float, float, Actor> OnMouseClick;
        public Color Color = new Color(255, 255, 255);
        private ActionsQueue actionsQueue = new ActionsQueue();

        public void AddAction(BaseAction action)
        {
            actionsQueue.Parent = this;
            actionsQueue.AddAction(action);
        }

        public void ClearActions()
        {
            actionsQueue.ClearActions();
        }

        public virtual void Draw(SpriteBatch batch)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            actionsQueue.Update();
        }
    }
}