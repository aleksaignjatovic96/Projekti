using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyImageViewer.Model
{
    public class UserRepository 
    {

        private List<User> sviKorisnici;
        private XDocument KorisnikData;
        private string activeuser = String.Empty;

        public UserRepository()
        {
            try
            {
                XDocument document = new XDocument();

                if (!File.Exists("KorisnikSlike.xml"))
                {
                    document.Add(new XElement("Image"));
                    document.Save("KorisnikSlike.xml");
                }

                document = new XDocument();

                if (!File.Exists("Korisnik.xml"))
                {
                    document.Add(new XElement("User"));
                    document.Save("Korisnik.xml");
                }


                sviKorisnici = new List<User>();
                KorisnikData = XDocument.Load("Korisnik.xml");
                var Korisnici = from t in KorisnikData.Descendants("item")
                                select new User(
                                t.Element("UserName").Value,
                                t.Element("Password").Value);

                sviKorisnici.AddRange(Korisnici.ToList<User>());
            }
            catch (Exception err)
            {

                //   throw new NotImplementedException();
            }
        }

        public IEnumerable<User> UzmiKorisnike()
        {
            return sviKorisnici;
        }

        public void SetActiveKorisnik(string korisnickoime)
        {
            activeuser = korisnickoime;
        }
        public User GetActiveKorisnik()
        {
            return sviKorisnici.Find(item => item.UserName == activeuser);
        }

        public User GetKorisnik(string korisnickoime)
        {
            return sviKorisnici.Find(item => item.UserName == korisnickoime);
        }

        public User GetKorisnik(string korisnickoime, string lozinka)
        {
            return sviKorisnici.Find(item => item.UserName == korisnickoime && item.Password == lozinka);
        }

        public void KorisnikRefresh()
        {
            sviKorisnici = new List<User>();
            KorisnikData = XDocument.Load("Korisnik.xml");
            var Korisnici = from t in KorisnikData.Descendants("item")
                            select new User(
                            t.Element("UserName").Value,
                            t.Element("Password").Value);

            sviKorisnici.AddRange(Korisnici.ToList<User>());
        }

        public void InsertKorisnik(User Korisnik)
        {

            KorisnikData.Root.Add(new XElement("item",
                new XElement("UserName", Korisnik.UserName),
                new XElement("Password", Korisnik.Password)
                ));


            KorisnikData.Save("Korisnik.xml");
        }


        public void EditKorisnik(User korisnik)
        {
            try
            {
                XElement node = KorisnikData.Root.Elements("item").Where(i => (string)i.Element("UserName") == korisnik.UserName).FirstOrDefault();

                node.SetElementValue("UserName", korisnik.UserName);
                node.SetElementValue("Password", korisnik.Password);
                KorisnikData.Save("Korisnik.xml");
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }



        public void InsertKorisnikSlike(User korisnik,MyImage slika)
        {
            XDocument KorisnikSlikeData;
            KorisnikSlikeData = XDocument.Load("KorisnikSlike.xml");

            KorisnikSlikeData.Root.Add(new XElement("item",
                new XAttribute("UserName", korisnik.UserName),
                new XElement("ImageTitle", slika.ImageTitle),
                new XElement("ImageDescription", slika.ImageDescription),
                new XElement("ImageSource", slika.ImageSource)
                ));


            KorisnikSlikeData.Save("KorisnikSlike.xml");
        }

        public List<MyImage> GetKorisnikSlike(string korisnickoime)
        {
            List<MyImage> sveSlike = new List<MyImage>();

            XDocument KorisnikSlikeData;
            KorisnikSlikeData = XDocument.Load("KorisnikSlike.xml");

            var Korisnici = from t in KorisnikSlikeData.Root.Elements()
                            where t.Attribute("UserName").Value.Contains(korisnickoime)
                            select new MyImage(
                            t.Element("ImageTitle").Value,
                            t.Element("ImageDescription").Value,
                            t.Element("ImageSource").Value);



            sveSlike.AddRange(Korisnici.ToList<MyImage>());

            return sveSlike;
        }

    }
}
