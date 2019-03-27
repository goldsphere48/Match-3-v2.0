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
        public abstract void Update(Actor actor);
        public abstract bool IsFinished(Actor actor);
        public abstract void Refresh();
    }
}
