using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImageViewer.Model
{
    public class User : ValidationBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }



        public User( )
        {

        }
        public User(string korisnickoIme, string lozinka)
        {
            this.UserName = korisnickoIme;
            this.Password = lozinka;

        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this.UserName))
            {
                this.ValidationErrors["UserName"] = "UserName cannot be empty.";
            }
            if (string.IsNullOrWhiteSpace(this.Password))
            {
                this.ValidationErrors["Password"] = "Password cannot be empty.";
            }
            if (!string.IsNullOrEmpty(this.UserName) && !Char.IsLetter(this.UserName[0]))
            {
                this.ValidationErrors["UserName"] = "Username cannot start with number.";
            }
            if (this.Password.Length <= 6)
            {
                this.ValidationErrors["Password"] = "Password must be longer than 6 characters.";
            }
        }
    }
}
