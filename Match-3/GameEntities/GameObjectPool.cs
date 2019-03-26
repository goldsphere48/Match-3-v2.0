using Match_3.GameEntities.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Match_3.GameEntities.Objects.GameElement;

namespace Match_3.GameEntities
{
    class GameObjectPool
    {
        private List<GameElement> pool;

        public GameObjectPool(int poolSize)
        {
            pool = new List<GameElement>(poolSize);
        }

        public GameElement Get(ElementColor color, ElementType type, Vector2 position)
        {
            GameElement element = pool.Find(x => !x.Active && x.GameElementType == type);
            if (element == null)
                element = GameElementFactory.CreateGameElement(color, type);
            element.GameElementColor = color;
            element.Position = position;
            return element;
        }
    }
}
