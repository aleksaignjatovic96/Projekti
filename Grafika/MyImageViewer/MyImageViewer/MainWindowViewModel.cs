using MyImageViewer.Model;
using MyImageViewer.ViewModel;
using MyImageViewer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImageViewer
{
    class MainWindowViewModel : BindableBase
    {
        private string menuvisibility = "Hidden";

        private BindableBase currentViewModel;

        private LoginViewModel loginViewModel = new LoginViewModel();
        private MyImageViewModel myImageViewModel = new MyImageViewModel();
        private AddImageViewModel addImageViewModel = new AddImageViewModel();
        private AccountDetailViewModel accountDetailViewModel = new AccountDetailViewModel();

        public MainWindowViewModel()
        {
            CurrentViewModel = loginViewModel;

            NavCommand = new MyICommand<string>(OnNav);

            TheMenu = new List<MyMenuItem>
            {
                new MyMenuItem { Header = "My Image", Command = NavCommand, IconSource = "/MyImageViewer;component/Resource/myimage.png" },
                new MyMenuItem { Header = "Add Image", Command = NavCommand, IconSource = "/MyImageViewer;component/Resource/myimageadd.png" },
                new MyMenuItem { Header = "Account", Command = NavCommand, IconSource = "/MyImageViewer;component/Resource/account.png" },
                new MyMenuItem { Header = "Logout", Command = NavCommand, IconSource = "/MyImageViewer;component/Resource/logout.png" },
            };

            loginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
        }

        private void LoginViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (loginViewModel.IsAuthenticated == 1)
            {
                myImageViewModel.ImageListRefresh();
                CurrentViewModel = myImageViewModel;
                MenuVisibility = "Visible";
            }
            else if (loginViewModel.IsAuthenticated == 2)
            {
                CurrentViewModel = addImageViewModel;
                MenuVisibility = "Visible";
            }
            else
            {
                CurrentViewModel = loginViewModel;
                MenuVisibility = "Hidden";
            }

        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "My Image":
                    myImageViewModel.ImageListRefresh();
                    CurrentViewModel = myImageViewModel;
                    break;
                case "Add Image":
                    CurrentViewModel = addImageViewModel;
                    break;
                case "Account":
                    CurrentViewModel = accountDetailViewModel;
                    break;
                case "Logout":
                    loginViewModel.IsAuthenticated = 0;
                    break;
                default:
                    break;
                
            }
        }


        #region Property

        public MyICommand<string> NavCommand { get; private set; }
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }

        public string MenuVisibility
        {
            get { return menuvisibility; }
            set
            {
                SetProperty(ref menuvisibility, value);
                OnPropertyChanged("MenuVisibility");
            }
        }

        public List<MyMenuItem> TheMenu { get; set; }


        #endregion // Property

    }
}
