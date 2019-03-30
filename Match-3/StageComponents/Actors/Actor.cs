using Match_3.StageComponents.Actions;
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
        public Stage Stage;
        private List<BaseAction> actions = new List<BaseAction>();

        protected bool active = true;
        public virtual bool Active
        {
            get => active;
            set
            { 
                active = value;
            }
        }

        public bool IsActionFinished
        {
            get => currentAction == null;
        }

        public void AddAction(BaseAction action)
        {
            actions.Add(action);
        }

        public void ClearActions()
        {
            currentActionIndex = 0;
            actions.Clear();
        }

        public void RefreshActions()
        {
            actions.ForEach(x => x.Refresh());
        }

        public void AddToParentStage(Actor actor)
        {
            Stage.AddActor(actor);
        }

        public virtual void Draw(SpriteBatch batch)
        {

        }

        private BaseAction currentAction;
        private int currentActionIndex = 0;
        public virtual void Update(GameTime gameTime)
        {
            if(actions.Count > currentActionIndex)
            {
                currentAction = actions[currentActionIndex];
                if (!currentAction.IsFinished(this))
                {
                    currentAction.Update(this);
                }
                else
                    currentActionIndex++;
            }
            else
            {
                currentAction = null;
            }
        }
    }
}
