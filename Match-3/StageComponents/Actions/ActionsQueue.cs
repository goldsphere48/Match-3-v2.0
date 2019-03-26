using Match_3.StageComponents.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.StageComponents.Actions
{
    class ActionsQueue : BaseAction
    {
        private List<BaseAction> actions = new List<BaseAction>();
        private BaseAction currentAction;
        private int currentActionIndex = 0;

        public BaseAction NextAction
        {
            get
            {
                if(currentActionIndex  < actions.Count)
                {
                    return actions[currentActionIndex++];
                }
                return null;
            }
        }

        public void AddAction(BaseAction action)
        {
            if (currentAction == null)
            {
                currentAction = action;
                currentActionIndex++;
            }
            action.Parent = Parent;
            actions.Add(action);
        }

        public void ClearActions()
        {
            actions.Clear();
        }

        public override void Update()
        {
            if(currentAction != null)
            {
                if (!currentAction.IsFinished())
                {
                    currentAction.Update();
                }
                else
                {
                    currentAction = NextAction;
                }
            }
        }

        public void Start()
        {
            currentActionIndex = 0;
            actions.ForEach(x => x.Refresh());
        }

        public void Stop()
        {
            currentActionIndex = actions.Count;
        }

        public override bool IsFinished()
        {
            return currentActionIndex == actions.Count;
        }
    }
}
