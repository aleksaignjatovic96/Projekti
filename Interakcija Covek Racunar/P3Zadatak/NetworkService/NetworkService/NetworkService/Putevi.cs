using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NetworkService
{
    [Serializable]
    public class Putevi : INotifyPropertyChanged
    {
        private int vrednost;

        public int ID { get; set; }
        public string Naziv { get; set; }
        public string Tip { get; set; }
        public bool Ukljucen { get; set; }
        public int Vrednost
        {
            get { return this.vrednost; }
            set
            {
                if (this.vrednost != value)
                {
                    this.vrednost = value;
                    this.NotifyPropertyChanged("Promena");
                }
            }
        }

        public Putevi() { }

        public Putevi(int id, string n, string t, int v)
        {
            Naziv = n;
            ID = id;
            Tip = t;
            vrednost = v;
            Ukljucen = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }


    }
}
