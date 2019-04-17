using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PredmetniZadatak2
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = false)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class NetworkModel
    {
        [System.Xml.Serialization.XmlArrayItemAttribute("SubstationEntity", IsNullable = false)]
        public List<SubstationEntity> Substations { get; set; }

        [System.Xml.Serialization.XmlArrayItemAttribute("NodeEntity", IsNullable = false)]
        public List<NodeEntity> Nodes { get; set; }

        [System.Xml.Serialization.XmlArrayItemAttribute("SwitchEntity", IsNullable = false)]
        public List<SwitchEntity> Switches { get; set; }

        [System.Xml.Serialization.XmlArrayItemAttribute("LineEntity", IsNullable = false)]
        public List<LineEntity> Lines { get; set; }
    }
}
