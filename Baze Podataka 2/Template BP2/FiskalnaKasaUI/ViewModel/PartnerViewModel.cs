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

namespace FiskalnaKasaUI.ViewModel
{

    public class PartnerViewModel : INotifyPropertyChanged
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

        private string adresa;
        public string Adresa
        {
            get { return adresa; }
            set
            {
                adresa = value;
                NoticeMe("Adresa");
            }
        }

        private string telefon;
        public string Telefon
        {
            get { return telefon; }
            set
            {
                telefon = value;
                NoticeMe("Telefon");
            }
        }

        private string ziro_Racun;
        public string Ziro_Racun
        {
            get { return ziro_Racun; }
            set
            {
                ziro_Racun = value;
                NoticeMe("Ziro_Racun");
            }
        }

        private Partner _selectedItem;
        public Partner SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;

                if (_selectedItem != null)
                {
                    Sifra = _selectedItem.SIF_PART;
                    Naziv = _selectedItem.Ime_Partnera;
                    Adresa = _selectedItem.Adresa;
                    Telefon = _selectedItem.Telefon;
                    Ziro_Racun = _selectedItem.Ziro_Racun;
                }
                else
                {
                    Sifra = 0;
                    Naziv = "";
                    Adresa = "";
                    Telefon = "";
                    Ziro_Racun = "";
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


        public PartnerViewModel()
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
                Adresa = "";
                Telefon = "";
                Ziro_Racun = "";
                ButtonAddContent = "Cancel";
            }
            else
            {
                ButtonAddContent = "Add";
                SelectedItem = Collection.View.CurrentItem as Partner;

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
                _ctx.Partners.Remove(_selectedItem);
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

                    bool IsInDB = _ctx.Partners.Any(usr => usr.SIF_PART == Sifra);

                    if (!IsInDB)
                    {
                        Partner modify = new Partner();
                        modify.SIF_PART = Sifra;
                        modify.Ime_Partnera = Naziv;
                        modify.Adresa = Adresa;
                        modify.Telefon = Telefon;
                        modify.Ziro_Racun = Ziro_Racun;
                        _ctx.Partners.Add(modify);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (SelectedItem == null) return;

                    SelectedItem.Ime_Partnera = Naziv;
                    SelectedItem.Adresa = Adresa;
                    SelectedItem.Telefon = Telefon;
                    SelectedItem.Ziro_Racun = Ziro_Racun;

                }

                _ctx.SaveChanges();
                Refresh();
                SelectedItem = Collection.View.CurrentItem as Partner;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void LoadData()
        {
            Refresh();
            SelectedItem = Collection.View.CurrentItem as Partner;
        }

        private void Refresh()
        {
            _ctx = new FiskalnaKasaEntities();
            _ctx.Partners.Load();
            Collection.Source = _ctx.Partners.Local;
            Collection.SortDescriptions.Add(new SortDescription("SIF_PART", ListSortDirection.Ascending));            //Orders the datagrid based on ID
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
