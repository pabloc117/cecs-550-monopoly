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
        int _cost, _loc, _house1, _house2, _house3, _house4, _hotel, _housecost, _mortgage;
        Brush _color;
        Property.Side _side;
        string _imgLoc = null;
        bool _isCorner = false;
        bool _isSpecial = false;
        public bool IsOwned
        {
            get
            {
                if (_owner == -1)
                    return false;
                else return true;
            }
        }
        private int _owner = -1;
        public int Owner
        {
            set { _owner = value; }
        }

        public PropertyListing(String name, int cost, int rent, int house1, int house2, int house3, int house4, int hotel, int housecost, int mortgage, int loc, int side, SolidColorBrush color)
        {
            _name = name;
            _cost = cost;
            _loc = loc;
            _color = color;
            _house1 = house1;
            _house2 = house2;
            _house3 = house3;
            _house4 = house4;
            _hotel = hotel;
            _housecost = housecost;
            _mortgage = mortgage;

            _side = ParseSide(side);
        }

        public PropertyListing(String name, int cost, int rent, int house1, int house2, int house3, int house4, int hotel, int housecost, int mortgage, int loc, int side, string imgLoc, bool isCorner)
        {
            _name = name;
            _cost = cost;
            _loc = loc;
            _imgLoc = imgLoc;
            _isCorner = isCorner;
            _house1 = house1;
            _house2 = house2;
            _house3 = house3;
            _house4 = house4;
            _hotel = hotel;
            _housecost = housecost;
            _mortgage = mortgage;

            _side = ParseSide(side);
            _isSpecial = true;
        }

        public int House1
        {
            get { return _house1; }
        }

        public int House2
        {
            get { return _house2; }
        }

        public int House3
        {
            get { return _house3; }
        }

        public int House4
        {
            get { return _house4; }
        }

        public int Hotel
        {
            get { return _hotel; }
        }

        public int HouseCost
        {
            get { return _housecost; }
        }

        public int Mortgage
        {
            get { return _mortgage; }
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
