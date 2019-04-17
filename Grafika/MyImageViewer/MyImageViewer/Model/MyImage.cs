using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImageViewer.Model
{
    public class MyImage : ValidationBase
    {
        private string imagetitle = String.Empty;
        private string imagedescription = String.Empty;
        private string imagesource = String.Empty;

        public string ImageTitle
        {
            get { return imagetitle; }
            set
            {
                if (value != imagetitle)
                {
                    imagetitle = value;
                    OnPropertyChanged("ImageTitle");
                }
            }
        }
        public string ImageDescription
        {
            get { return imagedescription; }
            set
            {
                if (value != imagedescription)
                {
                    imagedescription = value;
                    OnPropertyChanged("ImageDescription");
                }
            }
        }
        public string ImageSource
        {
            get { return imagesource; }
            set
            {
                if (value != imagesource)
                {
                    imagesource = value;
                    OnPropertyChanged("ImageSource");
                }
            }
        }




        public MyImage()
        {

        }
        public MyImage(string imagetitle, string imagedescription, string imagesource)
        {
            this.ImageTitle = imagetitle;
            this.ImageDescription = imagedescription;
            this.ImageSource = imagesource;
        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this.ImageTitle))
            {
                this.ValidationErrors["ImageTitle"] = "ImageTitle cannot be empty.";
            }
            if (string.IsNullOrWhiteSpace(this.ImageSource))
            {
                this.ValidationErrors["ImageSource"] = "ImageSource cannot be empty.";
            }
        }

    }
}
