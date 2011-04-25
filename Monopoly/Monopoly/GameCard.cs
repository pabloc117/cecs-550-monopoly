using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopoly
{
    public class GameCard
    {
        int _jump, _move, _collect, _pay, _id;
        string _payto, _collectFrom, _text, _type;
        
        public GameCard(int id, int jump, int move, int collect, int pay, string payto, string collectfrom, string text, string type)
        {
            _id = id;
            _jump = jump;
            _move = move;
            _collect = collect;
            _pay = pay;            
            _payto = payto;
            _collectFrom = collectfrom;
            _text = text;
            _type = type;
        }
        public int Jump
        {
            get {return _jump;}
        }
        public int Move
        {
            get {return _move;}
        }
        public int Collect
        {
            get {return _collect;}
        }
        public int Pay
        {
            get {return _pay;}
        }
        public int Id
        {
            get {return _id; }
        }
        public string PayTo
        {
            get {return _payto;}
        }
        public string CollectFrom
        {
            get {return _collectFrom;}
        }
        public string Text
        {
            get {return _text;}
        }
        public string Type
        {
            get { return _type; }
        }
    }
}
