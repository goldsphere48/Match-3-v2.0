using Match_3.GameEntities.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Match_3.GameEntities.Objects.GameElement;

namespace Match_3.GameEntities
{
    static class GameElementFactory
    {
        public static GameElement CreateGameElement(ElementColor color, ElementType type)
        {
            string name = color.ToString() + type.ToString();
            GameElement element = new GameElement(name);
            element.GameElementColor = color;
            element.GameElementType = type;
            return element;
        }
    }
}
