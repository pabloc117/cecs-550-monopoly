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

namespace Monopoly
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : UserControl
    {
        private enum Sides
        {
            LEFT = 0,
            RIGHT = 11,
            TOP = 0,
            BOTTOM = 11
        }
        public GameBoard()
        {
            InitializeComponent();
            for (int i = 1; i < 10; i++)
            {
                Property rp = new Property((int)Property.Location.RIGHT);
                rp.PropertyColor = Brushes.Purple;
                rp.PropertyName = "[THISISALONG TESTNAME]";
                rp.PropertyCost = 99;
                Grid.SetColumn(rp, (int)Sides.RIGHT);
                Grid.SetRow(rp, i);
                Property tp = new Property((int)Property.Location.TOP);
                tp.PropertyColor = Brushes.Purple;
                tp.PropertyName = "[THISISALONG TESTNAME]";
                tp.PropertyCost = 99;
                Grid.SetColumn(tp, i);
                Grid.SetRow(tp, (int)Sides.TOP); 
                Property lp = new Property((int)Property.Location.LEFT);
                lp.PropertyColor = Brushes.Purple;
                lp.PropertyName = "[THISISALONG TESTNAME]";
                lp.PropertyCost = 99;
                Grid.SetColumn(lp, (int)Sides.LEFT);
                Grid.SetRow(lp, i); 
                Property bp = new Property((int)Property.Location.BOTTOM);
                bp.PropertyColor = Brushes.Purple;
                bp.PropertyName = "[THISISALONG TESTNAME]";
                bp.PropertyCost = 99;
                Grid.SetColumn(bp, i);
                Grid.SetRow(bp, (int)Sides.BOTTOM);
                myBoard.Children.Add(tp);
                myBoard.Children.Add(rp);
                myBoard.Children.Add(lp);
                myBoard.Children.Add(bp);
            }
        }

        private void board_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged)
            {
                this.Width = e.NewSize.Height;
            }
        }
    }
}
