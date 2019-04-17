using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace Projekat_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private DataIO serializer = new DataIO();
        public static BindingList<Povrce> Povrce { get; set; }

        public MainWindow()
        {
            Povrce = serializer.DeSerializeObject<BindingList<Povrce>>("povrce.xml");
            if (Povrce == null)
            {
                Povrce = new BindingList<Povrce>();
            }
            DataContext = this;

            InitializeComponent();
        }

        private void button_Dodaj(object sender, RoutedEventArgs e)
        {
            AddWindow newWindow = new AddWindow();
            newWindow.ShowDialog();
        }

        private void button_izlaz(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void save(object sender, CancelEventArgs e)
        {
            serializer.SerializeObject<BindingList<Povrce>>(Povrce, "povrce.xml");
        }


        void button_pregled(object sender, RoutedEventArgs e)
        {
            PregledWindow newWindow = new PregledWindow(dataGrid.SelectedItem);
            newWindow.ShowDialog();
        }

        void button_obrisi(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Da li ste sigurni da želite da obrišete ovo povrće?", "Da li ste sigurni?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Povrce.RemoveAt(dataGrid.SelectedIndex);
        }

        void button_izmeni(object sender, RoutedEventArgs e)
        {
            string n = ((Povrce)dataGrid.SelectedItems[0]).Naziv.ToString();
            string p = ((Povrce)dataGrid.SelectedItems[0]).Poreklo.ToString();
            double z = Double.Parse(((Povrce)dataGrid.SelectedItems[0]).Zaliha.ToString());
            double c = Double.Parse(((Povrce)dataGrid.SelectedItems[0]).Cena.ToString());
            string s = ((Povrce)dataGrid.SelectedItems[0]).Slika.ToString();
            IzmeniWindow newWindow = new IzmeniWindow(n, p, z, c, s, dataGrid.SelectedIndex);
            newWindow.ShowDialog();
        }
    }
}
