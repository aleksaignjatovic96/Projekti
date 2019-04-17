using MyImageViewer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImageViewer.ViewModel
{
    public class LoginViewModel : BindableBase
    {
        private User loginuser;
        private int isauthenticated = 0;
        private string loginerror = String.Empty;

        public LoginViewModel()
        {
            LoginUser = new User { UserName = "Name", Password = "Password" };

            LoginCommand = new MyICommand(DoLogin);
            RegistrationCommand = new MyICommand(DoRegistration);
        }

        private void DoLogin()
        {
            LoginUser.Validate();

            if (LoginUser.IsValid)
            {
                if (UserRepository.GetKorisnik(LoginUser.UserName, LoginUser.Password) != null)
                {
                    UserRepository.SetActiveKorisnik(LoginUser.UserName);

                    IsAuthenticated = 1;
                    LoginError = String.Empty;
                }
                else
                {
                    UserRepository.SetActiveKorisnik(String.Empty);
                    LoginError = "Username or password incorrect!";
                }
            }
            
        }
        private void DoRegistration()
        {

            LoginUser.Validate();

            if (LoginUser.IsValid)
            {

                if (UserRepository.GetKorisnik(LoginUser.UserName) == null)
                {
                    UserRepository.InsertKorisnik(new User(LoginUser.UserName, LoginUser.Password));

                    UserRepository.KorisnikRefresh();
                    UserRepository.SetActiveKorisnik(LoginUser.UserName);

                    IsAuthenticated = 2;
                    LoginError = String.Empty;
                }
                else
                {
                    LoginError = "This user already exists!";
                }
            }
        }

        public User LoginUser
        {
            get { return loginuser; }
            set

            {
                if (value != loginuser)
                {
                    loginuser = value;
                    OnPropertyChanged("LoginUser");
                }
            }
        }
        public int IsAuthenticated
        {
            get { return isauthenticated; }
            set
            {
                if (value != isauthenticated)
                {
                    isauthenticated = value;
                    OnPropertyChanged("IsAuthenticated");
                }
            }
        }
        public string LoginError
        {
            get { return loginerror; }
            set
            {
                if (value != loginerror)
                {
                    loginerror = value;
                    OnPropertyChanged("LoginError");
                }
            }
        }

        public MyICommand LoginCommand { get; private set; }
        public MyICommand RegistrationCommand { get; private set; }


        public UserRepository UserRepository
        {
            get { return MainWindow.m_UserRepository; }
        }



    }
}
