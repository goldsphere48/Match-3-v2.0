using Match_3.StageComponents;
using Match_3.StageComponents.Actions;
using Match_3.StageComponents.Actors;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Match_3.GameEntities.Objects.GameElement;

namespace Match_3.GameEntities.Objects
{
    class Grid : Group
    {
        private const int rows = 8;
        private const int columns = 8;
        private ElementColor[] colorsSet = {
            ElementColor.Blue,
            ElementColor.Brown,
            ElementColor.Gold,
            ElementColor.Green,
            ElementColor.Purple
        };
        private Random random = new Random();

        private GameElement[,] grid = new GameElement[rows, columns];
        private GameObjectPool pool = new GameObjectPool(rows * columns);

        public Grid()
        {
            for(int i = 0; i < rows; ++i)
            {
                for(int j = 0; j < columns; ++j)
                {
                    int k = random.Next(colorsSet.Length);
                    grid[i, j] = pool.Get(colorsSet[k], ElementType.Regular, new Vector2(j * 80, i * 80));
                    grid[i, j].XIndex = j;
                    grid[i, j].YIndex = i;
                    grid[i, j].OnMouseClick = (button, x, y, self) =>
                    {
                        if (button == MouseButton.Left)
                        {
                            GameElement element = self as GameElement;
                            GameElement anotherSelectedElement = GetSelectedElement();
                            element.StartAnimation();
                            if (anotherSelectedElement != null)
                            {
                                if(CanSwap(element, anotherSelectedElement))
                                {
                                    Swap(element, anotherSelectedElement);
                                }
                                else
                                {
                                    element.StopAnimation();
                                    anotherSelectedElement.StopAnimation();
                                }
                            }
                        }
                    };
                    Childrens.Add(grid[i, j]);
                }
            }
        }

        private GameElement GetSelectedElement()
        {
            for(int i = 0; i < rows; ++i)
            {
                for(int j = 0; j < columns; ++j)
                {
                    if (grid[i, j].IsAnimated)
                        return grid[i, j];
                }
            }

            return null;
        }

        private bool CanSwap(GameElement first, GameElement second)
        {
            return ((Math.Abs(first.XIndex - second.XIndex) == 1 && first.YIndex == second.YIndex) ||
                    (Math.Abs(first.YIndex - second.YIndex) == 1 && first.XIndex == second.XIndex));
        }

        private void Swap(GameElement first, GameElement second)
        {
            first.AddAction(new MoveAction(second.Position, 4));
            first.AddAction(new RunableAction((self) => {

            }));
            second.AddAction(new MoveAction(first.Position, 4));
            second.AddAction(new RunableAction((self) => {

            }));
            first.StopAnimation();
            second.StopAnimation();
        }
    }
}
