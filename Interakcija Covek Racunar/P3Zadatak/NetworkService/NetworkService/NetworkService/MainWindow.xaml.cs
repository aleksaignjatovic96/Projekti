using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace NetworkService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region GLOBAL VARS
        private int count = 15; // Inicijalna vrednost broja objekata u sistemu
                                // ######### ZAMENITI stvarnim brojem elemenata

        // Monitor je aktivan
        private bool qMonitorStart = false;


        private DataIO serializer = new DataIO();

        //Lista svih puteva
        public static BindingList<Putevi> Putevi { get; set; }

        //Lista aktivnih puteva(monitoring tab1)
        public ObservableCollection<Canvas> qCanvas = new ObservableCollection<Canvas>();

        //Lista za graph
        List<Grafikon> qGraph = new List<Grafikon>();


        private Putevi draggedItem;
        private bool dragging = false;
        #endregion

        #region MAIN
        public MainWindow()
        {
            // Ucitaj Listu puteva u BindingList<Putevi>
            Putevi = serializer.DeSerializeObject<BindingList<Putevi>>("putevi.xml");
            if (Putevi == null)
            {
                Putevi = new BindingList<Putevi>();
            }
            DataContext = this;


            InitializeComponent();
            createListener(); //Povezivanje sa serverskom aplikacijom

            // Ucitavanje log fajla
            Log("Start NetworkService");
            UcitajLog();

            //Danasni datum za log filter
            DatumOd.SelectedDate = DateTime.Now.Date;
            DatumDo.SelectedDate = DateTime.Now.Date;

            //event na promenu podataka u binding list Putevi
            Putevi.ListChanged += Putevi_ListChanged;

            listgrafikon.ItemsSource = qGraph;

            qMonitorStart1.Text = "Neaktivan";

        }
        #endregion

        #region LISTENER
        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(Putevi.Count.ToString());
                            stream.Write(data, 0, data.Length);
                                                        this.Dispatcher.Invoke((Action)(() =>
                            {
                              qMonitorStart = true;
                              qMonitorStart1.Text = "Aktivan";
                            }));
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            //Console.WriteLine(incomming); //Na primer: "Objekat_1:272"

                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji


                            this.Dispatcher.Invoke((Action)(() =>
                            {

                                if (qMonitorStart == true)
                                {
                                    //Azuriranje obekta Putevi
                                    string[] obj = incomming.Split(new char[] { '_', ':' });

                                    if (Putevi.Count > Int32.Parse(obj[1]))
                                    {

                                        Putevi[Int32.Parse(obj[1])].Vrednost = Int32.Parse(obj[2]);
                                        listBox.Items.Refresh();
                                        dataGrid.Items.Refresh();





                                        //Dodaj u Log fajl
                                        Log(incomming);

                                        string datum = DateTime.Now.ToString("HH:mm:ss dd MMMM yyyy");
                                        string ql = datum + " " + incomming;
                                        TextBox_LOG.Text = ql + "\n" + TextBox_LOG.Text;


                                        //Refresh Grafikon
                                        Grafikon_Refresh();
                                    }
                                }

                            }));

                            ;
                        }
                    }, null);
                    
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }
        #endregion

        #region EVENT
        void Putevi_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                BindingList<Putevi> x11 = (BindingList<Putevi>)sender;


                for (int k = 0; k < qCanvas.Count; k++)
                {
                    int qID = ((Putevi)qCanvas[k].Resources["taken"]).ID;
                    string qTIP = ((Putevi)qCanvas[k].Resources["taken"]).Tip;
                    int qVREDNOST = ((Putevi)qCanvas[k].Resources["taken"]).Vrednost;


                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    if (qTIP == "Tip - IB")
                        if (qVREDNOST > 7000)
                        {
                            img.UriSource = new Uri(@"PutErr.png", UriKind.RelativeOrAbsolute);
                        }
                        else
                        {
                            img.UriSource = new Uri(@"Put.png", UriKind.RelativeOrAbsolute);
                        }
                    else
                    {
                        if (qVREDNOST > 15000)
                        {
                            img.UriSource = new Uri(@"AutoputErr.png", UriKind.RelativeOrAbsolute);
                        }
                        else
                        {
                            img.UriSource = new Uri(@"Autoput.png", UriKind.RelativeOrAbsolute);
                        }
                    }
                    img.EndInit();
                    qCanvas[k].Background = new ImageBrush(img);


                } // End For

            }
            else if (e.ListChangedType == ListChangedType.ItemDeleted)
            {


            }

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }

            int i = Putevi.Count;

            while (i > 0)
            {
                Putevi[i-1].Ukljucen = false;
                i--;
            }

            

            serializer.SerializeObject<BindingList<Putevi>>(Putevi, "putevi.xml");

            
        }
        #endregion

        #region TAB 1


        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dragging)
            {
                dragging = true;
                draggedItem = (Putevi)listBox.SelectedItem;
                DragDrop.DoDragDrop(this, draggedItem, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void listBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //draggedItem = null;
            listBox.SelectedItem = null;
            dragging = false;
        }

        private void dragOver(object sender, DragEventArgs e)
        {
            base.OnDragOver(e);

            if (((Canvas)sender).Resources["taken"] != null || draggedItem.Ukljucen == true)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                
                 e.Effects = DragDropEffects.Copy;

            }

            e.Handled = true;
        }

        private void drop(object sender, DragEventArgs e)
        {
            base.OnDrop(e);

            if (draggedItem != null)
            {
                if (((Canvas)sender).Resources["taken"] == null)
                {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    if (draggedItem.Tip == "Tip - IB")
                        img.UriSource = new Uri(@"Put.png", UriKind.RelativeOrAbsolute);
                    else
                        img.UriSource = new Uri(@"Autoput.png", UriKind.RelativeOrAbsolute);
                    img.EndInit();
                    ((Canvas)sender).Background = new ImageBrush(img);
                    ((Canvas)sender).Resources.Add("taken", draggedItem);
                    ((Button)(((Canvas)sender).Children[0])).Visibility = Visibility.Visible;
                    draggedItem.Ukljucen = true;
                    qCanvas.Add(((Canvas)sender));
                }

                listBox.SelectedItem = null;
                dragging = false;
            }

            e.Handled = true;
        }

        private void Canvas_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (((Canvas)sender).Resources["taken"] != null)
            {
               int qID = ((Putevi)((Canvas)sender).Resources["taken"]).ID;
               string qNaziv = ((Putevi)((Canvas)sender).Resources["taken"]).Naziv;
               string qTIP = ((Putevi)((Canvas)sender).Resources["taken"]).Tip;
               int qVREDNOST = ((Putevi)((Canvas)sender).Resources["taken"]).Vrednost;

               lbstatusbar.Text = "ID: " + qID.ToString() + "    Naziv: " + qNaziv + "    Tip: " + qTIP + "   Vrednost: " + qVREDNOST.ToString();
            }

        }

        private void Canvas_1_MouseLeave(object sender, MouseEventArgs e)
        {
            lbstatusbar.Text = "";
        }

        private void ButtonIzbaci_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Visibility = Visibility.Hidden;                                 //Sakri dugme X
            ((Putevi)((Canvas)((Button)sender).Parent).Resources["taken"]).Ukljucen = false; //Omoguci ponovno ubacivanje objekta
            ((Canvas)((Button)sender).Parent).Resources.Remove("taken");                     // Obrisi resource iz Canvas
            ((Canvas)((Button)sender).Parent).Background = Brushes.Transparent;              //Postavi transparentnu pozadinu u Canvas
            qCanvas.Remove(((Canvas)((Button)sender).Parent));                               //Obrisi obj Putevi iz Liste monitoringa

        }

        #endregion

        #region TAB 2

        private void button_dodaj_Click(object sender, RoutedEventArgs e)
        {
            AddWindow newWindow = new AddWindow();
            newWindow.ShowDialog();
            qMonitorStart = false;
            qMonitorStart1.Text = "Neaktivan";
        }

        private void button_obrisi_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            if (dataGrid.SelectedItem != null)
            {
                qMonitorStart = false;
                qMonitorStart1.Text = "Neaktivan";


                //ako postoji u monitor, izbaci
                for (int k = 0; k < qCanvas.Count; k++)
                {

                    if (((Putevi)dataGrid.SelectedItem).ID == ((Putevi)qCanvas[k].Resources["taken"]).ID)
                    {
                        ((Button)qCanvas[k].Children[0]).Visibility = Visibility.Hidden;    // Sakri dugme X        
                        ((Canvas)qCanvas[k]).Resources.Remove("taken");                     // Obrisi resource iz Canvas          
                        ((Canvas)qCanvas[k]).Background = Brushes.Transparent;              // Postavi transparentnu pozadinu u Canvas
                        qCanvas.Remove(qCanvas[k]);                                         // Obrisi obj Putevi iz Liste monitoringa
                    }

                } // End For


                // Obrisi objekat Putevi
                Putevi.Remove((Putevi)dataGrid.SelectedItem);

                dataGrid.Items.Refresh();
                listBox.Items.Refresh();
            }
        }

        private void button_pretraga_Click(object sender, RoutedEventArgs e)
        {
            var filter = Putevi.Where(pretraga => pretraga.Naziv.Contains((TextBox_Pretraga.Text.ToUpper())));

            dataGrid.ItemsSource = filter; 
        }

        #endregion

        #region TAB 3

        private void Grafikon_Refresh()
        {
            // Ako je ComboBox selektovan
            if (ComboBox_Grafikon.SelectedItem == null)
            {
                return;
            }


            //Obrisi sve elemente
            if (qGraph.Count != 0)
               qGraph.Clear();

            int i = 0;

            // Ucitaj poslednja 38 reda
            // iz fajla
            foreach (string line in File.ReadLines("Log.txt").Reverse())
            {

                String substring = line.Substring(25, line.Length - 25);
                string[] obj = substring.Split(new char[] { '_', ':' });

                string qBoja = "Blue";

                string vreme = null;

                if (obj.Length == 3)
                {
                    if (Putevi.Count > Int32.Parse(obj[1]))
                    {

                        if (Putevi[Int32.Parse(obj[1])].ID == ((Putevi)ComboBox_Grafikon.SelectedItem).ID)
                        {

                            vreme = line.Substring(0, 24);

                            if (Putevi[Int32.Parse(obj[1])].Tip == "Tip - IB")
                            {
                                if (Int32.Parse(obj[2]) > 7000)
                                {
                                    qBoja = "Red";
                                }
                            }
                            else
                            {
                                if (Int32.Parse(obj[2]) > 15000)
                                {
                                    qBoja = "Red";
                                }
                            }

                            Double koef = 350.0 / 19000.0 * Int32.Parse(obj[2]);

                            Putevi qput = Putevi[Int32.Parse(obj[1])];
                            qGraph.Add(new Grafikon(i, qBoja, (int)koef, qput.ID, qput.Naziv, qput.Tip, Int32.Parse(obj[2]), vreme));
                            i++;

                        }
                    }

                    if (i > 38)
                    {
                        break;
                    }
                }
            }

            listgrafikon.Items.Refresh();
        }


        //Prikaz informacije grafa u statusbar
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            ListBoxItem lkl = sender as ListBoxItem;

            int qID = ((NetworkService.Grafikon)lkl.Content).PUTID;
            string qNaziv = ((NetworkService.Grafikon)lkl.Content).PUTNaziv;
            string qTIP = ((NetworkService.Grafikon)lkl.Content).PUTTip;
            int qVREDNOST = ((NetworkService.Grafikon)lkl.Content).PUTVrednost;
            string V = ((NetworkService.Grafikon)lkl.Content).Vreme;

            lbstatusbar.Text = "ID: " + qID.ToString() + "    Naziv: " + qNaziv + "    Tip: " + qTIP + "   Vrednost: " + qVREDNOST.ToString() + "    Vreme: " + V;
        }
        private void listgrafikon_MouseLeave(object sender, MouseEventArgs e)
        {
            lbstatusbar.Text = "";
        }

        private void ComboBox_Grafikon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Grafikon_Refresh();
        }

        #endregion

        #region TAB 4

        // Obrisi Log fajl
        private void Button_ObrisiLog_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("log.txt", "");
            Log("Log fajl obrisan !!!");
            UcitajLog();
        }

        private void Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            string line;


           TextBox_LOG.Text = "";


            using (StreamReader r = File.OpenText("log.txt"))
            {
                while ((line = r.ReadLine()) != null)
                {
                    String substring = line.Substring(9, 15);
                    DateTime dt = Convert.ToDateTime(substring);
                    DateTime dtod = Convert.ToDateTime(DatumOd.Text);
                    DateTime dtdo = Convert.ToDateTime(DatumDo.Text);

                    if (dt >= dtod && dt <= dtdo)
                    {
                        TextBox_LOG.Text = TextBox_LOG.Text + line + "\n";
                    }
                }
            }

            TextBox_LOG.Select(TextBox_LOG.Text.Length, 0);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DatumOd.IsEnabled = true;
            DatumDo.IsEnabled = true;
            Button_Filter.IsEnabled = true;
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DatumOd.IsEnabled = false;
            DatumDo.IsEnabled = false;
            Button_Filter.IsEnabled = false;
            UcitajLog();
        }
        // Snimi Log
        public void Log(string logMessage)
        {
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                string datum = DateTime.Now.ToString("HH:mm:ss dd MMMM yyyy");
                w.WriteLine("{0} {1} ", datum, logMessage);
            }
        }

        // Ucitaj Log
        public void UcitajLog()
        {
            string line;

            TextBox_LOG.Text = "";

            using (StreamReader r = File.OpenText("log.txt"))
            {
                while ((line = r.ReadLine()) != null)
                {
                    TextBox_LOG.Text = line + "\n"+TextBox_LOG.Text ;
                }
            }

            TextBox_LOG.Select(TextBox_LOG.Text.Length, 0);
            
        }

















        #endregion

        
    }

    #region GRAPH
    public class CounterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo cultureInfo)
        {
            int counter = (int)value;
            return counter * 20;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }
    #endregion


}
