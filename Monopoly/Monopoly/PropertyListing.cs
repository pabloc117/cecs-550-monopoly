using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Monopoly
{

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
            switch (side)
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
