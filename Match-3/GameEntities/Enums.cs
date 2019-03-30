using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.GameEntities
{
    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum ElementColor
    {
        Brown,
        Blue,
        Purple,
        Green,
        Gold,
        Empty
    }

    public enum BonusType
    {
        None,
        Line,
        Bomb
    }

    public enum GameState
    {
        ElementsSwap,
        ElementsMatch,
        ElementsRemove,
        ElementsSpawn,
        ElementsDrop,
        WaitingForDestroyersAnimationEnd,
        DestroyersAnimationEnd,
        None
    }

}
