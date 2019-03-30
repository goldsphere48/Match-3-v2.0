using Match_3.StageComponents;
using Match_3.StageComponents.Actions;
using Match_3.StageComponents.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public readonly int Rows = 8;
        public readonly int Columns = 8;
        public List<Destroyer> Destroyers { get; private set; }
        private Action<MouseButton, float, float, Actor> elementOnClickAction;
        private GameElement[] selectedElements = new GameElement[2];
        private GameElement[,] cells;
        private GameState gameState;
        
        public GameElement[,] Cells
        {
            get => cells;
            private set { }
        }

        public bool IsDestroyersIsActive
        {
            get => Destroyers.Any(x => x.Active);
        }

        public Grid()
        {
            elementOnClickAction = (button, x, y, self) =>
            {
                if (button == MouseButton.Left && gameState == GameState.None)
                {
                    var firstSelectedElement = self as GameElement;
                    var secondSelectedElement = GetSelectedElement();
                    firstSelectedElement.StartAnimation();
                    if(secondSelectedElement != null)
                    {
                        selectedElements[0] = firstSelectedElement;
                        selectedElements[1] = secondSelectedElement;
                        if(CanSwap(firstSelectedElement, secondSelectedElement))
                        {
                            Swap(firstSelectedElement, secondSelectedElement, true);
                        }

                        firstSelectedElement.StopAnimation();
                        firstSelectedElement.BackToFirstfame();
                        secondSelectedElement.StopAnimation();
                        secondSelectedElement.BackToFirstfame();
                    }
                }
            };

            gameState = GameState.None;
            cells = new GameElement[Columns, Rows];

            /*cells[0, 0] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Green);
            cells[0, 1] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Purple);
            cells[0, 2] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Blue);
            cells[0, 3] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Green);
            cells[0, 4] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Purple);
            cells[0, 5] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Green);
            cells[0, 6] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Green);
            cells[0, 7] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Blue);

            cells[1, 0] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Green);
            cells[1, 1] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Green);
            cells[1, 2] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Blue);
            cells[1, 3] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Purple);
            cells[1, 4] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Green);
            cells[1, 5] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Green);

            cells[1, 6] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Blue);
            cells[1, 6].InitBombBonus();
            cells[1, 7] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Purple);
            cells[1, 7].InitLineBonus(Orientation.Horizontal);

            cells[2, 0] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Purple);
            cells[2, 1] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Green);
            cells[2, 2] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Gold);
            cells[2, 3] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Gold);
            cells[2, 4] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Blue);
            cells[2, 5] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Blue);
            cells[2, 6] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Green);
            cells[2, 7] = GameElementFactory.CreateGameElementWithParameters(this, ElementColor.Blue);*/

            Destroyers = new List<Destroyer>();

            do
            {
                Console.WriteLine(1);
                for (int i = 0; i < Columns; ++i)
                {
                    for (int j = 0; j < Rows; ++j)
                    {
                        cells[i, j] = GameElementFactory.CreateGameElement(this);
                        cells[i, j].Position = new Vector2(i * 80, j * 80);
                        cells[i, j].Index = new Vector2(i, j);
                        cells[i, j].OnMouseClick = elementOnClickAction;
                    }
                }
            } while (!IsMoveExist() || IsMatchExist());

            AddAllChildrens();
            //GenerateGrid();
        }

        public void SetState(GameState state)
        {
            gameState = state;
        }

        public void SpawnDestroyers(Orientation orientation, Vector2 Index)
        {
            gameState = GameState.WaitingForDestroyersAnimationEnd;
            var first = GetFreeDestroyer();
            var second = GetFreeDestroyer();
            switch (orientation)
            {
                case Orientation.Horizontal:
                    first.Activate(Direction.Left, Index);
                    second.Activate(Direction.Right, Index);
                    break;
                case Orientation.Vertical:
                    first.Activate(Direction.Up, Index);
                    second.Activate(Direction.Down, Index);
                    break;
            }
        }

        private Destroyer GetFreeDestroyer()
        {
            Destroyer destroyer;
            //if(destroyer == null)
            //{
                destroyer = new Destroyer(this);
                Destroyers.Add(destroyer);
                //AddToParentStage(destroyer);
            //}
            return destroyer;
        }

        private List<GameElement> GetEmptyElements()
        {
            return Childrens.Select(x => x as GameElement).ToList().FindAll(x => x.ElementColor == ElementColor.Empty);
        }

        private bool IsMoveExist()
        {
            int matchesCount = 1;
            int mistakes = 0;
            for (int i = 0; i < Columns - 1; ++i)
            {
                for (int j = 0; j < Rows - 1; ++j)
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
                    if (cells[i, j].ElementColor == cells[i, j + 1].ElementColor)
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

            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Columns - 1; ++j)
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
                    if (cells[j, i].ElementColor == cells[j + 1, i].ElementColor)
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

        private void RemoveMatches(List<GameElement> _matches)
        {
            if (_matches.Count == 0)
                return;
            _matches.ForEach(el => el.Active = false);
        }

        private void Swap(GameElement first, GameElement second, bool unswap)
        {
            gameState = GameState.ElementsSwap;
            first.AddAction(new MoveAction(second.Position, 4));
            second.AddAction(new MoveAction(first.Position, 4));
            first.AddAction(new RunableAction((self) => {

                SwapElementsInModel(first, second);
                if (!IsMatchExist() && unswap)
                {
                    Swap(first, second, false);
                }
                gameState = GameState.ElementsMatch;
            }));
            
        }

        private void SwapElementsInModel(GameElement first, GameElement second)
        {
            var firstPos = new Vector2(first.Index.X, first.Index.Y);
            var secondPos = new Vector2(second.Index.X, second.Index.Y);

            cells[(int)firstPos.X, (int)firstPos.Y] = second;
            cells[(int)secondPos.X, (int)secondPos.Y] = first;

            cells[(int)firstPos.X, (int)firstPos.Y].Index = firstPos;
            cells[(int)secondPos.X, (int)secondPos.Y].Index = secondPos;
        }

        private void DropElements(List<GameElement> _matches)
        {
            Childrens.ForEach(x =>
            {
                if (!_matches.Contains(x))
                {
                    var el = x as GameElement;
                    x.AddAction(new MoveAction(new Vector2(el.Position.X, Y + el.Index.Y * 80), 4));
                }
            });

            _matches.ForEach(x => {
                var el = x as GameElement;
                x.AddAction(new MoveAction(new Vector2(el.Position.X, Y + el.Index.Y * 80), 4));
            });
        }

        private void SpawnElements(List<GameElement> elements)
        {
            RebuildGrid();

            elements.ForEach(x => {
                x.Active = true;
                x.Y = Y - (elements.Count - x.Index.Y) * 80 - 80;
            });

            var maxY = elements.Max(x => x.Y);
            elements.ForEach(x => {
                x.Y += Y - maxY - 80;
            });

            DropElements(elements);
        }

        public void RebuildGrid()
        {
            for (int i = 0; i < Columns; ++i)
            {
                for (int j = 0; j < Rows; ++j)
                {
                    if (cells[i, j].ElementColor == ElementColor.Empty)
                    {
                        for (int k = j; k >= 1; k--)
                        {
                            SwapElementsInModel(cells[i, k], cells[i, k - 1]);
                        }
                    }
                }
            }
        }

        private bool IsMatchExist()
        {
            //Vertical
            int matchesCount = 1;
            for (int i = 0; i < Columns; ++i)
            {
                int j;
                for (j = 0; j < Rows - 1; ++j)
                {
                    if (cells[i, j].ElementColor == ElementColor.Empty)
                        continue;
                    if (cells[i, j].ElementColor == cells[i, j + 1].ElementColor)
                    {
                        matchesCount++;
                    }
                    else
                    {
                        if (matchesCount >= 3)
                        {
                            return true;
                        }
                        matchesCount = 1;
                    }
                }
                if (matchesCount >= 3)
                {
                    return true;
                }
                matchesCount = 1;

            }

            //Horizontal
            matchesCount = 1;
            for (int i = 0; i < Rows; ++i)
            {
                int j;
                for (j = 0; j < Columns - 1; ++j)
                {
                    if (cells[j, i].ElementColor == ElementColor.Empty)
                        continue;
                    if (cells[j, i].ElementColor == cells[j + 1, i].ElementColor)
                    {
                        matchesCount++;
                    }
                    else
                    {
                        if (matchesCount >= 3)
                        {
                            return true;
                        }
                        matchesCount = 1;
                    }
                }
                if (matchesCount >= 3)
                {
                    return true;
                }
                matchesCount = 1;
            }

            return false;
        }

        private List<GameElement> GetVerticalMatches()
        {
            List<GameElement> horizontalMatches = new List<GameElement>();
            int matchesCount = 1;
            for (int i = 0; i < Columns; ++i)
            {
                int j;
                for (j = 0; j < Rows - 1; ++j)
                {
                    if (cells[i, j].ElementColor == ElementColor.Empty)
                        continue;
                    if (cells[i, j].ElementColor == cells[i, j + 1].ElementColor)
                    {
                        matchesCount++;
                    }
                    else
                    {
                        if (matchesCount >= 3)
                        {
                            for (int k = 0; k < matchesCount; ++k)
                            {
                                horizontalMatches.Add(cells[i, j - k]);
                            }
                            SpawnBonus(horizontalMatches, matchesCount, Orientation.Vertical);
                        }
                        matchesCount = 1;
                    }
                }
                if (matchesCount >= 3)
                {
                    for (int k = 0; k < matchesCount; ++k)
                        horizontalMatches.Add(cells[i, j - k]);
                    SpawnBonus(horizontalMatches, matchesCount, Orientation.Vertical);
                }
                matchesCount = 1;

            }

            return horizontalMatches;
        }

        private List<GameElement> GetHorizontalMatches()
        {
            List<GameElement> verticalMatches = new List<GameElement>();
            int matchesCount = 1;
            for (int i = 0; i < Rows; ++i)
            {
                int j;
                for (j = 0; j < Columns - 1; ++j)
                {
                    if (cells[j, i].ElementColor == ElementColor.Empty)
                        continue;
                    if (cells[j, i].ElementColor == cells[j + 1, i].ElementColor)
                    {
                        matchesCount++;
                    }
                    else
                    {
                        if (matchesCount >= 3)
                        {
                            for (int k = 0; k < matchesCount; ++k)
                                verticalMatches.Add(cells[j - k, i]);
                            SpawnBonus(verticalMatches, matchesCount, Orientation.Horizontal);
                        }
                        matchesCount = 1;
                    }
                }
                if (matchesCount >= 3)
                {
                    for (int k = 0; k < matchesCount; ++k)
                        verticalMatches.Add(cells[j - k, i]);
                    SpawnBonus(verticalMatches, matchesCount, Orientation.Horizontal);
                }
                matchesCount = 1;
            }

            return verticalMatches;
        }

        private void SpawnBonus(List<GameElement> _matches, int matchesCount, Orientation orientation)
        {
            for (int i = 0; i < 2; ++i)
            {
                if (selectedElements[i] != null && _matches.Contains(selectedElements[i]))
                {
                    if (matchesCount == 4)
                    {
                        selectedElements[i].InitLineBonus(orientation);
                        _matches.Remove(selectedElements[i]);
                    }
                    else if (matchesCount >= 5)
                    {
                        selectedElements[i].InitBombBonus();
                        _matches.Remove(selectedElements[i]);
                    }
                }
            }
        }

        private List<GameElement> GetMatches()
        {
            List<GameElement> _matches = new List<GameElement>();
            var horizontalMatches = GetHorizontalMatches();
            var verticalMatches = GetVerticalMatches();
            var intersections = horizontalMatches.FindAll(x => verticalMatches.Contains(x));
            _matches.AddRange(horizontalMatches);
            _matches.AddRange(verticalMatches);
            _matches.RemoveAll(x => intersections.Contains(x));
            SpawnBonus(intersections, 5, Orientation.Horizontal);
            return _matches;
        }

        private float Sqr(float value)
        {
            return value * value;
        }

        private bool CanSwap(GameElement first, GameElement second)
        {
            var r = Math.Sqrt(Sqr(first.Position.X - second.Position.X) + Sqr(first.Position.Y - second.Position.Y));
            return r == 80;
        }

        private GameElement GetSelectedElement()
        {
            for (int i = 0; i < Columns; ++i)
            {
                for (int j = 0; j < Rows; ++j)
                {
                    if (cells[i, j].IsAnimated)
                        return cells[i, j];
                }
            }

            return null;
        }

        private void AddAllChildrens()
        {
            for (int i = 0; i < Columns; ++i)
            {
                for (int j = 0; j < Rows; ++j)
                {
                    Childrens.Add(cells[i, j]);
                }
            }
        }

        private void Print()
        {
            for(int i = 0; i < Columns; ++i)
            {
                for(int j = 0; j < Rows; ++j)
                {
                    Console.Write(cells[i, j].ElementColor + " ");
                }
                Console.WriteLine();
            }
        }

        private List<GameElement> GetRemovedByBonusesElements()
        {
            HashSet<GameElement> elements = new HashSet<GameElement>();
            for(int i = 0; i < Columns; ++i)
            {
                for(int j = 0; j < Rows; ++j)
                {
                    if(!cells[i, j].Active && !matches.Contains(cells[i, j]))
                        elements.Add(cells[i, j]);
                }
            }
            return elements.ToList();
        }

        private List<GameElement> matches = new List<GameElement>();

        public override void Update(GameTime gameTime)
        {
            for(int i = 0; i < Destroyers.Count; ++i)
            {
                Destroyers[i].Update(gameTime);
            }

            if(IsAllChildrensActionsFinished)
            {
                Print();
                switch (gameState)
                {
                    case GameState.ElementsMatch:
                        matches = GetMatches();
                        selectedElements[0] = null;
                        selectedElements[1] = null;
                        if (matches.Count == 0)
                        {
                            gameState = GameState.None;
                        }
                        else
                        {
                            gameState = GameState.ElementsRemove;
                        }
                        break;
                    case GameState.ElementsRemove:
                        gameState = GameState.ElementsSpawn;
                        RemoveMatches(matches);
                        var removedByBombElements = GetRemovedByBonusesElements();
                        matches.AddRange(removedByBombElements);
                        break;
                    case GameState.WaitingForDestroyersAnimationEnd:
                        if (!IsDestroyersIsActive)
                            gameState = GameState.DestroyersAnimationEnd;
                        break;
                    case GameState.DestroyersAnimationEnd:
                        var removedByDestroyersElements = GetRemovedByBonusesElements();
                        matches.AddRange(removedByDestroyersElements);
                        gameState = GameState.ElementsSpawn;
                        break;
                    case GameState.ElementsSpawn:
                        SpawnElements(matches);
                        gameState = GameState.ElementsDrop;
                        break;
                    case GameState.ElementsDrop:
                        DropElements(matches);
                        matches.Clear();
                        gameState = GameState.ElementsMatch;
                        break;
                }
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < Destroyers.Count; ++i)
            {
                Destroyers[i].Draw(batch);
            }
        }
    }
}
