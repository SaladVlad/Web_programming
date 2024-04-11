using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vezbe4
{
    internal class Klub
    {
        string ime, grad;
        bool aktivan;
        int brojBodova;

        public Klub(string ime, string grad, bool aktivan, int brojBodova)
        {
            this.Ime = ime;
            this.Grad = grad;
            this.Aktivan = aktivan;
            this.BrojBodova = brojBodova;
        }

        public string Ime { get => ime; set => ime = value; }
        public string Grad { get => grad; set => grad = value; }
        public bool Aktivan { get => aktivan; set => aktivan = value; }
        public int BrojBodova { get => brojBodova; set => brojBodova = value; }
    }
}
