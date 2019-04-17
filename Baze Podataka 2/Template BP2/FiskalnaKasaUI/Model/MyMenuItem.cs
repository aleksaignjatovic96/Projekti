using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FiskalnaKasaUI.Model
{
    public class MyMenuItem
    {
        public string Header { get; set; }
        public string Hint { get; set; }
        public string IconSource { get; set; }
        public ICommand Command { get; set; }
    }
}
