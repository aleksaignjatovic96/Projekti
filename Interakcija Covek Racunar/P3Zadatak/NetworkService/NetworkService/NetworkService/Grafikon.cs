using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService
{
    public class Grafikon
    {

        public int RB { get; set; }
        public string BOJA { get; set; }
        public int Vrednost { get; set; }
        public int PUTID { get; set; }
        public string PUTNaziv { get; set; }
        public string PUTTip { get; set; }
        public int PUTVrednost { get; set; }
        public string Vreme { get; set; }

        public Grafikon() { }

        public Grafikon(int rb, string boja, int v, int qi, string qn, string qt, int qv, string dat)
        {
            RB = rb;
            BOJA = boja;
            Vrednost = v;
            PUTID = qi;
            PUTNaziv = qn;
            PUTTip = qt;
            PUTVrednost = qv;
            Vreme = dat;
        }
    }
}
