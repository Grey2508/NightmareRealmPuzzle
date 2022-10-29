using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NightmareRealmPuzzle
{
    internal static class GameBoard
    {
        public static Cell[,] GameGrid;

        private static MainWindow _mainWindow;

        private static Columns _columns;

        private static int _height;

        private static int[] _coloredCellsCount;
        private static List<Cell> _freeCells;

        private static Random _random;

        public static void Initialization(int width, int height, int colorsCount, MainWindow mainWindow)
        {
            _random = new Random();

            if (_mainWindow == null)
                _mainWindow = mainWindow;

            _height = height;

            GameGrid = new Cell[width, height];

            _columns = new Columns(width, colorsCount);
            _coloredCellsCount = new int[colorsCount];
            _freeCells = new List<Cell>();

            for (int i = 0; i < width; i++)
            {
                if (_columns[i] != CellType.Free)
                    _mainWindow.FillQuestCell(i, CellColor.GetColor((CellType)_columns[i]));
                else
                    FillBlocked(i);

                FillColumn(colorsCount + 1, i);
            }

            CheckCountColors();

            GetFilling();
        }

        private static void FillBlocked(int columnNumber)
        {
            int blockedTargetCount = _height + 1;

            for (int i = 0; i < blockedTargetCount / 2; i++)
            {
                int index = _random.Next(_height);

                if (GameGrid[columnNumber, index] != null)
                {
                    i--;
                    continue;
                }

                GameGrid[columnNumber, index] = new Cell(columnNumber, index, CellType.Block);

                _mainWindow.FillGameBoardCell(columnNumber, index, CellColor.GetColor(CellType.Block), false);
            }
        }

        private static void FillColumn(int countVariables, int columnNumber)
        {
            int i = 0;

            while (i < _height)
            {
                if (GameGrid[columnNumber, i] != null)
                {
                    i++;
                    continue;
                }

                int type = _random.Next(countVariables);

                if (type != 0)
                {
                    if (_coloredCellsCount[type - 1] >= _height)
                        continue;

                    _coloredCellsCount[type - 1]++;
                }
                else
                    _freeCells.Add(new Cell(columnNumber, i, CellType.Free));

                GameGrid[columnNumber, i] = new Cell(columnNumber, i, (CellType)type);

                _mainWindow.FillGameBoardCell(columnNumber, i, CellColor.GetColor((CellType)type));

                i++;
            }
        }

        private static void GetFilling()
        {
            for (int i = 0; i < _columns.Length; i++)
            {
                if (_columns[i] == CellType.Free)
                    continue;

                int counter = 0;

                for (int j = 0; j < _height; j++)
                    if (GameGrid[i, j].Type == _columns[i])
                        counter++;

                _coloredCellsCount[(int)_columns[i] - 1] = counter;
            }
        }

        private static void CheckCountColors()
        {
            for (int i = 0; i < _coloredCellsCount.Length; i++)
            {
                if (_coloredCellsCount[i] == _height)
                    continue;

                for (int j = _coloredCellsCount[i]; j < _height; j++)
                {
                    int index = _random.Next(_freeCells.Count);
                    Cell cell = _freeCells[index];

                    _freeCells.RemoveAt(index);

                    GameGrid[cell.X, cell.Y].ChangeType((CellType)i + 1);

                    _mainWindow.FillGameBoardCell(cell.X, cell.Y, CellColor.GetColor((CellType)i + 1));

                    _coloredCellsCount[i]++;
                }
            }
        }

        public static bool TryTurn(int sourceX, int sourceY, int targetX, int targetY)
        {
            Cell source = GameGrid[sourceX, sourceY];
            Cell target = GameGrid[targetX, targetY];

            if (source.Type != CellType.Free && target.Type != CellType.Free)
                return false;

            CellType sourceType = source.Type;

            source.ChangeType(target.Type);

            target.ChangeType(sourceType);

            if (source.Type != CellType.Free)
            {
                if (source.Type == _columns[targetX])
                    _coloredCellsCount[(int)source.Type - 1]--;

                if (source.Type == _columns[sourceX])
                    _coloredCellsCount[(int)source.Type - 1]++;
            }

            if (target.Type != CellType.Free)
            {
                if (target.Type == _columns[sourceX])
                    _coloredCellsCount[(int)target.Type - 1]--;

                if (target.Type == _columns[targetX])
                    _coloredCellsCount[(int)target.Type - 1]++;
            }

            return true;
        }

        public static bool CheckWin()
        {
            for (int i = 0; i < _coloredCellsCount.Length; i++)
                if (_coloredCellsCount[i] != _height)
                    return false;

            return true;

        }
    }
}
