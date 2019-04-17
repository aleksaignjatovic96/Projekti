using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Element
    {
        private string id;
        private string naziv;
        private float x;
        private float y;

        public Element()
        {
            id = "";
            naziv = "";
            x = 0;
            y = 0;
        }

        public Element(string _id, string _naziv, float _x, float _y)
        {
            if(_id == null || _naziv == null)
            {
                throw new ArgumentNullException("Argument ne sme biti null");
            }

            if(_id.Trim() == "")
            {
                throw new ArgumentException("Argument ID ne sme biti prazan");
            }

            if (_naziv.Trim() == "")
            {
                throw new ArgumentException("Argument Naziv ne sme biti prazan");
            }

            id = _id;
            naziv = _naziv;
            x = _x;
            y = _y;
        }

        [DataMember]
        public string Id { get { return id; } set { id = value; } }
        [DataMember]
        public string Naziv { get { return naziv; } set { naziv = value; } }
        [DataMember]
        public float X { get { return x; } set { x = value; } }
        [DataMember]
        public float Y { get { return y; } set { y = value; } }
    }
}
