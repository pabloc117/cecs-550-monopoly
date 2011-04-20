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
    /// Interaction logic for Dice.xaml
    /// </summary>
    public partial class Dice : UserControl
    {
        public int Value { get; set; }
        int rand;
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        BitmapImage[] images = 
        { 
            new BitmapImage(new Uri("/Resources/dice1.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Resources/dice2.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Resources/dice3.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Resources/dice4.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Resources/dice5.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Resources/dice6.png", UriKind.Relative))
        };

        public Dice()
        {
            InitializeComponent();
        }
        /// <summary>
        /// start the thread to roll the dice
        /// </summary>
        /// <param name="Rand"></param>
        public void Roll(int Rand)
        {
            this.rand = Rand;
            ThreadStart threadStart = new ThreadStart(RollLogic);
            Thread thread = new Thread(threadStart);
            thread.IsBackground = true;
            thread.Start();
        }
        /// <summary>
        /// roll logic
        /// </summary>
        public void RollLogic()
        {
            int d = rand;
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    ChangeImage(images[i]);
                    Value = i+1;
                    Thread.Sleep(5*i*i);
                    if (j == 7 && i == rand)
                        break;
                }
            }
            OnDiceEnded(new DiceEndedEventArgs());
        }
        public event EventHandler<DiceEndedEventArgs> RollEnded;
        protected void OnDiceEnded(DiceEndedEventArgs e)
        {
            RollEnded(this, e);
        }
        /// <summary>
        /// change the dice image to make the "rolling effect"
        /// </summary>
        /// <param name="imageS"></param>
        private void ChangeImage(BitmapImage imageS)
        {
            if (mainWindow.Dispatcher.CheckAccess())
            {
                image.Source = imageS;
            }
            else
            {
                mainWindow.Dispatcher.BeginInvoke(new Action<BitmapImage>(ChangeImage), new object[] { imageS });
            }
        }
    }
    public class DiceEndedEventArgs : EventArgs
    {
        public DiceEndedEventArgs()
        {
        }
    }
}
