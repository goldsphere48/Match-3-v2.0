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
        private const int elementSize = 80;

        private bool swaping = false;
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

        private Action<MouseButton, float, float, Actor> elementOnClickAction;

        public Grid()
        {

            elementOnClickAction = (button, x, y, self) =>
            {
                if (button == MouseButton.Left && !swaping)
                {
                    GameElement element = self as GameElement;
                    GameElement anotherSelectedElement = GetSelectedElement();
                    element.StartAnimation();
                    if (anotherSelectedElement != null)
                    {
                        if (CanSwap(element, anotherSelectedElement))
                        {
                            Swap(element, anotherSelectedElement, true);
                        }
                        else
                        {
                            element.StopAnimation();
                            anotherSelectedElement.StopAnimation();
                        }
                    }
                }
            };

            List<GameElement> matches;
            do
            {
                GenerateGrid();
                matches = GetMatches();

            } while (matches.Count != 0 || !IsMoveExist());

            AddAllChildrens();
        }

        private void AddAllChildrens()
        {
            for (int i = 0; i < columns; ++i)
            {
                for (int j = 0; j < rows; ++j)
                {
                    Childrens.Add(grid[i, j]);
                }
            }
        }

        private void GenerateGrid()
        {
            for (int i = 0; i < columns; ++i)
            {
                for (int j = 0; j < rows; ++j)
                {
                    int k = random.Next(colorsSet.Length);
                    grid[i, j] = pool.Get(colorsSet[k], ElementType.Regular, new Vector2(i * 80, j * 80));
                    grid[i, j].Index.X = i;
                    grid[i, j].Index.Y = j;
                    grid[i, j].OnMouseClick = elementOnClickAction;
                }
            }
        }

        private GameElement GetSelectedElement()
        {
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < columns; ++j)
                {
                    if (grid[i, j].IsAnimated)
                        return grid[i, j];
                }
            }

            return null;
        }

        private bool CanSwap(GameElement first, GameElement second)
        {
            return ((Math.Abs(first.Index.X - second.Index.X) == 1 && first.Index.Y == second.Index.Y) ||
                    (Math.Abs(first.Index.Y - second.Index.Y) == 1 && first.Index.X == second.Index.X));
        }

        private void Swap(GameElement first, GameElement second, bool tryToMatch)
        {
            swaping = true;
            Console.WriteLine(swaping);
            first.AddAction(new MoveAction(second.Position, 4));
            second.AddAction(new MoveAction(first.Position, 4));
            second.AddAction(new RunableAction((self) =>
            {
                Console.WriteLine(swaping);
                if (tryToMatch)
                {
                    TryToMatch(first, second, true);
                }
                swaping = false;
            }));

            var tmpIndex = first.Index;
            first.Index = second.Index;
            second.Index = tmpIndex;

            var tmpElement = grid[(int)first.Index.X, (int)first.Index.Y];
            grid[(int)first.Index.X, (int)first.Index.Y] = grid[(int)second.Index.X, (int)second.Index.Y];
            grid[(int)second.Index.X, (int)second.Index.Y] = tmpElement;

            first.StopAnimation();
            second.StopAnimation();
        }

        private async void TryToMatch(GameElement first, GameElement second, bool isNeedSwap)
        {
            var matches = GetMatches();
            if (matches.Count > 0)
            {
                matches.ForEach(x => x.Active = false);
                DropExistingElements();
                await Task.Run(() => {
                    while (!IsAllElementsOnGround()) ;
                });
                TryToMatch(first, second, false);
            }
            else if(isNeedSwap)
            {
                Swap(first, second, false);
            }
        }

        public void DropExistingElements()
        {
            for (int i = 0; i < columns; ++i)
            {
                for(int j = 0; j < rows - 1; ++j)
                {
                    if(grid[i, j + 1].GameElementColor == ElementColor.Empty)
                    {
                        for(int k = j; k >= 0; k--)
                        {
                            grid[i, k].AddAction(new MoveDistanceAction(new Vector2(0, 80), 4));

                            var tmpIndex = grid[i, k].Index;
                            grid[i, k].Index = grid[i, k + 1].Index;
                            grid[i, k + 1].Index = tmpIndex;

                            var tmpElement = grid[i, k];
                            grid[i, k] = grid[i, k + 1];
                            grid[i, k + 1] = tmpElement;
                        }
                    }
                }
            }
        }

        private bool IsAllElementsOnGround()
        {
            bool result = true;
            for(int i = 0; i < columns; ++i)
            {
                for(int j = 0; j < rows; ++j)
                {
                    result = result && grid[i, j].IsActionFinished;
                }
            }
            return result;
        }

        private bool IsMoveExist()
        {
            int matchesCount = 1;
            int mistakes = 0;
            for(int i = 0; i < columns; ++i)
            {
                for (int j = 0; j < rows - 1; ++j)
                {
                    if (mistakes > 1 && matchesCount < 3)
                    {
                        mistakes = 0;
                        matchesCount = 1;
                    }
                    else if(matchesCount >= 3)
                    {
                        return true;
                    }
                    if (grid[i, j].GameElementColor == grid[i, j + 1].GameElementColor)
                    {
                        matchesCount++;
                    }
                    else
                    {
                        mistakes++;
                    }
                }
            }

            matchesCount = 1;
            mistakes = 0;

            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < columns - 1; ++j)
                {
                    if (mistakes > 1 && matchesCount < 3)
                    {
                        mistakes = 0;
                        matchesCount = 1;
                    }
                    else if (matchesCount >= 3)
                    {
                        return true;
                    }
                    if (grid[j, i].GameElementColor == grid[j + 1, i].GameElementColor)
                    {
                        matchesCount++;
                    }
                    else
                    {
                        mistakes++;
                    }
                }
            }

            return false;
        }

        private List<GameElement> GetVerticalMatches()
        {
            List<GameElement> horizontalMatches = new List<GameElement>();
            int matchesCount = 1;
            for (int i = 0; i < columns; ++i)
            {
                int j;
                for (j = 0; j < rows - 1; ++j)
                {
                    if (grid[i, j].GameElementColor == ElementColor.Empty)
                        continue;
                    if (grid[i, j].GameElementColor == grid[i, j + 1].GameElementColor)
                    {
                        matchesCount++;
                    }
                    else
                    {
                        if (matchesCount >= 3)
                        {
                            for (int k = 0; k < matchesCount; ++k)
                                horizontalMatches.Add(grid[i, j - k]);
                        }
                        matchesCount = 1;
                    }
                }
                if (matchesCount >= 3)
                {
                    for (int k = 0; k < matchesCount; ++k)
                        horizontalMatches.Add(grid[i, j - k]);
                }
                matchesCount = 1;

            }

            return horizontalMatches;
        }

        private List<GameElement> GetHorizontalMatches()
        {
            List<GameElement> verticalMatches = new List<GameElement>();
            int matchesCount = 1;
            for (int i = 0; i < rows; ++i)
            {
                int j;
                for (j = 0; j < columns - 1; ++j)
                {
                    if (grid[j, i].GameElementColor == ElementColor.Empty)
                        continue;
                    if (grid[j, i].GameElementColor == grid[j + 1, i].GameElementColor)
                    {
                        matchesCount++;
                    }
                    else
                    {
                        if (matchesCount >= 3)
                        {
                            for (int k = 0; k < matchesCount; ++k)
                                verticalMatches.Add(grid[j - k, i]);
                        }
                        matchesCount = 1;
                    }
                }
                if (matchesCount >= 3)
                {
                    for (int k = 0; k < matchesCount; ++k)
                        verticalMatches.Add(grid[j - k, i]);
                }
                matchesCount = 1;

            }

            return verticalMatches;
        }

        private List<GameElement> GetMatches()
        {
            List<GameElement> matches = new List<GameElement>();
            matches.AddRange(GetHorizontalMatches());
            matches.AddRange(GetVerticalMatches());
            return matches;
        }
    }
}
