using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PZ4
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class LineEntity
    {
        public string Name { get; set; }
        public ulong Id { get; set; }
        public bool IsUndergrounfd { get; set; }
        public float R { get; set; }
        public string ConductorMaterial { get; set; }
        public string LineType { get; set; }
        public int ThermalConstantHeat { get; set; }
        public ulong FirstEnd { get; set; }
        public ulong SecondEnd { get; set; }
        public List<Point> Vertices { get; set; }

        public LineEntity()
        {
            Vertices = new List<Point>();
        }

        public LineEntity(string name, ulong id, bool isUnder, float r, string cond, string line, int term, ulong first, ulong second, List<Point> vert)
        {
            this.Name = name;
            this.Id = id;
            this.IsUndergrounfd = isUnder;
            this.R = r;
            this.ConductorMaterial = cond;
            this.LineType = line;
            this.ThermalConstantHeat = term;
            this.FirstEnd = first;
            this.SecondEnd = second;
            this.Vertices = vert;
        }

    }
}
