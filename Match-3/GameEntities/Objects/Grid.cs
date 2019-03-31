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
    class Grid : Group, IDisposable
    {
        public readonly int Rows = 8;
        public readonly int Columns = 8;
        private readonly int swapSpeed = 6;
        private readonly int dropSpeed = 10;
        public readonly int CellSize = 82;
        public List<Destroyer> Destroyers { get; private set; }
        private List<GameElement> matches = new List<GameElement>();
        private Action<MouseButton, float, float, Actor> elementOnClickAction;
        public GameState GameState;
        private GameElement[] selectedElements = new GameElement[2];
        private GameElement[,] cells;

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
                if (button == MouseButton.Left && GameState == GameState.None)
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

            GameState = GameState.None;
            cells = new GameElement[Columns, Rows];
            Destroyers = new List<Destroyer>();

            for (int i = 0; i < Columns; ++i)
            {
                for (int j = 0; j < Rows; ++j)
                {
                    cells[i, j] = GameElementFactory.CreateGameElement(this);
                    cells[i, j].Position = new Vector2(i * CellSize, j * CellSize);
                    cells[i, j].Index = new Vector2(i, j);
                    cells[i, j].OnMouseClick = elementOnClickAction;
                }
            }

            while (IsMatchExist())
            {
                foreach(var cell in cells)
                {
                    cell.Randomize();
                }
            }

            AddAllChildrens();
        }

        private void Swap(GameElement first, GameElement second, bool unswap)
        {
            GameState = GameState.ElementsSwap;
            first.AddAction(new MoveAction(second.Position, swapSpeed));
            second.AddAction(new MoveAction(first.Position, swapSpeed));
            first.AddAction(new RunableAction((self) => {

                SwapElementsInModel(first, second);
                if (!IsMatchExist() && unswap)
                {
                    Swap(first, second, false);
                }
                GameState = GameState.ElementsMatch;
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

        public void SpawnDestroyers(Orientation orientation, Vector2 Index)
        {
            GameState = GameState.WaitingForDestroyersAnimationEnd;
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
            Destroyer destroyer = new Destroyer(this);
            Destroyers.Add(destroyer);
            return destroyer;
        }

        private void RemoveMatches(List<GameElement> _matches)
        {
            if (_matches.Count == 0)
                return;
            _matches.ForEach(el => el.Active = false);
        }

        private void DropElements(List<GameElement> _matches)
        {
            Childrens.ForEach(x =>
            {
                if (!_matches.Contains(x))
                {
                    var el = x as GameElement;
                    x.AddAction(new MoveAction(new Vector2(el.Position.X, Y + el.Index.Y * CellSize), dropSpeed));
                }
            });

            _matches.ForEach(x => {
                var el = x as GameElement;
                x.AddAction(new MoveAction(new Vector2(el.Position.X, Y + el.Index.Y * CellSize), dropSpeed));
            });
        }

        private void SpawnElements(List<GameElement> elements)
        {
            RebuildGrid();

            elements.ForEach(x => {
                x.Active = true;
                x.Y = Y - (elements.Count - x.Index.Y) * CellSize - CellSize;
            });

            var maxY = elements.Max(x => x.Y);
            elements.ForEach(x => {
                x.Y += Y - maxY - CellSize;
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

        private List<List<GameElement>> GetVerticalMatches()
        {
            List<List<GameElement>> verticalMatches = new List<List<GameElement>>();
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
                            verticalMatches.Add(new List<GameElement>());
                            for (int k = 0; k < matchesCount; ++k)
                            {
                                verticalMatches.Last().Add(cells[i, j - k]);
                            }
                        }
                        matchesCount = 1;
                    }
                }
                if (matchesCount >= 3)
                {
                    verticalMatches.Add(new List<GameElement>());
                    for (int k = 0; k < matchesCount; ++k)
                        verticalMatches.Last().Add(cells[i, j - k]);
                }
                matchesCount = 1;

            }

            return verticalMatches;
        }

        private List<List<GameElement>> GetHorizontalMatches()
        {
            List<List<GameElement>> horizontalMatches = new List<List<GameElement>>();
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
                            horizontalMatches.Add(new List<GameElement>());
                            for (int k = 0; k < matchesCount; ++k)
                                horizontalMatches.Last().Add(cells[j - k, i]);
                        }
                        matchesCount = 1;
                    }
                }
                if (matchesCount >= 3)
                {
                    horizontalMatches.Add(new List<GameElement>());
                    for (int k = 0; k < matchesCount; ++k)
                        horizontalMatches.Last().Add(cells[j - k, i]);
                }
                matchesCount = 1;
            }

            return horizontalMatches;
        }

        public bool TryToSpawnBombOnIntersection(GameElement element)
        {
            for(int i = 0; i < 2; ++i)
            {
                if(selectedElements[i] != null && selectedElements[i] == element)
                {
                    selectedElements[i].InitBombBonus();
                    selectedElements[i].IsNewBonus = true;
                    return true;
                }
            }
            return true;
        }

        public bool TryToSpawnBomb(List<GameElement> elements)
        {
            for (int i = 0; i < 2; ++i)
            {
                if (selectedElements[i] != null && elements.Contains(selectedElements[i]))
                {
                    selectedElements[i].InitBombBonus();
                    selectedElements[i].IsNewBonus = true;
                    return true;
                }
            }
            return true;
        }

        public bool TryToSpawnLine(List<GameElement> elements, Orientation orientation)
        {
            for (int i = 0; i < 2; ++i)
            {
                if (selectedElements[i] != null && elements.Contains(selectedElements[i]))
                {
                    selectedElements[i].InitLineBonus(orientation);
                    selectedElements[i].IsNewBonus = true;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get mathces and spawn new bonuses 
        /// </summary>
        /// <returns>List of game elemements which need to destroy</returns>
        private List<GameElement> GetMatches()
        {
            HashSet<GameElement> _matches = new HashSet<GameElement>();
            var horizontalMatches = GetHorizontalMatches();
            var verticalMatches = GetVerticalMatches();
            var intersections = horizontalMatches.FindAll(x => verticalMatches.Contains(x));

            foreach(var horizontalMatch in horizontalMatches)
            {
                horizontalMatch.ForEach(x => _matches.Add(x));
                if(horizontalMatch.Count == 4)
                {
                    TryToSpawnLine(horizontalMatch, Orientation.Horizontal);
                }
                else if(horizontalMatch.Count == 5)
                {
                    TryToSpawnBomb(horizontalMatch);
                }
            }

            foreach(var verticalMatch in verticalMatches)
            {
                verticalMatch.ForEach(x => _matches.Add(x));
                if (verticalMatch.Count == 4)
                {
                    TryToSpawnLine(verticalMatch, Orientation.Vertical);
                }
                else if (verticalMatch.Count == 5)
                {
                    TryToSpawnBomb(verticalMatch);
                }
            }

            foreach(var horizontalMatch in horizontalMatches)
            {
                foreach(var verticalMatch in verticalMatches)
                {
                    var intersectionElement = horizontalMatch.Find(x => verticalMatch.Contains(x) && (x.BonusType == BonusType.None || x.IsNewBonus));
                    if (intersectionElement != null)
                    {
                        TryToSpawnBombOnIntersection(intersectionElement);
                    }
                }
            }
            var matchesList = _matches.ToList();
            var bonusesToRemoveFromMatchesList = matchesList.FindAll(x => x.IsNewBonus);
            bonusesToRemoveFromMatchesList.ForEach(x =>
            {
                x.IsNewBonus = false;
                matchesList.Remove(x);
            });
            return matchesList;
        }

        private float Sqr(float value)
        {
            return value * value;
        }

        private bool CanSwap(GameElement first, GameElement second)
        {
            var r = Math.Sqrt(Sqr(first.Position.X - second.Position.X) + Sqr(first.Position.Y - second.Position.Y));
            return r == CellSize;
        }

        private GameElement GetSelectedElement()
        {
            foreach(var cell in cells)
            {
                if (cell.IsAnimated)
                    return cell;
            }
            return null;
        }

        private void AddAllChildrens()
        {
            foreach (var cell in cells)
            {
                Childrens.Add(cell);
            }
        }

        private List<GameElement> GetRemovedByBonusesElements()
        {
            HashSet<GameElement> elements = new HashSet<GameElement>();
            foreach (var cell in cells)
            {
                if (!cell.Active && !matches.Contains(cell))
                    elements.Add(cell);
            }
            return elements.ToList();
        }

        public override void Update(GameTime gameTime)
        {
            for(int i = 0; i < Destroyers.Count; ++i)
            {
                Destroyers[i].Update(gameTime);
            }

            if(IsAllChildrensActionsFinished)
            {
                switch (GameState)
                {
                    case GameState.ElementsMatch:
                        matches = GetMatches();
                        selectedElements[0] = null;
                        selectedElements[1] = null;
                        if (matches.Count == 0)
                        {
                            GameState = GameState.None;
                        }
                        else
                        {
                            GameState = GameState.ElementsRemove;
                        }
                        break;
                    case GameState.ElementsRemove:
                        GameState = GameState.ElementsSpawn;
                        RemoveMatches(matches);
                        var removedByBombElements = GetRemovedByBonusesElements();
                        matches.AddRange(removedByBombElements);
                        break;
                    case GameState.WaitingForDestroyersAnimationEnd:
                        if (!IsDestroyersIsActive)
                            GameState = GameState.DestroyersAnimationEnd;
                        break;
                    case GameState.DestroyersAnimationEnd:
                        var removedByDestroyersElements = GetRemovedByBonusesElements();
                        matches.AddRange(removedByDestroyersElements);
                        GameState = GameState.ElementsSpawn;
                        break;
                    case GameState.ElementsSpawn:
                        SpawnElements(matches);
                        GameState = GameState.ElementsDrop;
                        break;
                    case GameState.ElementsDrop:
                        DropElements(matches);
                        matches.Clear();
                        GameState = GameState.ElementsMatch;
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

        public void Dispose()
        {
            Childrens.Clear();
            Destroyers.Clear();
            selectedElements[0] = null;
            selectedElements[1] = null;
            cells = null;
        }
    }
}
