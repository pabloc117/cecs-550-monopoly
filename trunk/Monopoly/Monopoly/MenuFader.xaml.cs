using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows;
using System;

namespace Monopoly
{
    /// <summary>
    /// Interaction logic for MenuFader.xaml
    /// </summary>
    public partial class MenuFader : UserControl
    {
        private bool expanded = false;
        Canvas parent;

        public MenuFader(Canvas parent)
        {
            InitializeComponent();
            this.parent = parent;
            this.SetBinding(HeightProperty, "parent.Height");
            this.SetBinding(WidthProperty, "parent.Width");
            Canvas.SetBottom(this, parent.Height - 5);
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!expanded)
            {
                Expand();
            }
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Close();
        }

        public void Expand()
        {
            Storyboard sb = new Storyboard();
            DoubleAnimation da = new DoubleAnimation(Canvas.GetBottom(this), parent.Height - myGrid.Height, new System.Windows.Duration(new System.TimeSpan(0, 0, 0, 0, 250)));
            Storyboard.SetTarget(da, this);
            Storyboard.SetTargetName(da, this.Name);
            Storyboard.SetTargetProperty(da, new PropertyPath(Canvas.BottomProperty));
            sb.Children.Add(da);
            sb.Begin();
            //Canvas.SetBottom(this, parent.Height - myGrid.Height);
        }

        public void Close()
        {
            Storyboard sb = new Storyboard();
            DoubleAnimation da = new DoubleAnimation(Canvas.GetBottom(this), parent.Height - 5, new System.Windows.Duration(new System.TimeSpan(0, 0, 0, 0, 250)));
            Storyboard.SetTarget(da, this);
            Storyboard.SetTargetName(da, this.Name);
            Storyboard.SetTargetProperty(da, new PropertyPath(Canvas.BottomProperty));
            sb.Children.Add(da);
            sb.Begin();
        }

        public void DisableConnectionButtons()
        {
            Host.IsEnabled = false;
            Join.IsEnabled = false;
        }

        public void DisableStartGameButton()
        {
            Start.IsEnabled = false;
        }

        public event EventHandler<CloseGameClickEventArgs> CloseGameClicked;
        private void OnCloseGameClicked(CloseGameClickEventArgs e)
        {
            CloseGameClicked(this, e);
        }
        public event EventHandler<JoinGameClickEventArgs> JoinGameClicked;
        private void OnJoinGameClicked(JoinGameClickEventArgs e)
        {
            JoinGameClicked(this, e);
        }
        public event EventHandler<HostGameClickEventArgs> HostGameClicked;
        private void OnHostGameClicked(HostGameClickEventArgs e)
        {
            HostGameClicked(this, e);
        }
        public event EventHandler<StartGameClickEventArgs> StartGameClicked;
        private void OnStartGameClicked(StartGameClickEventArgs e)
        {
            StartGameClicked(this, e);
        }

        private void Host_Click(object sender, RoutedEventArgs e)
        {
            OnHostGameClicked(new HostGameClickEventArgs());
        }

        private void Join_Click(object sender, RoutedEventArgs e)
        {
            OnJoinGameClicked(new JoinGameClickEventArgs());
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            OnCloseGameClicked(new CloseGameClickEventArgs());
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            OnStartGameClicked(new StartGameClickEventArgs());
        }
    }

    public class CloseGameClickEventArgs : EventArgs
    {
        public CloseGameClickEventArgs()
        {}
    }

    public class HostGameClickEventArgs : EventArgs
    {
        public HostGameClickEventArgs()
        { }
    }

    public class JoinGameClickEventArgs : EventArgs
    {
        public JoinGameClickEventArgs()
        { }
    }
    public class StartGameClickEventArgs : EventArgs
    {
        public StartGameClickEventArgs()
        { }
    }
}
