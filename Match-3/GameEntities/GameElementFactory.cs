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
        public static GameElement CreateGameElementWithParameters(Grid parent, ElementColor color, BonusType type = BonusType.None)
        {
            var element = new GameElement(color, parent);
            element.BonusType = type;
            return element;
        }

        public static GameElement CreateGameElement(Grid parent)
        {
            return new GameElement(parent);
        }
    }
}
