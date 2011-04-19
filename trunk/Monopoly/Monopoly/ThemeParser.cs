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
                else
                {
                    Console.WriteLine(listingPath + " does not exist.");
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
	}

	public class PropertyListing
	{
		string _name;
		int _cost;
		int _loc;
		Brush _color;
		Property.Side _side;
        string _imgLoc = null;
        bool _isCorner = false;
        bool _isSpecial = false;

        public PropertyListing(String name, int cost, int loc, int side, SolidColorBrush color)
		{
			_name = name;
			_cost = cost;
			_loc = loc;
			_color = color;
            _side = ParseSide(side);
		}

        public PropertyListing(String name, int cost, int loc, int side, string imgLoc, bool isCorner)
        {
            _name = name;
            _cost = cost;
            _loc = loc;
            _imgLoc = imgLoc;
            _isCorner = isCorner;
            _side = ParseSide(side);
            _isSpecial = true;
        }

        public string ImageLocation
        {
            get { return _imgLoc; }
        }

        public bool IsCorner
        {
            get { return _isCorner; }
        }

        public bool IsSpecial
        {
            get { return _isSpecial; }
        }

		public string Name
		{
			get { return _name; }
		}

		public int Cost
		{
			get { return _cost; }
		}

		public int Location
		{
			get { return _loc; }
		}

		public Property.Side Side
		{
			get { return _side; }
		}

		public Brush ColorGroup
		{
			get { return _color; }
		}

		private Property.Side ParseSide(int side)
		{
			Property.Side ret = Property.Side.UNKNOWN;
			switch(side) 
			{
				case (int)Property.Side.TOP:
					ret = Property.Side.TOP;
					break;
				case (int)Property.Side.RIGHT:
					ret = Property.Side.RIGHT;
					break;
				case (int)Property.Side.LEFT:
					ret = Property.Side.LEFT;
					break;
				case (int)Property.Side.BOTTOM:
					ret = Property.Side.BOTTOM;
					break;
				default:
					break;
			}
			return ret;
		}
	}
}
