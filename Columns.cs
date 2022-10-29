using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightmareRealmPuzzle
{
    internal class Columns
    {
        private CellType[] _columnTypes;

        public int Length
        {
            get { return _columnTypes.Length; }
        }

        private Random _random;

        public Columns(int count, int colorsCount)
        {
            _columnTypes = new CellType[count];

            _random = new Random();

            for (int i = 1; i < colorsCount + 1; i++)
            {
                SetTypeToRandomEmptyColumn((CellType)i, count);
            }
            //SetTypeToRandomEmptyColumn(ColumnType.Red, count);
            //SetTypeToRandomEmptyColumn(ColumnType.Green, count);
            //SetTypeToRandomEmptyColumn(ColumnType.Blue, count);
        }

        public CellType this[int key]
        {
            get => GetValue(key);
        }

        private CellType GetValue(int key)
        {
            return _columnTypes[key];
        }

        public bool IsFree(int index)
        {
            return _columnTypes[index].Equals(CellType.Free);
        }

        private void SetTypeToRandomEmptyColumn(CellType columnType, int maxValue)
        {
            int index = _random.Next(maxValue);

            if (_columnTypes[index] == CellType.Free)
                _columnTypes[index] = columnType;
            else
                SetTypeToRandomEmptyColumn(columnType, maxValue);
        }
    }
}
