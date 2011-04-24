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
    /// Interaction logic for DiceRack.xaml
    /// </summary>
    public partial class DiceRack : UserControl
    {
        public DiceRack()
        {
            InitializeComponent();
            d1.RollEnded += new EventHandler<DiceEndedEventArgs>(dice_ended);
            d2.RollEnded += new EventHandler<DiceEndedEventArgs>(dice_ended);
        }

        private bool oneDone;
        private bool twoDone;
        private bool rolling;
        void dice_ended(object sender, DiceEndedEventArgs e)
        {
            if (this.Dispatcher.CheckAccess())
            {
                if ((string)((sender as Dice).Tag) == "1")
                    oneDone = true;
                if ((string)((sender as Dice).Tag) == "2")
                    twoDone = true;
                if (oneDone && twoDone && rolling)
                {
                    rolling = false;
                    OnRollEnded(new RollEndedEventArgs(d1.Value, d2.Value));
                }
            }
            else this.Dispatcher.BeginInvoke(new Action<object, DiceEndedEventArgs>(dice_ended), new object[] { sender, e });
        }

        public void ToggleRollsEnabled(bool isEnabled)
        {
            if (this.Dispatcher.CheckAccess())
            {
                roll_button.IsEnabled = isEnabled;
            }
        }

        public void RollDice(int seed)
        {
            oneDone = false;
            twoDone = false;
            rolling = true;
            roll_button.IsEnabled = false;
            Random rand = new Random(seed);
            d1.Roll(rand.Next(0, 6));
            d2.Roll(rand.Next(0, 6));
        }
        
        public event EventHandler<RollEndedEventArgs> RollEnded;
        protected void OnRollEnded(RollEndedEventArgs e)
        {
            RollEnded(this, e);
        }

        public event EventHandler<RollStartedEventArgs> RollStarted;
        protected void OnRollStarted(RollStartedEventArgs e)
        {
            RollStarted(this, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int seed = (int)DateTime.Now.Ticks;
            OnRollStarted(new RollStartedEventArgs(seed));
            RollDice(seed);
        }
    }
    public class RollStartedEventArgs : EventArgs
    {
        public int Seed = 0;
        public RollStartedEventArgs(int seed)
        {
            Seed = seed;
        }
    }

    public class RollEndedEventArgs : EventArgs
    {
        public int DiceOneValue = 0;
        public int DiceTwoValue = 0;
        public RollEndedEventArgs(int v1, int v2)
        {
            DiceOneValue = v1;
            DiceTwoValue = v2;
        }
    }
}
