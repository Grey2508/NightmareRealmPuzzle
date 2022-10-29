using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NightmareRealmPuzzle
{
    internal class Cell
    {
        public CellType Type
        {
            get;
            private set;
        }

        public int X
        {
            get;
            private set;
        }
        public int Y
        {
            get;
            private set;
        }

        public Cell(int x, int y, CellType type)
        {
            X = x;
            Y = y;
            Type = type;
        }
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Cell(Cell cell)
        {
            X = cell.X;
            Y = cell.Y;
            Type = cell.Type;
        }

        public void ChangeType(CellType targetType)
        {
            Type = targetType;
            MainWindow.ChangeButtonColor(X, Y, CellColor.GetColor(targetType));
        }
    }
}
