using System.Windows.Media;

namespace NightmareRealmPuzzle
{
    internal class CellColor
    {
        private static Brush[] BrushColors = new Brush[] {
            Brushes.White,
            Brushes.Red,
            Brushes.Green,
            Brushes.Blue,
            Brushes.Black
        };

        public static Brush GetColor(CellType type)
        {
            return BrushColors[(int)type];
        }
    }
}
