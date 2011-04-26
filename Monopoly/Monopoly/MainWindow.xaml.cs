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
using Networking;
using System.Threading;
using System.Net;
using System.Windows.Threading;

namespace Monopoly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point _dim;
        private Point _loc;
        private Communicator comm;
        private MessageHandler mHandler = new MessageHandler();
        private GameBoard myBoard;
        private ChatComponent myChat;
        private TabControl myTabControl;
        private MenuFader myMenu;
        private UserPiece[] pieces;
        private Dictionary<string, Player> Players = new Dictionary<string,Player>();
        private Player localPlayer;
        private Engine engine;
        private int currentTurnPlayerID = 0;

        public MainWindow()
        {
            InitializeComponent();
            comm = Communicator.Instance;
            this.Background = ThemeParser.GetColor(ThemeParser.Colors.Background);
            comm.ConnectionStatusChanged += new EventHandler<ConnectionStatusChangedEventArgs>(comm_ConnectionStatusChanged);
            comm.DataRecieved += new EventHandler<DataReceivedEventArgs>(comm_DataRecieved);
            mHandler.NewIncomingMessage += new EventHandler<NewIncomingMessageEventArgs>(mHandler_NewIncomingMessage);
            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        ///////////////
        //Event Hooks//
        ///////////////
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ThreadStart s = new ThreadStart(SplashDelay);
            Thread splash = new Thread(s);
            splash.IsBackground = true;
            splash.Start();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //TODO Cleanup here
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                if (this.WindowState != System.Windows.WindowState.Normal)
                    Restore();
                else
                    Maximize();
            }
        }
        
        private void engine_PlayerTurn(object sender, PlayerTurnEventArgs e)
        {
            string msg = e.EndTurnId + Message.DELIMETER + e.StartTurnId;
            comm.Send(new Message(Message.Type.Turn, Encoding.UTF8.GetBytes(msg)).ToBytes());
            NewPlayerTurnMessage(e.EndTurnId, e.StartTurnId);
        }

        private void mHandler_NewIncomingMessage(object sender, NewIncomingMessageEventArgs e)
        {
            if (e.Message == null)
                return;
            string[] attr;
            string data = e.Message.Data == null ? "" : Encoding.UTF8.GetString(e.Message.Data);
            switch (e.Message.MessageType)
            {
                case Message.Type.Chat:
                    attr = MessageHandler.SplitString(data);
                    NewChatMessage(attr[0], attr[1]);
                    break;
                case Message.Type.Roll:
                    attr = MessageHandler.SplitString(data);
                    NewRollMessage(Int32.Parse(attr[1]));
                    break;
                case Message.Type.Trade:
                    break;
                case Message.Type.Unknown:
                    break;
                case Message.Type.IdInit:
                    NewPlayerInitMessage(data);
                    break;
                case Message.Type.Turn:
                    attr = MessageHandler.SplitString(data);
                    NewPlayerTurnMessage(Int32.Parse(attr[0]), Int32.Parse(attr[1]));
                    break;
                case Message.Type.EndTurn:
                    NewEndTurnMessage();
                    break;
                case Message.Type.Buy:
                    attr = MessageHandler.SplitString(data);
                    PropertyBought(Int32.Parse(attr[0]), attr[1]);
                    return;
                case Message.Type.Rent:
                    attr = MessageHandler.SplitString(data);
                    PayRent(attr[0], attr[1], Int32.Parse(attr[2]));
                    return;
                default:
                    break;
            }
        }

        private void myMenu_StartGameClicked(object sender, StartGameClickEventArgs e)
        {
            comm.EndWaitConnect();
            CompilePlayersPacket();
            InitializePieces(Players.Count);
            engine.StartGame(Players.Count);
            InitPlayerViews();
        }

        private void myMenu_HostGameClicked(object sender, HostGameClickEventArgs e)
        {
            comm.UserRole = Communicator.ROLE.SERVER;
            comm.StartServer(23);
            IPAddress ip = comm.GetMyIpAddr();
            Players.Add(comm._ComputerID.ToString("N"), new Player(0, comm._ComputerID.ToString("N")));
            localPlayer = Players[comm._ComputerID.ToString("N")];
            myMenu.DisableConnectionButtons();
            MessageBox.Show(ip.ToString());
            engine = new Engine();
            engine.PlayerTurn += new EventHandler<PlayerTurnEventArgs>(engine_PlayerTurn);
        }

        private void myMenu_CloseGameClicked(object sender, CloseGameClickEventArgs e)
        {
            this.Close();
        }

        private void myMenu_JoinGameClicked(object sender, JoinGameClickEventArgs e)
        {
            comm.UserRole = Communicator.ROLE.CLIENT;
            IPRequest ip = new IPRequest();
            ip.Owner = this;
            ip.IPAccept += new EventHandler<ConnectClickedEventArgs>(ip_IPAccept);
            ip.ShowDialog();
        }

        private void Dice_RollStarted(object sender, RollStartedEventArgs e)
        {
            string msg = localPlayer.PlayerId + Message.DELIMETER + e.Seed;
            comm.Send(new Message(Message.Type.Roll, Encoding.UTF8.GetBytes(msg)).ToBytes());
        }

        private void Dice_RollEnded(object sender, RollEndedEventArgs e)
        {
            int d1 = e.DiceOneValue;
            int d2 = e.DiceTwoValue;
            Move(pieces[currentTurnPlayerID], (d1 + d2));
        }

        private void Dice_EndTurn(object sender, EndTurnEventArgs e)
        {
            ToggleTurnItems(false);
            if (currentTurnPlayerID == localPlayer.PlayerId)
                if (comm.UserRole == Communicator.ROLE.SERVER)
                    engine.TurnEnded();
                else
                    comm.Send(new Message(Message.Type.EndTurn, new byte[0]).ToBytes());
        }

        private void ip_IPAccept(object sender, ConnectClickedEventArgs e)
        {
            comm.StartClient(e.IP, 23);
            myMenu.DisableConnectionButtons();
            myMenu.DisableStartGameButton();
        }

        private void myBoard_GameBuilt(object sender, GameBoardBuiltEventArgs e)
        {
            if (Dispatcher.CheckAccess())
            {
                Loading.Visibility = Visibility.Hidden;
            }
            else Dispatcher.BeginInvoke(new Action<object, GameBoardBuiltEventArgs>(myBoard_GameBuilt), new object[] { null, null });
        }

        private void comm_DataRecieved(object sender, DataReceivedEventArgs e)
        {
            mHandler.QueueMessage(e.DataReceived);
        }

        private void comm_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs e)
        {
            Console.Write("Connected = " + e.Connected);
            if (e.Connected && comm.UserRole == Communicator.ROLE.SERVER)
            {
                Players.Add(e.RemoteGUID.ToString("N"), new Player(Players.Count, e.RemoteGUID.ToString("N")));
                myChat.NewMessage("System", Players[e.RemoteGUID.ToString("N")].PlayerName + (e.Connected ? " has connected." : " has disconnected"));
            }
            else
            {
                myChat.NewMessage("System", "You have " + (e.Connected ? " connected." : " been disconnected"));
            }
        }

        private void Chat_NewOutgoingMessage(object sender, NewOutgoingMessageEventArgs e)
        {
            string localName = "RemoteUser";
            if (localPlayer != null)
                localName = localPlayer.PlayerName;
            string str = String.Concat(localName, Message.DELIMETER, e.Message);
            byte[] msg = Encoding.UTF8.GetBytes(str);
            comm.Send((new Message(Message.Type.Chat, msg)).ToBytes());
        }

        private void bq_Result(object sender, BuyPropertyEventArgs e)
        {
            if (e.Bought)
            {
                string msg = e.PropertyIndex + Message.DELIMETER + localPlayer.PlayerGUID;
                comm.Send(new Message(Message.Type.Buy, Encoding.UTF8.GetBytes(msg)).ToBytes());
                PropertyBought(e.PropertyIndex, localPlayer.PlayerGUID);
            }
            myBoard.Dice.ToggleEndTurnEnabled(true);
        }

        ///////////
        //Methods//
        ///////////
        public void InitialPlacement(ref UserPiece u)
        {
            foreach (var tp in myBoard.myBoard.Children)
            {
                if (tp as Property != null && ((Property)tp).PropertyListing.Location == 0)
                {
                    ((Property)tp).Spots.Children.Add(u = new UserPiece());
                    u.CurrentLocation = 0;
                }
            }
        }

        public void Move(UserPiece up, int value)
        {
            ParameterizedThreadStart start = new ParameterizedThreadStart(MoveWork);
            Thread moveThread = new Thread(start);
            moveThread.IsBackground = true;
            moveThread.Name = "MoveThread";
            moveThread.Start(new object[] { up, value } as object);
        }

        public void Jump(UserPiece up, int current, int destination)
        {
            if (this.Dispatcher.CheckAccess())
            {
                Property cur = null;
                Property des = null;
                foreach (var tp in myBoard.myBoard.Children)
                {
                    if (tp as Property != null && ((Property)tp).PropertyListing.Location == current)
                        cur = ((Property)tp);
                    if (tp as Property != null && ((Property)tp).PropertyListing.Location == destination)
                        des = ((Property)tp);
                }
                if (cur == null || des == null)
                    return;
                cur.Spots.Children.Remove(up);
                try
                {
                    des.Spots.Children.Add(up);
                }
                catch (Exception) { }
                up.CurrentLocation = destination;
                if (up.CurrentLocation == 0)
                {
                    Players[up.PlayerGUID].Money += 200;
                    myChat.NewMessage("System", Players[up.PlayerGUID].PlayerName + " receives $200 for passing on GO.");
                }
            }
            else this.Dispatcher.BeginInvoke(new Action<UserPiece, int, int>(Jump), new object[] { up, current, destination });
        }

        private void SplashDelay()
        {
            DateTime loadTime = DateTime.Now;
            while (DateTime.Now.Subtract(loadTime).TotalSeconds < 2.5) { }

            HideSplash();
            while (Splash.Visibility != Visibility.Collapsed) { }
            Setup();
        }

        private void HideSplash()
        {
            if (this.Dispatcher.CheckAccess())
            {
                Splash.Visibility = Visibility.Collapsed;
            }
            else this.Dispatcher.BeginInvoke(new Action(HideSplash));
        }

        private void Setup()
        {
            if (this.Dispatcher.CheckAccess())
            {
                myBoard = new GameBoard();
                myBoard.SetBinding(WidthProperty, "myGrid.Height");
                myBoard.SetBinding(HeightProperty, "myGrid.Height");
                myBoard.Background = ThemeParser.GetColor(ThemeParser.Colors.Board);
                Grid.SetRowSpan(myBoard, 3);
                Grid.SetColumn(myBoard, 0);
                myChat = new ChatComponent();
                myChat.NewOutgoingMessage += new EventHandler<NewOutgoingMessageEventArgs>(Chat_NewOutgoingMessage);
                Grid.SetColumn(myChat, 1);
                Grid.SetRow(myChat, 2);
                myChat.Margin = new Thickness(5, 0, 0, 0);
                myMenu = new MenuFader(myCanvas);
                myMenu.Margin = new Thickness(25,0,25,0);
                myMenu.HostGameClicked += new EventHandler<HostGameClickEventArgs>(myMenu_HostGameClicked);
                myMenu.JoinGameClicked += new EventHandler<JoinGameClickEventArgs>(myMenu_JoinGameClicked);
                myMenu.CloseGameClicked += new EventHandler<CloseGameClickEventArgs>(myMenu_CloseGameClicked);
                myMenu.StartGameClicked += new EventHandler<StartGameClickEventArgs>(myMenu_StartGameClicked);
                myGrid.Children.Add(myBoard);
                myGrid.Children.Add(myChat);
                myCanvas.Children.Add(myMenu);
                myBoard.GameBuilt += new EventHandler<GameBoardBuiltEventArgs>(myBoard_GameBuilt);
                myBoard.Dice.RollEnded += new EventHandler<RollEndedEventArgs>(Dice_RollEnded);
                myBoard.Dice.RollStarted += new EventHandler<RollStartedEventArgs>(Dice_RollStarted);
                myBoard.Dice.EndTurn += new EventHandler<EndTurnEventArgs>(Dice_EndTurn);
                ToggleTurnItems(false);

                
            }
            else this.Dispatcher.BeginInvoke(new Action(Setup), null);
        }

        private void MoveWork(object param)
        {
            object[] parameters = param as object[];
            if (parameters == null)
            {
                System.Console.WriteLine("Error in parsing parameters for MoveWork.");
                return;
            }
            UserPiece up = parameters[0] as UserPiece;
            int value = (int)parameters[1];
            if (up == null)
            {
                System.Console.WriteLine("Error in parsing UserPiece for MoveWork.");
                return;
            }
            int i = up.CurrentLocation + value;
            if (i > 39)
            {
                int j = up.CurrentLocation;
                i = i - 40;
                value = value - i;
                while (up.CurrentLocation <= j + value)
                {
                    if (up.CurrentLocation == 39)
                    {
                        Jump(up, up.CurrentLocation, 0);
                        Thread.Sleep(250);
                        break;
                    }
                    else Jump(up, up.CurrentLocation, up.CurrentLocation + 1);
                    Thread.Sleep(250);
                }
            }
            while(up.CurrentLocation < i)
            {
                Jump(up, up.CurrentLocation, up.CurrentLocation + 1);
                Thread.Sleep(250);
            }
            if (localPlayer.PlayerId == currentTurnPlayerID)
                ShowBuyQuery(myBoard.Listings[up.CurrentLocation]);
        }

        private void InitializePieces(int num)
        {
            if (this.Dispatcher.CheckAccess())
            {
                pieces = new UserPiece[num];
                for (int i = 0; i < pieces.Count<UserPiece>(); i++)
                {
                    InitialPlacement(ref pieces[i]);
                    foreach (Player p in Players.Values)
                    {
                        if (p.PlayerId == i)
                            pieces[i].PlayerGUID = p.PlayerGUID;
                    }
                    switch (i)
                    {
                        case 0:
                            Grid.SetColumn(pieces[i], 0);
                            Grid.SetRow(pieces[i], 0);
                            pieces[i].ellipse.Fill = Brushes.Red;
                            break;
                        case 1:
                            Grid.SetColumn(pieces[i], 3);
                            Grid.SetRow(pieces[i], 0);
                            pieces[i].ellipse.Fill = Brushes.Green;
                            break;
                        case 2:
                            Grid.SetColumn(pieces[i], 0);
                            Grid.SetRow(pieces[i], 3);
                            pieces[i].ellipse.Fill = Brushes.Blue;
                            break;
                        case 3:
                            Grid.SetColumn(pieces[i], 3);
                            Grid.SetRow(pieces[i], 3);
                            pieces[i].ellipse.Fill = Brushes.Orange;
                            break;
                        default:
                            break;
                    }
                }
            }
            else this.Dispatcher.BeginInvoke(new Action<int>(InitializePieces), new object[] { num });
        }

        private void CompilePlayersPacket()
        {
            StringBuilder packet = new StringBuilder();
            foreach (Player p in Players.Values)
            {
                packet.Append(p.PlayerId + "=" + p.PlayerGUID + ";");
            }
            byte[] msg = Encoding.UTF8.GetBytes(packet.ToString());
            comm.Send((new Message(Message.Type.IdInit, msg)).ToBytes());
        }

        private void ToggleTurnItems(bool isEnabled)
        {
            myBoard.Dice.ToggleRollsEnabled(isEnabled);
            myBoard.Dice.ToggleEndTurnEnabled(false);
        }

        private void ShowBuyQuery(PropertyListing property)
        {
            if (this.Dispatcher.CheckAccess())
            {
                myBoard.Dice.ToggleEndTurnEnabled(true);
                if (property.IsOwned)
                {
                    string msg = localPlayer.PlayerGUID + Message.DELIMETER + property.Owner + Message.DELIMETER + property.CalculateRent();
                    comm.Send(new Message(Message.Type.Rent, Encoding.UTF8.GetBytes(msg)).ToBytes());
                    PayRent(localPlayer.PlayerGUID, property.Owner, property.CalculateRent());
                    return;
                }
                if (property.Cost > localPlayer.Money)
                {
                    MessageBox.Show("You cannot afford this property");
                    return;
                }
                if(property.Cost == 0)
                {
                    return;
                }
                BuyQuery bq = new BuyQuery(this, property);
                bq.Result += new EventHandler<BuyPropertyEventArgs>(bq_Result);
                bq.ShowDialog();
            }
            else this.Dispatcher.BeginInvoke(new Action<PropertyListing>(ShowBuyQuery), new object[] { property });
        }

        public void PayRent(string payerGUID, string payeeGUID, int rent)
        {
            Players[payerGUID].Money -= rent;
            Players[payeeGUID].Money += rent;
            myChat.NewMessage("System", Players[payerGUID].PlayerName + " paid " + Players[payeeGUID].PlayerName + " $" + rent + " rent.");
        }

        private void NewChatMessage(string Sender, string Message)
        {
            if (myChat != null)
            {
                myChat.NewMessage(Sender, Message);
            }
        }

        private void NewRollMessage(int Seed)
        {
            myBoard.Dice.RollDice(Seed);
        }

        private void NewEndTurnMessage()
        {
            if (comm.UserRole == Communicator.ROLE.SERVER)
                engine.TurnEnded();
        }

        private void NewPlayerTurnMessage(int EndTurnId, int StartTurnId)
        {
            currentTurnPlayerID = StartTurnId;
            if (localPlayer == null)
                throw new NullReferenceException("Player was null.");
            if (localPlayer.PlayerId == EndTurnId)
                ToggleTurnItems(false);
            else if (localPlayer.PlayerId == StartTurnId)
                ToggleTurnItems(true);
        }

        private void NewPlayerInitMessage(String MessageData)
        {
            string[] players = MessageData.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in players)
            {
                string[] playerString = s.Split(new string[] { "=" }, StringSplitOptions.None);
                string endpointID = playerString[1];
                Players.Add(playerString[1], new Player(Int32.Parse(playerString[0]), playerString[1]));
                if (Players[endpointID].PlayerGUID.CompareTo(comm._ComputerID.ToString("N")) == 0)
                {
                    localPlayer = Players[endpointID];
                }
            }
            InitPlayerViews();
            InitializePieces(Players.Count);
        }

        private void PropertyBought(int PropertyIndex, string UserGUID)
        {
            myBoard.Listings[PropertyIndex].Owner = UserGUID;
            Players[UserGUID].Money -= myBoard.Listings[PropertyIndex].Cost;
            myBoard.SetOwnerText(PropertyIndex, Players[UserGUID].PlayerName);
            Players[UserGUID].AddProperty(PropertyIndex, myBoard.Listings[PropertyIndex]);
            myChat.NewMessage("System", Players[UserGUID].PlayerName + " bought " + myBoard.Listings[PropertyIndex].Name + ".");
        }

        private void InitPlayerViews()
        {
            if (this.Dispatcher.CheckAccess())
            {
                myTabControl = new TabControl();
                foreach (Player p in Players.Values)
                {
                    TabItem ti = new TabItem();
                    ti.Content = new PlayerInfo(p);
                    ti.Header = p.PlayerName;
                    myTabControl.Items.Add(ti);
                }
                Grid.SetColumn(myTabControl, 1);
                Grid.SetRow(myTabControl, 0);
                myTabControl.Margin = new Thickness(5, 0, 0, 0);
                Dispatcher.BeginInvoke(new Action(() => Keyboard.Focus((TabItem)(myTabControl.Items.GetItemAt(localPlayer.PlayerId)))), DispatcherPriority.ContextIdle);
                Dispatcher.BeginInvoke(new Action(() => Keyboard.ClearFocus()), DispatcherPriority.ContextIdle);  
                myGrid.Children.Add(myTabControl);
            }
            else this.Dispatcher.BeginInvoke(new Action(InitPlayerViews));
        }

        private void Maximize()
        {
            if (this.Dispatcher.CheckAccess())
            {
                _dim = new Point(this.Width, this.Height);
                _loc = new Point(this.Left, this.Top);
                this.WindowState = System.Windows.WindowState.Maximized;
                this.WindowStyle = System.Windows.WindowStyle.None;
                if(myMenu != null)
                    this.myMenu.Close();
            }
            else this.Dispatcher.BeginInvoke(new Action(Maximize));
        }

        private void Restore()
        {
            if (this.Dispatcher.CheckAccess())
            {
                this.Left = _loc.X;
                this.Top = _loc.Y;
                this.Width = _dim.X;
                this.Height = _loc.Y;
                this.WindowState = System.Windows.WindowState.Normal;
                this.WindowStyle = System.Windows.WindowStyle.ThreeDBorderWindow;
                this.myMenu.Close();
            }
            else this.Dispatcher.BeginInvoke(new Action(Restore));
        }
    }
}
