using MyImageViewer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImageViewer.ViewModel
{
    public class AccountDetailViewModel : ValidationBase
    {
        private string pass1 = String.Empty ;
        private string pass2 = String.Empty;

        public AccountDetailViewModel()
        {
            ChangePasswordCommand = new MyICommand(DoChangePassword);
        }

        private void DoChangePassword()
        {
            this.Validate();
            if (this.IsValid)
            {
                User activeuser = UserRepository.GetActiveKorisnik();
                activeuser.Password = Password1;

                UserRepository.EditKorisnik(activeuser);
            }
        }

        public MyICommand ChangePasswordCommand { get; private set; }
        public UserRepository UserRepository
        {
            get { return MainWindow.m_UserRepository; }
        }

        public string Password1
        {
            get { return pass1; }
            set
            {
                if (value != pass1)
                {
                    pass1 = value;
                    OnPropertyChanged("Password1");
                }
            }
        }
        public string Password2
        {
            get { return pass2; }
            set
            {
                if (value != pass2)
                {
                    pass2 = value;
                    OnPropertyChanged("Password2");
                }
            }
        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this.Password1))
            {
                this.ValidationErrors["Password1"] = "Password1 cannot be empty.";
            }
            if (string.IsNullOrWhiteSpace(this.Password2))
            {
                this.ValidationErrors["Password2"] = "Password2 cannot be empty.";
            }
            if (this.Password1 != this.Password2)
            {
                this.ValidationErrors["Password1"] = "Not correct.";
            }
        }

    }
}
