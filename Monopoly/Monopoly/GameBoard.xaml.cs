using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace Monopoly
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : UserControl
    {
        private Dictionary<int, PropertyListing> _listings;
        public enum Side
        {
            LEFT = 0,
            RIGHT = 11,
            TOP = 0,
            BOTTOM = 11,
            UNKNOWN = -1
        }
        public GameBoard()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(GameBoard_Loaded);
        }

        void GameBoard_Loaded(object sender, RoutedEventArgs e)
        {
            ThreadStart job = new ThreadStart(BuildBoard);
            Thread work = new Thread(job);
            work.IsBackground = true;
            work.Start();
        }

        private void board_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged)
            {
                this.Width = e.NewSize.Height;
            }
        }

        private void BuildBoard()
        {
            _listings = ThemeParser.GetPropertyListing();
            for (int i = 1; i < 10; i++)
            {
                foreach (PropertyListing p in _listings.Values)
                {
                    MakeProperty(p);
                }
            }
            OnGameBuilt(new GameBoardBuiltEventArgs());
        }

        private void MakeProperty(PropertyListing p)
        {
            if (Dispatcher.CheckAccess())
            {
                Property pr = new Property((int)p.Side);
                pr.PropertyColor = p.ColorGroup;
                pr.PropertyCost = p.Cost;
                pr.PropertyName = p.Name;
                switch (p.Side)
                {
                    case Property.Side.TOP:
                        Grid.SetColumn(pr, p.Location % 10);
                        Grid.SetRow(pr, (int)Side.TOP);
                        break;
                    case Property.Side.RIGHT:
                        Grid.SetColumn(pr, (int)Side.RIGHT);
                        Grid.SetRow(pr, p.Location % 10);
                        break;
                    case Property.Side.BOTTOM:
                        Grid.SetColumn(pr, 10 - p.Location % 10);
                        Grid.SetRow(pr, (int)Side.BOTTOM);
                        break;
                    case Property.Side.LEFT:
                        Grid.SetColumn(pr, (int)Side.LEFT);
                        Grid.SetRow(pr, 10 - p.Location % 10);
                        break;
                }
                myBoard.Children.Add(pr);
            }
            else Dispatcher.BeginInvoke(new Action<PropertyListing>(MakeProperty), new object[] { p });
        }

        public event EventHandler<GameBoardBuiltEventArgs> GameBuilt;
        private void OnGameBuilt(GameBoardBuiltEventArgs e)
        {
            GameBuilt(this, e);
        }

    }

    public class GameBoardBuiltEventArgs : EventArgs
    {
        public GameBoardBuiltEventArgs()
        { }
    }
}
