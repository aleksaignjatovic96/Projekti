using FiskalnaKasaUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


using System.Data.Entity;
using System.ComponentModel;
using System.Windows.Input;
using FiskalnaKasaUI.Helpers;
using System.Windows;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Data;

namespace FiskalnaKasaUI.ViewModel
{

    public class KalkulacijaViewModel : INotifyPropertyChanged
    {

        private int sifraradnika;
        public int Sifraradnika
        {
            get { return sifraradnika; }
            set
            {
                sifraradnika = value;
                NoticeMe("Sifraradnika");
            }
        }


        private int brkalk;
        public int BrKalk
        {
            get { return brkalk; }
            set
            {
                brkalk = value;
                NoticeMe("BrKalk");
            }
        }

        private int sifrapartnera;
        public int SifraPartnera
        {
            get { return sifrapartnera; }
            set
            {
                sifrapartnera = value;
                NoticeMe("Sifrapartnera");
            }
        }

        private DateTime datum;
        public DateTime Datum
        {
            get { return datum; }
            set
            {
                datum = value;
                NoticeMe("Datum");
            }
        }


        private int sifraartikla;
        public int SifraArtikla
        {
            get { return sifraartikla; }
            set
            {
                sifraartikla = value;
                NoticeMe("SifraArtikla");
            }
        }

        private int kolicina;
        public int Kolicina
        {
            get { return kolicina; }
            set
            {
                kolicina = value;
                NoticeMe("Kolicina");
            }
        }

        private int cena;
        public int Cena
        {
            get { return cena; }
            set
            {
                cena = value;
                NoticeMe("Cena");
            }
        }

        private int marza;
        public int Marza
        {
            get { return marza; }
            set
            {
                marza = value;
                NoticeMe("Marza");
            }
        }


        private Dobavljanje _selectedItem;
        public Dobavljanje SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;

                if (_selectedItem != null)
                {
                    sifraartikla = _selectedItem.Artikal_SIF_ART;
                    Kolicina = _selectedItem.Kolicina;
                    Cena = _selectedItem.Cena_Dobavljaca;
                    Marza = _selectedItem.Marza_Procenat;
                }
                else
                {
                    sifraartikla = 0;
                    Kolicina = 0;
                    Cena = 0;
                    Marza = 0;
                }
                NoticeMe("SelectedItem");
                ButtonAddContent = "Add";
            }
        }


        private bool _stackForm = false;
        public bool StackForm
        {
            get
            {
                return _stackForm;
            }

            set
            {
                _stackForm = value;
                NoticeMe("StackForm");
            }
        }

        private string _buttonAddContent;
        public string ButtonAddContent
        {
            get
            {
                return _buttonAddContent;
            }

            set
            {
                _buttonAddContent = value;
                NoticeMe("ButtonAddContent");
            }
        }


        public ICommand NovaKalkulacijaCommand { get; set; }
        public ICommand SaveICommand { get; set; }
        public ICommand AddICommand { get; set; }
        public ICommand DeleteICommand { get; set; }

        public CollectionViewSource Collection { get; private set; }
        private FiskalnaKasaEntities _ctx;


        public KalkulacijaViewModel()
        {
            ButtonAddContent = "Add";

            this.NovaKalkulacijaCommand = new RelayCommand(NovaKalkulacijaExecute, NovaKalkulacijaCanExecute);
            this.SaveICommand = new RelayCommand(SaveExecute, SaveCanExecute);
            this.AddICommand = new RelayCommand(AddExecute, AddCanExecute);
            this.DeleteICommand = new RelayCommand(DeleteExecute, DeleteCanExecute);


            Collection = new CollectionViewSource();
            LoadData();
 
        }

        public bool NovaKalkulacijaCanExecute(object parametar)
        {
            return true;
        }
        public void NovaKalkulacijaExecute(object parametar)
        {
            try
            {
                bool IsInDB = _ctx.Radniks.Any(usr => usr.SIF_RAD == Sifraradnika);
                bool IsInDB1 = _ctx.Partners.Any(usr => usr.SIF_PART == SifraPartnera);

                if (!IsInDB || !IsInDB1)
                {
                    MessageBox.Show("Pogresno uneti podaci.");
                }
                else
                {
                    StackForm = true;

                    Kalkulacija poslednji = _ctx.Kalkulacijas
                       .OrderByDescending(p => p.SIF_KALK)
                       .FirstOrDefault();


                    //var poslednji = _ctx.Database.SqlQuery<int>("Function1 @paramName1", 0);

                    if (poslednji == null)
                        BrKalk = 1;
                    else
                        BrKalk = poslednji.SIF_KALK + 1;

                    Kalkulacija modify = new Kalkulacija();
                    modify.SIF_KALK = BrKalk;
                    modify.Datum_Kalkulacije = Datum; 
                    modify.Radnik_SIF_RAD = Sifraradnika;
                    modify.Partner_SIF_PART = SifraPartnera;
                    _ctx.Kalkulacijas.Add(modify);
                    _ctx.SaveChanges();

                    _ctx.Dobavljanjes.Load();
                    Collection.Source = _ctx.Dobavljanjes.Local;
                    Collection.SortDescriptions.Add(new SortDescription("Artikal_SIF_ART", ListSortDirection.Ascending));            //Orders the datagrid based on ID



                }


            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public bool AddCanExecute(object parametar)
        {
            return true;
        }
        public void AddExecute(object parametar)
        {
            if (ButtonAddContent == "Add")
            {
                sifraartikla = 0;
                Kolicina = 0;
                Cena = 0;
                Marza = 0;
                ButtonAddContent = "Cancel";
            }
            else
            {
                ButtonAddContent = "Add";
                SelectedItem = Collection.View.CurrentItem as Dobavljanje;

            }
        }

        public bool DeleteCanExecute(object parametar)
        {
            return true;
        }
        public void DeleteExecute(object parametar)
        {
            if (_selectedItem == null) return;

            try
            {
                _ctx.Dobavljanjes.Remove(_selectedItem);
                _ctx.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool SaveCanExecute(object parametar)
        {
            return true;
        }

        public void SaveExecute(object parametar)
        {
            try
            {
                if (ButtonAddContent == "Cancel")
                {

                    Dobavljanje modify = new Dobavljanje();
                    modify.Artikal_SIF_ART = SifraArtikla;
                    modify.Kolicina = kolicina;
                    modify.Cena_Dobavljaca = Cena;
                    modify.Marza_Procenat = Marza;

                    modify.Kalkulacija_SIF_KALK = BrKalk;
                    _ctx.Dobavljanjes.Add(modify);
                }
                else
                {
                    if (SelectedItem == null) return;
                    SelectedItem.Artikal_SIF_ART = SifraArtikla;
                }

                _ctx.SaveChanges();
                Refresh();
                SelectedItem = Collection.View.CurrentItem as Dobavljanje;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void LoadData()
        {
            _ctx = new FiskalnaKasaEntities();
 

        }

        private void Refresh()
        {
            _ctx.Dobavljanjes.Load();
            Collection.Source = _ctx.Dobavljanjes.Local;
            Collection.SortDescriptions.Add(new SortDescription("Artikal_SIF_ART", ListSortDirection.Ascending));            //Orders the datagrid based on ID

        }


        #region INotifyPropertyChanged
        protected void NoticeMe(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

    }
}
