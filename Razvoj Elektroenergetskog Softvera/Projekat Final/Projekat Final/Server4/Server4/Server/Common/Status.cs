using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public enum Status 
    {
        [EnumMember]
        Novo,
        [EnumMember]
        NaCekanju,
        [EnumMember]
        UProgresu,
        [EnumMember]
        Testiranje,
        [EnumMember]
        Zatvoreno

    }//end Status
}
