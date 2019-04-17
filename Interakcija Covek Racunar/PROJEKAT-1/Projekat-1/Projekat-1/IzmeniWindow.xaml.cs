using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for IzmeniWindow.xaml
    /// </summary>
    public partial class IzmeniWindow : Window
    {
        public Povrce povrce_izmeni { get; set; }
        public string slika_path = "";
        public int index;

        public IzmeniWindow(string n, string p, double z, double c, string s, int ind)
        {
            povrce_izmeni = new Povrce(n, p, z, c, s);
            index = ind;
            slika_path = povrce_izmeni.Slika;

            DataContext = this;
            InitializeComponent();
        }

        private void button_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_Pretraga(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.png)|*.png|(*.jpg)|*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                slika_path = "" + openFileDialog.FileName + "";
                ImageSource imageSource = new BitmapImage(new Uri(openFileDialog.FileName));
                img_slika.Source = imageSource;
            }
        }

        private bool validate()
        {
            bool result = true;

            //NAZIV
            if (textBox_Naziv.Text.Trim().Equals("") || !Regex.IsMatch(textBox_Naziv.Text, @"^[a-zA-Z]+$"))
            {
                result = false;
                textBox_Naziv.BorderBrush = Brushes.Red;
                textBox_Naziv.BorderThickness = new Thickness(1);

                if (textBox_Naziv.Text.Trim().Equals(""))
                    label_NazivError.Content = "Ne može biti prazno!";
                else
                    label_NazivError.Content = "Morate uneti slova!";
            }
            else
            {
                textBox_Naziv.BorderBrush = Brushes.Green;
                label_NazivError.Content = string.Empty;
            }

            //POREKLO
            if (textBox_Poreklo.Text.Trim().Equals("") || !Regex.IsMatch(textBox_Naziv.Text, @"^[a-zA-Z]+$"))
            {
                result = false;
                textBox_Poreklo.BorderBrush = Brushes.Red;
                textBox_Poreklo.BorderThickness = new Thickness(1);

                if (textBox_Poreklo.Text.Trim().Equals(""))
                    label_PorekloError.Content = "Ne može biti prazno!";
                else
                    label_PorekloError.Content = "Morate uneti slova!";
            }
            else
            {
                textBox_Poreklo.BorderBrush = Brushes.Green;
                label_PorekloError.Content = string.Empty;
            }

            //ZALIHA
            double parsedValue;
            if (!double.TryParse(textBox_Zaliha.Text, out parsedValue) || Double.Parse(textBox_Zaliha.Text) < 0)
            {
                result = false;
                textBox_Zaliha.BorderBrush = Brushes.Red;
                textBox_Zaliha.BorderThickness = new Thickness(1);

                if (!double.TryParse(textBox_Zaliha.Text, out parsedValue))
                    label_ZalihaError.Content = "Morate uneti broj!";
                else
                    label_ZalihaError.Content = "Mora pozitivan broj!";
            }
            else
            {
                textBox_Zaliha.BorderBrush = Brushes.Green;
                label_ZalihaError.Content = string.Empty;
            }

            //CENA
            if (!double.TryParse(textBox_Cena.Text, out parsedValue) || Double.Parse(textBox_Cena.Text) < 0)
            {
                result = false;
                textBox_Cena.BorderBrush = Brushes.Red;
                textBox_Cena.BorderThickness = new Thickness(1);

                if (!double.TryParse(textBox_Cena.Text, out parsedValue))
                    label_CenaError.Content = "Morate uneti broj!";
                else
                    label_CenaError.Content = "Mora pozitivan broj!";
            }
            else
            {
                textBox_Cena.BorderBrush = Brushes.Green;
                label_CenaError.Content = string.Empty;
            }

            //SLIKA
            if (slika_path.Trim().Equals(""))
            {
                result = false;
            }

            return result;
        }

        private void button_izmeni(object sender, RoutedEventArgs e)
        {
            Povrce novo_povrce;

            if (validate())
            {
                Uri uri = new Uri(@slika_path);
                novo_povrce = new Povrce(textBox_Naziv.Text, textBox_Poreklo.Text, Double.Parse(textBox_Zaliha.Text), Double.Parse(textBox_Cena.Text), uri.ToString());
                MainWindow.Povrce[index] = novo_povrce;
                this.Close();

            }
            else
            {
                MessageBox.Show("Podaci nisu dobro popunjeni", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
