using Match_3.StageComponents.Actions.Behaviors;
using Match_3.StageComponents.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.StageComponents.Actions
{
    abstract class BaseAction
    {
        public Actor Parent;
        protected IRefreshable refreshableBehavior = new NoRefresh();

        public abstract void Update();
        public abstract bool IsFinished();
        public void Refresh()
        {
            refreshableBehavior.Refresh();
        }
    }
}
