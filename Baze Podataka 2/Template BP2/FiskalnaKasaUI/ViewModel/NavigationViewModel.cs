using FiskalnaKasaUI.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FiskalnaKasaUI.ViewModel
{
    public class NavigationViewModel : INotifyPropertyChanged
    {
        public ICommand artikalCommand { get; set; }
        public ICommand grupaCommand { get; set; }
        public ICommand tarifaCommand { get; set; }
        public ICommand partnerCommand { get; set; }
        public ICommand jmCommand { get; set; }
        public ICommand radnikCommand { get; set; }

        public ICommand kalkulacijaCommand { get; set; }


        private object selectedViewModel;

        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set { selectedViewModel = value; OnPropertyChanged("SelectedViewModel"); }
        }


        public NavigationViewModel()
        {
            artikalCommand = new BaseCommand(Openartikal);
            grupaCommand = new BaseCommand(Opengrupa);
            tarifaCommand = new BaseCommand(Opentarifa);
            partnerCommand = new BaseCommand(Openpartner);
            jmCommand = new BaseCommand(OpenJM);
            radnikCommand = new BaseCommand(OpenRadnik);

            kalkulacijaCommand = new BaseCommand(OpenKalkulacija);

        }

        private void Openartikal(object obj)
        {
            SelectedViewModel = new ArtikalView();
        }

        private void Opengrupa(object obj)
        {
            SelectedViewModel = new GrupaView();
        }

        private void Opentarifa(object obj)
        {
            SelectedViewModel = new TarifaView();
        }

        private void Openpartner(object obj)
        {
            SelectedViewModel = new PartnerView();
        }

        private void OpenJM(object obj)
        {
            SelectedViewModel = new JMView();
        }

        private void OpenRadnik(object obj)
        {
            SelectedViewModel = new RadnikView();
        }

        private void OpenKalkulacija(object obj)
        {
            SelectedViewModel = new KalkulacijaView();
        }




        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

    public class BaseCommand : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<object> _method;
        public event EventHandler CanExecuteChanged;

        public BaseCommand(Action<object> method)
            : this(method, null)
        {
        }

        public BaseCommand(Action<object> method, Predicate<object> canExecute)
        {
            _method = method;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _method.Invoke(parameter);
        }
    }
}
