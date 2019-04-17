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
using System.Windows.Shapes;

namespace Projekat_1
{
    /// <summary>
    /// Interaction logic for PregledWindow.xaml
    /// </summary>
    public partial class PregledWindow : Window
    {
        public Povrce povrce_pregled { get; set; }

        public PregledWindow(object p)
        {

            povrce_pregled = new Povrce();
            povrce_pregled = (Povrce)p;

            DataContext = this;

            InitializeComponent();
        }

        private void button_izlaz(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
