using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NightmareRealmPuzzle
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Button[,] Buttons;

        public const int BoardWidth = 5;
        public const int BoardHeight = 5;
        public const int ColorsCount = 3;

        private Button _currentSelectBtn;
        private Button _previousBtn;
        private Brush _defaultBorderColor = Brushes.White;

        public MainWindow()
        {
            InitializeComponent();
            CreateBoard();
            Timer.Initialize(TimerLbl);
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            CreateBoard();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Timer.Start();

            if (_currentSelectBtn != null)
            {
                _previousBtn = _currentSelectBtn;
                _previousBtn.BorderBrush = _defaultBorderColor;
            }

            Button button = sender as Button;

            _currentSelectBtn = button;

            _currentSelectBtn.BorderBrush = Brushes.Lime;

            if (_previousBtn == null)
                return;

            int currentCol = Grid.GetColumn(button);
            int currentRow = Grid.GetRow(button);

            int prevCol = Grid.GetColumn(_previousBtn);
            int prevRow = Grid.GetRow(_previousBtn);

            if (!CheckPosition(currentCol, currentRow, prevCol, prevRow))
            {
                _previousBtn = null;
                return;
            }

            if (!GameBoard.TryTurn(currentCol, currentRow, prevCol, prevRow))
            {
                ShowError();
            }

            _previousBtn = null;
            _currentSelectBtn.BorderBrush = _defaultBorderColor;
            _currentSelectBtn = null;

            if (GameBoard.CheckWin())
                Win();
        }

        public void FillGameBoardCell(int x, int y, Brush color, bool isClickable = true)
        {
            Button newButton = new Button
            {
                Margin = new Thickness(1),
                Background = color,
                BorderThickness = new Thickness(4),
                BorderBrush = _defaultBorderColor,
                Style = (Style)FindResource("GameButton")
            };

            GameGrid.Children.Add(newButton);
            Grid.SetColumn(newButton, x);
            Grid.SetRow(newButton, y);
            Buttons[x, y] = newButton;

            if (!isClickable)
                newButton.IsEnabled = false;
            else
                newButton.Click += Button_Click;
        }

        public void FillQuestCell(int x, Brush color)
        {
            Rectangle newRectangle = new Rectangle
            {
                Margin = new Thickness(5),
                Fill = color
            };

            QuestGrid.Children.Add(newRectangle);
            Grid.SetColumn(newRectangle, x);
            Grid.SetRow(newRectangle, 0);
        }

        private void CreateBoard()
        {
            Buttons = new Button[BoardWidth, BoardHeight];
            QuestGrid.Children.Clear();
            GameGrid.Children.Clear();

            GameBoard.Initialization(BoardWidth, BoardHeight, ColorsCount, this);
        }

        private bool CheckPosition(int sourceX, int sourceY, int targetX, int targetY)
        {
            if (sourceX == targetX)
            {
                if (sourceY == targetY)
                    return false;

                if (Math.Abs(sourceY - targetY) != 1)
                    return false;

                return true;
            }

            if (sourceY == targetY)
            {
                if (sourceX == targetX)
                    return false;

                if (Math.Abs(sourceX - targetX) != 1)
                    return false;

                return true;
            }

            return false;
        }

        private async void ShowError()
        {
            Button button = _previousBtn;

            button.BorderBrush = Brushes.Red;

            await Task.Run(() => Thread.Sleep(1000));

            button.BorderBrush = _defaultBorderColor;
        }

        public static void ChangeButtonColor(int x, int y, Brush Color)
        {
            Buttons[x, y].Background = Color;
        }

        private void Win()
        {
            foreach (Button button in Buttons)
                button.Click -= Button_Click;

            string time = Timer.GetTimeAndStop();

            MessageBox.Show($"You win!!\r\nTime spent: {time}");
        }
    }
}
