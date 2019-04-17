using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImageViewer.Model
{
    public class MyMenuItem
    {
        public string Header { get; set; }
        public string IconSource { get; set; }
        public MyICommand<string> Command { get; set; }
    }
}
