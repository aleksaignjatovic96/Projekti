using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredmetniZadatak2
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
