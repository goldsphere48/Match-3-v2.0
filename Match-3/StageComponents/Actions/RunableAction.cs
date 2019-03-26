using Match_3.StageComponents.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.StageComponents.Actions
{
    class RunableAction : BaseAction
    {
        private bool done = false;
        private Action<Actor> action;

        public RunableAction(Action<Actor> action)
        {
            this.action = action;
        }

        public override bool IsFinished()
        {
            return done;
        }

        public override void Update()
        {
            if (!done)
            {
                action(Parent);
                done = true;
            }
        }
    }
}
