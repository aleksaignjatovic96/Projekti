using FiskalnaKasaUI.Helpers;
using FiskalnaKasaUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

using System.Data.Entity;
using System.Windows;

namespace FiskalnaKasaUI.ViewModel
{

    public class ArtikalViewModel : INotifyPropertyChanged
    {
        private int sifra;
        public int Sifra
        {
            get { return sifra; }
            set
            {
                sifra = value;
                NoticeMe("Sifra");
            }
        }

        private string naziv;
        public string Naziv
        {
            get { return naziv; }
            set
            {
                naziv = value;
                NoticeMe("Naziv");
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

        private int min_Kolicina;
        public int Min_Kolicina
        {
            get { return min_Kolicina; }
            set
            {
                min_Kolicina = value;
                NoticeMe("Min_Kolicina");
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

        private string barkod;
        public string BARCODE
        {
            get { return barkod; }
            set
            {
                barkod = value;
                NoticeMe("BARCODE");
            }
        }

        private int jedinicaMere_SIF_JEDMER;
        public int JedinicaMere_SIF_JEDMER
        {
            get { return jedinicaMere_SIF_JEDMER; }
            set
            {
                jedinicaMere_SIF_JEDMER = value;
                NoticeMe("JedinicaMere_SIF_JEDMER");
            }
        }

        private int tarifa_SIF_TAR;
        public int Tarifa_SIF_TAR
        {
            get { return tarifa_SIF_TAR; }
            set
            {
                tarifa_SIF_TAR = value;
                NoticeMe("Tarifa_SIF_TAR");
            }
        }

        private int grupa_SIF_GRP;
        public int Grupa_SIF_GRP
        {
            get { return grupa_SIF_GRP; }
            set
            {
                grupa_SIF_GRP = value;
                NoticeMe("Grupa_SIF_GRP");
            }
        }


        private Artikal _selectedItem;
        public Artikal SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;

                if (_selectedItem != null)
                {
                    Sifra = _selectedItem.SIF_ART;
                    Naziv = _selectedItem.Naziv;
                    Cena = _selectedItem.Cena;
                    Min_Kolicina = _selectedItem.Min_Kolicina;
                    Kolicina = _selectedItem.Kolicina;
                    BARCODE = _selectedItem.BARCODE;
                    JedinicaMere_SIF_JEDMER = _selectedItem.JedinicaMere_SIF_JEDMER;
                    Tarifa_SIF_TAR = _selectedItem.Tarifa_SIF_TAR;
                    Grupa_SIF_GRP = _selectedItem.Grupa_SIF_GRP;
                }
                else
                {
                    Sifra = 0;
                    Naziv = "";
                    Cena = 0;
                    Min_Kolicina = 0;
                    Kolicina = 0;
                    BARCODE = "";
                    JedinicaMere_SIF_JEDMER = 0;
                    Tarifa_SIF_TAR = 0;
                    Grupa_SIF_GRP = 0;
                }
                NoticeMe("SelectedItem");
                ButtonAddContent = "Add";
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


        public ICommand SaveICommand { get; set; }
        public ICommand AddICommand { get; set; }
        public ICommand DeleteICommand { get; set; }

        public CollectionViewSource Collection { get; private set; }
        private FiskalnaKasaEntities _ctx;


        public ArtikalViewModel()
        {
            ButtonAddContent = "Add";

            this.SaveICommand = new RelayCommand(SaveExecute, SaveCanExecute);
            this.AddICommand = new RelayCommand(AddExecute, AddCanExecute);
            this.DeleteICommand = new RelayCommand(DeleteExecute, DeleteCanExecute);


            Collection = new CollectionViewSource();
            LoadData();

        }


        public bool AddCanExecute(object parametar)
        {
            return true;
        }
        public void AddExecute(object parametar)
        {
            if (ButtonAddContent == "Add")
            {
                Sifra = 0;
                Naziv = "";
                Cena = 0;
                Min_Kolicina = 0;
                Kolicina = 0;
                BARCODE = "";
                JedinicaMere_SIF_JEDMER = 0;
                Tarifa_SIF_TAR = 0;
                Grupa_SIF_GRP = 0;

                ButtonAddContent = "Cancel";
            }
            else
            {
                ButtonAddContent = "Add";
                SelectedItem = Collection.View.CurrentItem as Artikal;

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
                _ctx.Artikals.Remove(_selectedItem);
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

                    bool IsInDB = _ctx.Artikals.Any(usr => usr.SIF_ART == Sifra);

                    if (!IsInDB)
                    {
                        Artikal modify = new Artikal();
                        modify.SIF_ART = Sifra;
                        modify.Naziv = Naziv;
                        modify.Cena = Cena;
                        modify.Min_Kolicina = Min_Kolicina;
                        modify.Kolicina = Kolicina;
                        modify.BARCODE = BARCODE;
                        modify.JedinicaMere_SIF_JEDMER = JedinicaMere_SIF_JEDMER;
                        modify.Tarifa_SIF_TAR = Tarifa_SIF_TAR;
                        modify.Grupa_SIF_GRP = Grupa_SIF_GRP;
                        _ctx.Artikals.Add(modify);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (SelectedItem == null) return;
                    SelectedItem.Naziv = Naziv;
                    SelectedItem.Cena = Cena;
                    SelectedItem.Min_Kolicina = Min_Kolicina;
                    SelectedItem.Kolicina = Kolicina;
                    SelectedItem.BARCODE = BARCODE;
                    SelectedItem.JedinicaMere_SIF_JEDMER = JedinicaMere_SIF_JEDMER;
                    SelectedItem.Tarifa_SIF_TAR = Tarifa_SIF_TAR;
                    SelectedItem.Grupa_SIF_GRP = Grupa_SIF_GRP;
                }

                _ctx.SaveChanges();
                Refresh();
                SelectedItem = Collection.View.CurrentItem as Artikal;

            }
            catch (Exception e)
            {
                MessageBox.Show("Uneti podaci nisu korektni.");
            }
        }

        private void LoadData()
        {
            Refresh();
            SelectedItem = Collection.View.CurrentItem as Artikal;
        }

        private void Refresh()
        {
            _ctx = new FiskalnaKasaEntities();
            _ctx.Artikals.Load();
            Collection.Source = _ctx.Artikals.Local;
            Collection.SortDescriptions.Add(new SortDescription("SIF_ART", ListSortDirection.Ascending));            //Orders the datagrid based on ID
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
