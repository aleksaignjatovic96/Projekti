using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class MyException 
    {
        //private string greska;

        //[DataMember]
        //public string Greska
        //{
        //    get { return greska; }
        //    set { greska = value; }
        //}

        [DataMember]
        public string Greska { get; set; }
    }
}
