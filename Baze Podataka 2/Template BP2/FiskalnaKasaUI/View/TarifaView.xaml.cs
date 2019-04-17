using FiskalnaKasaUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FiskalnaKasaUI.View
{
    /// <summary>
    /// Interaction logic for TarifaView.xaml
    /// </summary>
    public partial class TarifaView : UserControl
    {
        public TarifaView()
        {
            InitializeComponent();
            this.DataContext = new TarifaViewModel();
        }
    }
}
