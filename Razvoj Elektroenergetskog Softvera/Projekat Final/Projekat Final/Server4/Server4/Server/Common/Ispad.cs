using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Ispad
    {
        private int id;
        private DateTime vreme;
        private string opis;
        private Naponski_Nivo napon;
        private Status status;
        private List<Akcija> listaAkcija;
        private Element element;

        public Ispad()
        {
            listaAkcija = new List<Akcija>();
            element = new Element();
            id = 0;
            vreme = new DateTime();
            opis = "";
            napon = Naponski_Nivo.SrednjiNapon;
            status = Status.Novo;
        }

        public Ispad(int _id, DateTime _vreme, string _opis, Element _element, List<Akcija> _listaAkcija)
        {
            if (_opis == null || _element == null || _listaAkcija == null)
            {
                throw new ArgumentNullException("Argument ne sme biti null");
            }

            if(_opis.Trim() == "")
            {
                throw new ArgumentException("Argument ne sme biti prazan");
            }

            if(_element.Id.Trim() == "")
            {
                throw new ArgumentException("Argument ne sme biti prazan");
            }

            if (_element.Naziv.Trim() == "")
            {
                throw new ArgumentException("Argument ne sme biti prazan");
            }

            foreach (Akcija a in _listaAkcija)
            {
                if (a.OpisAkcije.Trim() == "")
                {
                    throw new ArgumentException("Argument ne sme biti prazan");
                }
            }

            id = _id;
            vreme = _vreme;
            napon = Naponski_Nivo.SrednjiNapon;
            opis = _opis;
            status = Status.Novo;
            element = _element;
            listaAkcija = _listaAkcija;
        }

        [DataMember]
        public DateTime Vreme { get { return vreme; }  set { vreme = value; } }
        [DataMember]
        public string Opis { get { return opis; } set { opis = value; } }
        [DataMember]
        public Naponski_Nivo Napon { get { return napon; } set { napon = value; } }
        [DataMember]
        public Status Status { get { return status; } set { status = value; } }
        [DataMember]
        public int Id { get { return id; } set { id = value; } }
        [DataMember]
        public List<Akcija> ListaAkcija { get { return listaAkcija; } set { listaAkcija = value; } }
        [DataMember]
        public Element Element { get { return element; } set { element = value; } }
    }
}
