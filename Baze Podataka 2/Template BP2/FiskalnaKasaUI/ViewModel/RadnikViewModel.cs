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

namespace FiskalnaKasaUI.ViewModel
{
    public class RadnikViewModel : INotifyPropertyChanged
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

        private string ime;
        public string Ime
        {
            get { return ime; }
            set
            {
                ime = value;
                NoticeMe("Ime");
            }
        }

        private string prezime;
        public string Prezime
        {
            get { return prezime; }
            set
            {
                prezime = value;
                NoticeMe("Prezime");
            }
        }


        private Radnik _selectedItem;
        public Radnik SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;

                if (_selectedItem != null)
                {
                    Sifra = _selectedItem.SIF_RAD;
                    Ime = _selectedItem.Ime;
                    Prezime = _selectedItem.Prezime;
                }
                else
                {
                    Sifra = 0;
                    Ime = "";
                    Prezime = "";
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


        public RadnikViewModel()
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
                Ime = "";
                Prezime = "";
                ButtonAddContent = "Cancel";
            }
            else
            {
                ButtonAddContent = "Add";
                SelectedItem = Collection.View.CurrentItem as Radnik;

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
                _ctx.Radniks.Remove(_selectedItem);
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

                    bool IsInDB = _ctx.Radniks.Any(usr => usr.SIF_RAD == Sifra);

                    if (!IsInDB)
                    {

                        Radnik modify = new Radnik();
                        modify.SIF_RAD = Sifra;
                        modify.Ime = Ime;
                        modify.Prezime = Prezime;
                        modify.Username = "";
                        modify.Password = "";

                        _ctx.Radniks.Add(modify);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (SelectedItem == null) return;
                    SelectedItem.Ime = Ime;
                    SelectedItem.Prezime = Prezime;
                }

                _ctx.SaveChanges();
                Refresh();
                SelectedItem = Collection.View.CurrentItem as Radnik;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void LoadData()
        {
            Refresh();
            SelectedItem = Collection.View.CurrentItem as Radnik;
        }

        private void Refresh()
        {
            _ctx = new FiskalnaKasaEntities();
            _ctx.Radniks.Load();
            Collection.Source = _ctx.Radniks.Local;
            Collection.SortDescriptions.Add(new SortDescription("SIF_RAD", ListSortDirection.Ascending));            //Orders the datagrid based on ID

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
