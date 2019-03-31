using Match_3.StageComponents.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.StageComponents
{
    /// <summary>
    /// Stage is a container for actors which draw, update and provide user input to them
    /// </summary>
    class Stage : InputListener
    {
        private List<Actor> childrens = new List<Actor>();
        public List<Actor> Childrens
        {
            get => childrens;
        }

        public void AddActor(Actor actor)
        {
            if (actor is Group)
            {
                (actor as Group).Childrens.ForEach(x => AddActor(x));
            }
            if(actor.Stage == null)
                actor.Stage = this;
            childrens.Add(actor);
        }
        public void RemoveActor(Actor actor) => childrens.Remove(actor);

        public void Update(GameTime gameTime)
        {
            HandleInput();
            //childrens.ForEach(x => x.Update(gameTime));
            for(int i = 0; i < childrens.Count; ++i)
            {
                childrens[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin();
            childrens.ForEach(x => x.Draw(batch));
            batch.End();
        }

        public override void OnMouseClick(MouseButton key, float x, float y)
        {
            childrens.ForEach(actor => {
                if (actor.OnMouseClick != null && actor.Active && actor.IsHit(x, y))
                    actor.OnMouseClick(key, x, y, actor);
            });
        }
    }
}
