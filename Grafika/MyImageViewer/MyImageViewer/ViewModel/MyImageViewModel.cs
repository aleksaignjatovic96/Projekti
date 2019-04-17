using MyImageViewer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImageViewer.ViewModel
{
    public class MyImageViewModel : BindableBase
    {
        public List<MyImage> ImageList { get; set; }

        public MyImageViewModel()
        {

        }

        public void ImageListRefresh()
        {
            User activeuser = UserRepository.GetActiveKorisnik();

            ImageList = UserRepository.GetKorisnikSlike(activeuser.UserName);

        }

        public UserRepository UserRepository
        {
            get { return MainWindow.m_UserRepository; }
        }
    }


}
