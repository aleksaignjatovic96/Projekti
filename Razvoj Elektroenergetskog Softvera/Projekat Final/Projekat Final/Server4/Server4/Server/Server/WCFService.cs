using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.ServiceModel;
using System.Xml.Serialization;

namespace Server
{
    public class WCFService : IIspad
    {
        public void UnosPodatakaOIspadu(int id, DateTime vreme, Naponski_Nivo naponski_Nivo, string opis, Status status, Common.Element element, List<Akcija> listaAkcija, int radnja)
        {
            Ispad ispad = new Ispad(id, vreme, opis, element, listaAkcija);

            if (File.Exists("bazaIspadi.xml"))
            {

                ListaSvihIspada lista = UcitajIzXml();

                Validacija(lista, id, element.Id, radnja);

                if (radnja == 1)
                {
                    int index = -1;
                    for (int i = 0; i < lista.UkupnaListaSvihIspada.Count; i++)
                    {
                        if (lista.UkupnaListaSvihIspada[i].Id == id)
                        {
                            index = i;
                            break;
                        }
                    }

                    if (index == -1)
                    {
                        MyException e = new MyException();
                        e.Greska = "Nemoguce naci postojeci ID!";

                        ActionLogs("GRESKA, nemoguce naci ID: " + element.Id);
                        throw new FaultException<MyException>(e);
                    }

                    lista.UkupnaListaSvihIspada[index] = ispad;
                    lista.UkupnaListaSvihIspada[index].Napon = naponski_Nivo;
                    lista.UkupnaListaSvihIspada[index].Status = status;
                    UpisiUXml(lista);
                    ActionLogs("Promena ispada sa ID: " + id);
                    return;
                }

                if (radnja == 0)
                {
                    lista.DodajIspad(ispad);
                    UpisiUXml(lista);
                    ActionLogs("Dodavanje Ispada: " + id);
                    return;
                }
                

                
            }

            //Do ovde dolazi samo ako baza ne postoji!
            ListaSvihIspada listaNova = new ListaSvihIspada();
            listaNova.DodajIspad(ispad);
            UpisiUXml(listaNova);
            ActionLogs("Dodavanje Ispada: " + id);
        }


        public List<Ispad> PrikaziSveIspade()
        {
            ListaSvihIspada lista = UcitajIzXml();

            if (lista.UkupnaListaSvihIspada.Count == 0)
            {
                
                MyException e = new MyException();
                e.Greska = "Baza je prazna!";

                ActionLogs("GRESKA, pokusaj citanja iz prazne baze");
                throw new FaultException<MyException>(e);
            }

            ActionLogs("Prikaz svih ispada");

            return lista.UkupnaListaSvihIspada;
        }

        public Ispad PrikaziOdredjeniIspad(int id)
        {
            Ispad ispad = new Ispad();
            ListaSvihIspada lista = UcitajIzXml();
            int pom = 0;

            foreach(Ispad item in lista.UkupnaListaSvihIspada)
            {
                if (item.Id == id)
                {
                    pom = 1;
                    ispad = item;
                }
            }

            if (pom == 0)
            {
                MyException e = new MyException();
                e.Greska = "Ne postoji takav ispad u bazi!";
                ActionLogs("GRESKA, pokusaj citanja nepostojeceg ispada sa ID: " + id);
                throw new FaultException<MyException>(e);
            }

            ActionLogs("Prikaz Ispada sa ID: " + id);


            return ispad;
        }

        public void KreirajDokument(int id, string nazivElementa, List<Akcija> spisakAkcija)
        {
            System.IO.FileStream fs = new FileStream("Ispad" + id + ".pdf", FileMode.Create);

            // Create an instance of the document class which represents the PDF document itself.
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            // Create an instance to the PDF file by creating an instance of the PDF 
            // Writer class using the document and the filestrem in the constructor.

            PdfWriter writer = PdfWriter.GetInstance(document, fs);

            // Open the document to enable you to write to the document

            document.Open();

            // Add a simple and wellknown phrase to the document in a flow layout manner

            document.Add(new Paragraph("ID: " + id));
            document.Add(new Paragraph("Naziv Elementa: " + nazivElementa));
            document.Add(new Paragraph("Spisak Akcija:"));
            document.Add(new Paragraph("----------------"));

            foreach (Akcija akcija in spisakAkcija)
            {
                document.Add(new Paragraph(akcija.OpisAkcije));
            }
            document.Add(new Paragraph("----------------"));

            // Close the document

            document.Close();
            // Close the writer instance

            writer.Close();
            // Always close open filehandles explicity
            fs.Close();


            ActionLogs(("Kreiranje novog dokumenta za Ispad sa ID: " + id));
        }

        public void UpisiUXml(ListaSvihIspada ListaZaBazu)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ListaSvihIspada));
            using (TextWriter textWriter = new StreamWriter("bazaIspadi.xml"))
            {
                serializer.Serialize(textWriter, ListaZaBazu);
            }

           
        }

        public ListaSvihIspada UcitajIzXml()
        {
            if (File.Exists("bazaIspadi.xml"))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(ListaSvihIspada));
                using (TextReader textReader = new StreamReader("bazaIspadi.xml"))
                {

                    object obj = deserializer.Deserialize(textReader);
                    ListaSvihIspada ListaZaBazu = (ListaSvihIspada)obj;

                    return ListaZaBazu;

                }
            }
            else
            {
                MyException e = new MyException();
                e.Greska = "Baza je prazna!";
                ActionLogs("GRESKA, pokusaj citanja iz prazne baze");
                throw new FaultException<MyException>(e);
            }

        }

        private void ActionLogs(string actionDone)
        {
            StreamWriter file = new StreamWriter("actionLogs.txt", true);

            
            file.WriteLine("|==============================================================|");
            file.WriteLine("Dogadjaj: " + actionDone);
            file.WriteLine("<==============================================================>");
            file.WriteLine("Datum i Vreme: " + DateTime.Now.ToString());
            file.WriteLine("|==============================================================|");
            file.WriteLine("");

            file.Close();
        }

        public void Validacija(ListaSvihIspada lista, int idIspada, string idElementa, int radnja)
        {

            if (radnja == 0)
            {

                for (int i = 0; i < lista.UkupnaListaSvihIspada.Count; i++)
                {

                    if (lista.UkupnaListaSvihIspada.ElementAt(i).Element.Id == idElementa)
                    {
                        MyException e = new MyException();
                        e.Greska = "Postoji vec element sa tim ID!";

                        ActionLogs("GRESKA, pokusaj dodavanja postojeceg elementa: " + idElementa);
                        throw new FaultException<MyException>(e);
                    }

                    
                        if (lista.UkupnaListaSvihIspada.ElementAt(i).Id == idIspada)
                        {
                            MyException e = new MyException();
                            e.Greska = "Postoji vec ispad sa tim ID!";

                            ActionLogs("GRESKA, pokusaj dodavanja postojeceg ispada: " + idIspada);
                            throw new FaultException<MyException>(e);
                        } 
                }
                
            }

            else if (radnja == 1)
                {

                for (int i = 0; i < lista.UkupnaListaSvihIspada.Count; i++)
                {

                    if (lista.UkupnaListaSvihIspada.ElementAt(i).Element.Id == idElementa && lista.UkupnaListaSvihIspada.ElementAt(i).Id != idIspada)
                    {
                        MyException e = new MyException();
                        e.Greska = "Postoji vec element sa tim ID!";

                        ActionLogs("GRESKA, pokusaj dodavanja postojeceg elementa: " + idElementa);
                        throw new FaultException<MyException>(e);
                    }

                }
            }

        }
    }
}

