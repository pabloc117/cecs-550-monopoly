using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.IO;
using System.Xml;

namespace Monopoly
{
    public static class ThemeParser
    {
        public static string ELEM_PROPERTY    = "Property";
        public static string ATTR_NAME        = "Name";
        public static string ATTR_COST        = "Cost";
        public static string ATTR_GROUP_COLOR = "GroupColor";
        public static string ATTR_LOCATION    = "Location";
        public static string ATTR_BOARD_SIDE   = "BoardSide";
        
        public static Dictionary<int, PropertyListing> GetPropertyListings()
        {            
            string listingPath = Directory.GetCurrentDirectory() + "\\Resources\\property_listing.xml";
            Dictionary<int, PropertyListing> dict = new Dictionary<int, PropertyListing>();
            if(File.Exists(listingPath))
            {
                Console.WriteLine("Accessing " + listingPath);
                XmlTextReader reader = new XmlTextReader(listingPath);
                int bs;
                int cost;
                string gc;
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
                            gc = reader.GetAttribute(ATTR_GROUP_COLOR);
                            loc = Int32.Parse(reader.GetAttribute(ATTR_LOCATION));
                            name = reader.GetAttribute(ATTR_NAME);
                            dict.Add(loc, new PropertyListing(name, cost, loc, gc, bs));
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

    }
    public class PropertyListing
    {
        string _name;
        int _cost;
        int _loc;
        Brush _color;
        Property.Side _side;

        public PropertyListing(String name, int cost, int loc, string colors, int side)
        {
            _name = name;
            _cost = cost;
            _loc = loc;
            _color = new SolidColorBrush(ParseRGB(colors));
            _side = ParseSide(side);
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

        private Color ParseRGB(string colors)
        {
            string[] c = colors.Split(';');
            string r = c[0];
            string g = c[1];
            string b = c[2];
            return Color.FromRgb(byte.Parse(r), byte.Parse(g), byte.Parse(b));
        }
    }
}
