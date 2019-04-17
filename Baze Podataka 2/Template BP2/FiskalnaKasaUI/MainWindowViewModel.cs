using FiskalnaKasaUI.Model;
using FiskalnaKasaUI.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FiskalnaKasaUI
{
    public class MainWindowViewModel : BindableBase
    {
 
        private NavigationViewModel navCommand;

        public List<MyMenuItem> TheMenu { get; set; }
        public NavigationViewModel NavCommand
        {
            get { return navCommand; }
            set { navCommand = value; OnPropertyChanged("NavCommand"); }
        }

        public MainWindowViewModel()
        {
            navCommand = new NavigationViewModel();

            TheMenu = new List<MyMenuItem>
            {
                new MyMenuItem { Header = "Artikli", Hint="",Command =  navCommand.artikalCommand, IconSource = "F1 M 19,50L 28,34L 63,34L 54,50L 19,50 Z M 19,28.0001L 35,28C 36,25 37.4999,24.0001 37.4999,24.0001L 48.75,24C 49.3023,24 50,24.6977 50,25.25L 50,28L 53.9999,28.0001L 53.9999,32L 27,32L 19,46.4L 19,28.0001 Z " },
                new MyMenuItem { Header = "TARIFNE GRUPE", Hint="", Command =  navCommand.tarifaCommand, IconSource = "F1 M 19,50L 28,34L 63,34L 54,50L 19,50 Z M 19,28.0001L 35,28C 36,25 37.4999,24.0001 37.4999,24.0001L 48.75,24C 49.3023,24 50,24.6977 50,25.25L 50,28L 53.9999,28.0001L 53.9999,32L 27,32L 19,46.4L 19,28.0001 Z " },
                new MyMenuItem { Header = "GRUPE ARTIKALA", Hint="",Command =  navCommand.grupaCommand, IconSource = "F1 M 19,50L 28,34L 63,34L 54,50L 19,50 Z M 19,28.0001L 35,28C 36,25 37.4999,24.0001 37.4999,24.0001L 48.75,24C 49.3023,24 50,24.6977 50,25.25L 50,28L 53.9999,28.0001L 53.9999,32L 27,32L 19,46.4L 19,28.0001 Z " },
                new MyMenuItem { Header = "PARTNERI", Hint="",Command =  navCommand.partnerCommand, IconSource = "F1 M 19,50L 28,34L 63,34L 54,50L 19,50 Z M 19,28.0001L 35,28C 36,25 37.4999,24.0001 37.4999,24.0001L 48.75,24C 49.3023,24 50,24.6977 50,25.25L 50,28L 53.9999,28.0001L 53.9999,32L 27,32L 19,46.4L 19,28.0001 Z " },
                new MyMenuItem { Header = "Jedinice mere", Hint="",Command =  navCommand.jmCommand, IconSource = "F1 M 19,50L 28,34L 63,34L 54,50L 19,50 Z M 19,28.0001L 35,28C 36,25 37.4999,24.0001 37.4999,24.0001L 48.75,24C 49.3023,24 50,24.6977 50,25.25L 50,28L 53.9999,28.0001L 53.9999,32L 27,32L 19,46.4L 19,28.0001 Z " },
                new MyMenuItem { Header = "Radnik", Hint="",Command =  navCommand.radnikCommand, IconSource = "F1 M 19,50L 28,34L 63,34L 54,50L 19,50 Z M 19,28.0001L 35,28C 36,25 37.4999,24.0001 37.4999,24.0001L 48.75,24C 49.3023,24 50,24.6977 50,25.25L 50,28L 53.9999,28.0001L 53.9999,32L 27,32L 19,46.4L 19,28.0001 Z " },


                new MyMenuItem { Header = "KALKULACIJE", Hint="",Command =  navCommand.kalkulacijaCommand, IconSource = "F1 M 25.3333,52.25L 50.6666,52.25L 50.6666,45.9167L 55.4166,45.9167L 55.4166,57L 20.5833,57L 20.5833,45.9167L 25.3333,45.9167L 25.3333,52.25 Z M 34.8333,15.8333L 41.1667,15.8333L 41.1667,36.4167L 49.0833,26.9167L 49.0833,36.4167L 38,49.0833L 26.9167,36.4167L 26.9167,26.9167L 34.8333,36.4167L 34.8333,15.8333 Z " },
                new MyMenuItem { Header = "KASA RACUNI", Hint="",Command =  navCommand.artikalCommand, IconSource = "F1 M 25.3333,52.25L 50.6667,52.25L 50.6667,45.9167L 55.4167,45.9167L 55.4167,57L 50.6667,57L 25.3333,57L 20.5833,57L 20.5833,45.9167L 25.3333,45.9167L 25.3333,52.25 Z M 34.8333,49.0834L 41.1667,49.0834L 41.1666,28.5L 49.0833,38L 49.0833,28.5L 38,15.8334L 26.9167,28.5L 26.9167,38L 34.8333,28.5L 34.8333,49.0834 Z " },

            };
 
        }

    }

    public class BindableBase : INotifyPropertyChanged
    {

        protected virtual void SetProperty<T>(ref T member, T val,
           [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
