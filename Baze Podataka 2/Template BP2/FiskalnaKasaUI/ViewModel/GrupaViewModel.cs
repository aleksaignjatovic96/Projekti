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
    public class GrupaViewModel : INotifyPropertyChanged
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

        private Grupa _selectedItem;
        public Grupa SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;

                if (_selectedItem != null)
                {
                    Sifra = _selectedItem.SIF_GRP;
                    Naziv = _selectedItem.Naziv_Grupe;
                }
                else
                {
                    Sifra = 0;
                    Naziv = "";
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


        public GrupaViewModel()
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
                ButtonAddContent = "Cancel";
            }
            else
            {
                ButtonAddContent = "Add";
                SelectedItem = Collection.View.CurrentItem as Grupa;

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
                _ctx.Grupas.Remove(_selectedItem);
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

                    bool IsInDB = _ctx.Grupas.Any(usr => usr.SIF_GRP == Sifra);

                    if (!IsInDB)
                    {

                        Grupa modify = new Grupa();
                        modify.SIF_GRP = Sifra;
                        modify.Naziv_Grupe = Naziv;
                        _ctx.Grupas.Add(modify);
                    }
                    else
                    {
                        
                    }
                }
                else
                {
                    if (SelectedItem == null) return;
                    SelectedItem.Naziv_Grupe = Naziv;
                }

                _ctx.SaveChanges();
                Refresh();
                SelectedItem = Collection.View.CurrentItem as Grupa;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void LoadData()
        {
            Refresh();
            SelectedItem = Collection.View.CurrentItem as Grupa;
        }

        private void Refresh()
        {
            _ctx = new FiskalnaKasaEntities();
            _ctx.Grupas.Load();
            Collection.Source = _ctx.Grupas.Local;
            Collection.SortDescriptions.Add(new SortDescription("SIF_GRP", ListSortDirection.Ascending));            //Orders the datagrid based on ID

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
