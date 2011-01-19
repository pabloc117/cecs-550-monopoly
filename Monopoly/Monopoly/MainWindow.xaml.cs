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
using System.Collections;

namespace Monopoly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point _dim;
        private Point _loc;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                if (this.WindowState != System.Windows.WindowState.Normal)
                    Restore();
                else
                    Maximize();
                
            }
            else if (e.Key == Key.Escape)
                Restore();
        }
        
        private void Maximize()
        {
            if (this.Dispatcher.CheckAccess())
            {
                _dim = new Point(this.Width, this.Height);
                _loc = new Point(this.Left, this.Top);
                this.WindowState = System.Windows.WindowState.Maximized;
                this.WindowStyle = System.Windows.WindowStyle.None;
            }
            else this.Dispatcher.BeginInvoke(new Action(Maximize));
        }

        public void Restore()
        {
            if (this.Dispatcher.CheckAccess())
            {
                this.Left = _loc.X;
                this.Top = _loc.Y;
                this.Width = _dim.X;
                this.Height = _loc.Y;
                this.WindowState = System.Windows.WindowState.Normal;
                this.WindowStyle = System.Windows.WindowStyle.ThreeDBorderWindow;
            }
            else this.Dispatcher.BeginInvoke(new Action(Restore));
        }
    }
}
