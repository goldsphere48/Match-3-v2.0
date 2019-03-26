using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.StageComponents.Actions.Behaviors
{
    class Refresh : IRefreshable
    {
        private Action action;

        public Refresh(Action action)
        {
            this.action = action;
        }

        void IRefreshable.Refresh()
        {
            action();
        }
    }
}
