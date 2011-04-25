using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.IO;
using System.Xml;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace Monopoly
{
	public static class ThemeParser
	{
        # region Board variables
		public static string ELEM_BACKGROUND = "Background";
		public static string ELEM_BOARD = "Board";
		public static string ELEM_GROUP1 = "Group1";
		public static string ELEM_GROUP2 = "Group2";
		public static string ELEM_GROUP3 = "Group3";
		public static string ELEM_GROUP4 = "Group4";
		public static string ELEM_GROUP5 = "Group5";
		public static string ELEM_GROUP6 = "Group6";
		public static string ELEM_GROUP7 = "Group7";
		public static string ELEM_GROUP8 = "Group8";
		public static string ELEM_PROPERTY    = "Property";
		public static string ATTR_NAME        = "Name";
		public static string ATTR_COST        = "Cost";
		public static string ATTR_GROUP_COLOR = "GroupColor";
		public static string ATTR_LOCATION    = "Location";
		public static string ATTR_BOARD_SIDE   = "BoardSide";
        public static string ATTR_IS_SPECIAL = "IsSpecial";
        public static string ATTR_IS_CORNER = "IsCorner";
        public static string ATTR_IMAGE_LOCATION = "ImageLocation";
		private static string listingPath = Directory.GetCurrentDirectory() + "\\Resources\\theme.xml";
        #endregion

        #region GameCards variables
        public static string ELEM_CARD = "Card";
        public static string ATTR_TYPE = "Type";
        public static string ATTR_JUMP = "Jump";
        public static string ATTR_MOVE = "Move";
        public static string ATTR_COLLECT = "Collect";
        public static string ATTR_PAY = "Pay";
        public static string ATTR_PAYTO = "PayTo";
        public static string ATTR_NUM = "num";
        public static string ATTR_COLLECTFROM = "CollectFrom";
        public static string ATTR_TEXT = "Text";
        #endregion

        #region PropertyCards variables
        public static string ATTR_RENT = "Rent";
        public static string ATTR_HOUSE1 = "House1";
        public static string ATTR_HOUSE2 = "House2";
        public static string ATTR_HOUSE3 = "House3";
        public static string ATTR_HOUSE4 = "House4";
        public static string ATTR_HOTEL = "Hotel";
        public static string ATTR_MORTGAGE = "Mortgate";
        public static string ATTR_GROUPCOLOR = "GroupColor";
        public static string ATTR_ID = "ID";
        public static string ATTR_TITLE = "Title";
        #endregion

        #region Board settings
        private static SolidColorBrush[] colorPalette = GetColors();

		private static Color ParseRGB(string colors)
		{
			string[] c = colors.Split(';');
			string r = c[0];
			string g = c[1];
			string b = c[2];
			return Color.FromRgb(byte.Parse(r), byte.Parse(g), byte.Parse(b));
		}

        private static SolidColorBrush[] GetColors()
        {
            SolidColorBrush[] colors = new SolidColorBrush[10];

            Console.WriteLine("Accessing " + listingPath);
            XmlTextReader reader = new XmlTextReader(listingPath);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals(ELEM_BACKGROUND))
                    {
                        colors[(int)Colors.Background] = new SolidColorBrush(ParseRGB((string)reader.GetAttribute("Color")));
                    }
                    else if (reader.Name.Equals(ELEM_BOARD))
                    {
                        colors[(int)Colors.Board] = new SolidColorBrush(ParseRGB((string)reader.GetAttribute("Color")));
                    }
                    else if (reader.Name.Equals(ELEM_GROUP1))
                    {
                        colors[(int)Colors.Group1] = new SolidColorBrush(ParseRGB((string)reader.GetAttribute("Color")));
                    }
                    else if (reader.Name.Equals(ELEM_GROUP2))
                    {
                        colors[(int)Colors.Group2] = new SolidColorBrush(ParseRGB((string)reader.GetAttribute("Color")));
                    }
                    else if (reader.Name.Equals(ELEM_GROUP3))
                    {
                        colors[(int)Colors.Group3] = new SolidColorBrush(ParseRGB((string)reader.GetAttribute("Color")));
                    }
                    else if (reader.Name.Equals(ELEM_GROUP4))
                    {
                        colors[(int)Colors.Group4] = new SolidColorBrush(ParseRGB((string)reader.GetAttribute("Color")));
                    }
                    else if (reader.Name.Equals(ELEM_GROUP5))
                    {
                        colors[(int)Colors.Group5] = new SolidColorBrush(ParseRGB((string)reader.GetAttribute("Color")));
                    }
                    else if (reader.Name.Equals(ELEM_GROUP6))
                    {
                        colors[(int)Colors.Group6] = new SolidColorBrush(ParseRGB((string)reader.GetAttribute("Color")));
                    }
                    else if (reader.Name.Equals(ELEM_GROUP7))
                    {
                        colors[(int)Colors.Group7] = new SolidColorBrush(ParseRGB((string)reader.GetAttribute("Color")));
                    }
                    else if (reader.Name.Equals(ELEM_GROUP8))
                    {
                        colors[(int)Colors.Group8] = new SolidColorBrush(ParseRGB((string)reader.GetAttribute("Color")));
                    }
                }
            }

            return colors;
        }

		public enum Colors
		{
			Background = 0,
			Board = 1,
			Group1 = 2,
			Group2 = 3,
			Group3 = 4,
			Group4 = 5,
			Group5 = 6,
			Group6 = 7,
			Group7 = 8,
			Group8 = 9
		}

        public static Dictionary<int, PropertyListing> GetPropertyListing()
		{    
			Dictionary<int, PropertyListing> dict = new Dictionary<int, PropertyListing>();
			if(File.Exists(listingPath))
			{
				Console.WriteLine("Accessing " + listingPath);
				XmlTextReader reader = new XmlTextReader(listingPath);
				int bs;
				int cost;
				SolidColorBrush gc;
				int loc;
				string name;
				while(reader.Read())
				{
					if(reader.NodeType == XmlNodeType.Element)
					{
						if(reader.Name.Equals(ELEM_PROPERTY))
						{
							bs = Int32.Parse(reader.GetAttribute(ATTR_BOARD_SIDE));
							cost = Int32.Parse(reader.GetAttribute(ATTR_COST));
							gc = colorPalette[Int32.Parse(reader.GetAttribute(ATTR_GROUP_COLOR))];
							loc = Int32.Parse(reader.GetAttribute(ATTR_LOCATION));
							name = reader.GetAttribute(ATTR_NAME);
                            if (Int32.Parse(reader.GetAttribute(ATTR_IS_SPECIAL)) == 0)
                                dict.Add(loc, new PropertyListing(name, cost, loc, bs, gc));
                            else
                            {
                                bool isCorner = Int32.Parse(reader.GetAttribute(ATTR_IS_CORNER)) == 1;
                                string imgLoc = reader.GetAttribute(ATTR_IMAGE_LOCATION);
                                dict.Add(loc, new PropertyListing(name, cost, loc, bs, imgLoc, isCorner));
                            }
							Console.WriteLine(loc + ": " + dict[loc].Name);
						}
					}
				}

			}
			else
			{
				Console.WriteLine(listingPath + " does not exist.");
			}
			return dict;
		}

        public static SolidColorBrush GetColor(ThemeParser.Colors color)
        {
            return colorPalette[(int)color];
        }
        #endregion

        #region Game Card settings
        public static Dictionary<int, GameCard> Getcard()
        {
            Dictionary<int, GameCard> gdictionary = new Dictionary<int, GameCard>();
            if (File.Exists(listingPath))
            {
                Console.WriteLine("Accessing " + listingPath);
                XmlTextReader reader = new XmlTextReader(listingPath);
                int jump, move, collect, pay, id;
                string payTo, collectFrom, text, type;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name.Equals(ELEM_CARD))
                        {
                            id = Int32.Parse(reader.GetAttribute(ATTR_ID));
                            jump = Int32.Parse(reader.GetAttribute(ATTR_JUMP));
                            move = Int32.Parse(reader.GetAttribute(ATTR_MOVE));
                            collect = Int32.Parse(reader.GetAttribute(ATTR_COLLECT));
                            pay = Int32.Parse(reader.GetAttribute(ATTR_PAY));
                            payTo = reader.GetAttribute(ATTR_PAYTO);
                            collectFrom = reader.GetAttribute(ATTR_COLLECTFROM);
                            text = reader.GetAttribute(ATTR_TEXT);
                            type = reader.GetAttribute(ATTR_TYPE);

                            gdictionary.Add(id, new GameCard(id, jump, move, collect, pay, payTo, collectFrom, text, type));

                            Console.WriteLine(id + ": " + gdictionary[id].Text);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(listingPath + " does not exist.");
            }
            return gdictionary;
        }
        #endregion
    }
}
