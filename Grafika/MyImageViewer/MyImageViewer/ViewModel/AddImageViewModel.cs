using Microsoft.Win32;
using MyImageViewer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImageViewer.ViewModel
{
    public class AddImageViewModel : BindableBase
    {
        //private string addimagesource = "/MyImageViewer;component/Resource/addimage.png";

        private MyImage newimage = new MyImage();

        public AddImageViewModel()
        {
            LoadImageCommand = new MyICommand(DoLoadImage);
            AddImageCommand = new MyICommand(DoAddImage);
        }

        private void DoLoadImage()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                Newimage.ImageTitle = op.SafeFileName;
                Newimage.ImageSource = op.FileName;
            }
        }

        private void DoAddImage()
        {
            Newimage.Validate();
            if (Newimage.IsValid)
            {
                User activeuser = UserRepository.GetActiveKorisnik();

                UserRepository.InsertKorisnikSlike(activeuser, Newimage);

                newimage.ImageSource = String.Empty;
                newimage.ImageTitle = String.Empty;
                newimage.ImageDescription = String.Empty;
            }
        }


        public MyICommand LoadImageCommand { get; private set; }
        public MyICommand AddImageCommand { get; private set; }

        public MyImage Newimage
        {
            get { return newimage; }
            set
            {
                if (value != newimage)
                {
                    newimage = value;
                    OnPropertyChanged("Newimage");
                }
            }
        }


        public UserRepository UserRepository
        {
            get { return MainWindow.m_UserRepository; }
        }


    }
}
