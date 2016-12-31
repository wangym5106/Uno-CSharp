using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno
{
    class Card
    {
        public Card(string color, string symbol)
        {
            this.color = color;
            this.symbol = symbol;
        }
        public string color { get; private set; }
        public string symbol { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Card c = obj as Card;
            if((System.Object)c == null)
                return false;

            return color.Equals(c.color) && symbol.Equals(c.symbol);
        }

        public bool Equals(Card c)
        {
            if ((System.Object)c == null)
                return false;

            return color.Equals(c.color) && symbol.Equals(c.symbol);
        }
        public override string ToString()
        {
            return color + symbol;
        }
    }
}
