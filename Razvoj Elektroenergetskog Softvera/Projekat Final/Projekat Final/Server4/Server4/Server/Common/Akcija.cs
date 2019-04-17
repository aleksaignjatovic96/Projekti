using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Akcija
    {
        private string opisAkcije;
        private DateTime vreme;

        public Akcija()
        {
            opisAkcije = "";
            vreme = new DateTime();
        }

        public Akcija(string _opis, DateTime _vreme)
        {
            if(_opis == null)
            {
                throw new ArgumentNullException("Argument ne sme biti null");
            }

            if (_opis.Trim() == "")
            {
                throw new ArgumentException("Argument opis ne sme biti prazan");
            }

            opisAkcije = _opis;
            vreme = _vreme;
        }

        [DataMember]
        public string OpisAkcije { get { return opisAkcije; } set { opisAkcije = value; } }
        [DataMember]
        public DateTime Vreme { get { return vreme; } set { vreme = value; } }
    }
}
