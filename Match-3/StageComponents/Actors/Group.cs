using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.StageComponents.Actors
{
    class Group : Actor
    {
        private List<Actor> childrens = new List<Actor>();
        public List<Actor> Childrens
        {
            get => childrens;
        }

        public override float X
        {
            get => base.X;
            set
            {
                childrens.ForEach(x => x.X -= base.X);
                base.X = value;
                childrens.ForEach(x => x.X += base.X);
            }
        }

        public override float Y
        {
            get => base.Y;
            set
            {
                childrens.ForEach(x => x.Y -= base.Y);
                base.Y = value;
                childrens.ForEach(x => x.Y += base.Y);
            }
        }
    }
}
