using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Projekat_1
{
    [Serializable]
    public class Povrce
    {
        public string Naziv { get; set; }
        public string Poreklo { get; set; }
        public double Cena { get; set; }
        public double Zaliha { get; set; }
        public string Slika { get; set; }

        public Povrce() { }

        public Povrce(string n, string p, double z, double c, string s)
        {
            Naziv = n;
            Poreklo = p;
            Cena = c;
            Zaliha = z;
            Slika = s;
        }
    }
}
