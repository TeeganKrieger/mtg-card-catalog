using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace MTGCC.Database
{
    public struct Symbol : IEquatable<Symbol>
    {
        public static Symbol Red => new Symbol("{R}");
        public static Symbol Blue => new Symbol("{U}");
        public static Symbol Green => new Symbol("{G}");
        public static Symbol Black => new Symbol("{B}");
        public static Symbol White => new Symbol("{W}");

        private string _symbol;

        public Symbol(string symbol)
        {
            this._symbol = symbol;
        }

        public bool Equals(Symbol other)
        {
            return this._symbol == other._symbol;
        }

        public override bool Equals(object other)
        {
            return other is Symbol c && this.Equals(c);
        }

        public override int GetHashCode()
        {
            return this._symbol.GetHashCode();
        }

        public override string ToString()
        {
            return this._symbol;
        }

    }

}
