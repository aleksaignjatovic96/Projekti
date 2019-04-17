using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PZ4
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class SwitchEntity
    {
        GeometryModel3D obj;

        private string name;
        private ulong id;
        private double x;
        private double y;
        private string status;

        public GeometryModel3D Obj
        {
            get
            {
                return obj;
            }

            set
            {
                obj = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public ulong Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

        public SwitchEntity() { }

        public SwitchEntity(string name, ulong id, double x, double y, string status)
        {
            this.name = name;
            this.id = id;
            this.x = x;
            this.y = y;
            this.status = status;
        }

    }
}
