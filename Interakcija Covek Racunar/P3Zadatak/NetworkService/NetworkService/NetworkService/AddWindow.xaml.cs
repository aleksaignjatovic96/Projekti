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

namespace NetworkService
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
        }

        private void button_odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
                Putevi novi_put;

                novi_put = new Putevi(Int32.Parse(textBox_id.Text), textBox_naziv.Text.ToUpper(), comboBox_tip.Text,0);
                MainWindow.Putevi.Add(novi_put);

                this.Close();

            }
            else
            {
                MessageBox.Show("Podaci nisu dobro popunjeni", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool validate()
        {
            bool result = true;

            //NAZIV
            if (textBox_naziv.Text.Trim().Equals(""))
            {
                result = false;
                textBox_naziv.BorderBrush = Brushes.Red;
                textBox_naziv.BorderThickness = new Thickness(1);

                if (textBox_naziv.Text.Trim().Equals(""))
                    label_greska_naziv.Content = "Ne može biti prazno!";
            }
            else
            {
                textBox_naziv.BorderBrush = Brushes.Green;
                label_greska_naziv.Content = string.Empty;
            }

            //ID          
            int parsedValue;
            if (!Int32.TryParse(textBox_id.Text, out parsedValue) || Int32.Parse(textBox_id.Text) < 0)
            {
                result = false;
                textBox_id.BorderBrush = Brushes.Red;
                textBox_id.BorderThickness = new Thickness(1);

                if (!Int32.TryParse(textBox_id.Text, out parsedValue))
                    label_greska_id.Content = "Morate uneti ceo broj!";
                else
                    label_greska_id.Content = "Mora pozitivan broj!";
            }
            else
            {
                textBox_id.BorderBrush = Brushes.Green;
                label_greska_id.Content = string.Empty;

                for (int k = 0; k < MainWindow.Putevi.Count; k++)
                {
                    if ((MainWindow.Putevi[k]).ID == parsedValue)
                    {
                        result = false;
                        textBox_id.BorderBrush = Brushes.Red;
                        textBox_id.BorderThickness = new Thickness(1);

                        label_greska_id.Content = "Već postoji taj ID!";
                    }
                }

            }

            //COMBOBOX
            if (comboBox_tip.SelectedItem == null)
            {
                result = false;
                comboBox_tip.BorderBrush = Brushes.Red;
                comboBox_tip.BorderThickness = new Thickness(1);

                label_greska_tip.Content = "Morate izabrati tip!";
            }
            else
            {
                comboBox_tip.BorderBrush = Brushes.Green;
                label_greska_tip.Content = string.Empty;
            }

            return result;
        }
    }
}
