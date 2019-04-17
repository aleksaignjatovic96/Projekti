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
    public class TarifaViewModel : INotifyPropertyChanged
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

        private double stopa;
        public double Stopa
        {
            get { return stopa; }
            set
            {
                stopa = value;
                NoticeMe("Stopa");
            }
        }

        private Tarifa _selectedItem;
        public Tarifa SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;

                if (_selectedItem != null)
                {
                    Sifra = _selectedItem.SIF_TAR;
                    Naziv = _selectedItem.Opis_Tarife;
                    Stopa = _selectedItem.Stopa;
                }
                else
                {
                    Sifra = 0;
                    Naziv = "";
                    Stopa = 0;
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


        public TarifaViewModel()
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
                Stopa = 0;
                ButtonAddContent = "Cancel";
            }
            else
            {
                ButtonAddContent = "Add";
                SelectedItem = Collection.View.CurrentItem as Tarifa;

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
                _ctx.Tarifas.Remove(_selectedItem);
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

                    bool IsInDB = _ctx.Tarifas.Any(usr => usr.SIF_TAR == Sifra);

                    if (!IsInDB)
                    {
                        Tarifa modify = new Tarifa();
                        modify.SIF_TAR = Sifra;
                        modify.Opis_Tarife = Naziv;
                        modify.Stopa = Stopa;
                        _ctx.Tarifas.Add(modify);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (SelectedItem == null) return;
                    SelectedItem.Opis_Tarife = Naziv;
                    SelectedItem.Stopa = Stopa;
                }

                _ctx.SaveChanges();
                Refresh();
                SelectedItem = Collection.View.CurrentItem as Tarifa;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void LoadData()
        {
            Refresh();
            SelectedItem = Collection.View.CurrentItem as Tarifa;
        }

        private void Refresh()
        {
            _ctx = new FiskalnaKasaEntities();
            _ctx.Tarifas.Load();
            Collection.Source = _ctx.Tarifas.Local;
            Collection.SortDescriptions.Add(new SortDescription("SIF_TAR", ListSortDirection.Ascending));            //Orders the datagrid based on ID
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
