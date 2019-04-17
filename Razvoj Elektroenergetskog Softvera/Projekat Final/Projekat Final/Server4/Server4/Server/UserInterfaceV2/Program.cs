using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.ServiceModel;

namespace UserInterfaceV2
{
    class Program
    {
        static IIspad ispad;
        //static int i = 0;
        static void Main(string[] args)
        {
            Connecting();
            Menu();
            Console.ReadKey();
        }

        public static void Connecting()
        {
            ChannelFactory<IIspad> factory = new ChannelFactory<IIspad>(
                                                            new NetTcpBinding(),
                                                            new EndpointAddress("net.tcp://localhost:6015/IIspad"));

            ispad = factory.CreateChannel();
        }

        public static void Menu()
        {
            bool tempBool = true;

            while (tempBool)
            {
                Console.WriteLine("\\V//OMS Glavni meni:\\V//");
                Console.WriteLine("\t1. Unesi podatke o novom ispadu");
                Console.WriteLine("\t2. Ispisi sve postojece ispade");
                Console.WriteLine("\t3. Ispisi odredjeni ispad");
                Console.WriteLine("\t4. Kreiraj Novi Dokument");
                Console.WriteLine("\t5. Zatvori program");
                Console.WriteLine("//A\\Izaberi jednu od ponudjenih funkcija.//A\\");
                string temp = Console.ReadLine();

                switch (temp)
                {
                    case "1":
                        Console.Clear();
                        CreateNewException();
                        Console.Clear();
                        break;
                    case "2":
                        Console.Clear();
                        ListAllExceptions();
                        Console.Clear();
                        break;
                    case "3":
                        Console.Clear();
                        ListSpecificException();
                        Console.Clear();
                        break;
                    case "4":
                        Console.Clear();
                        CreateNewDocument();
                        Console.Clear();
                        break;
                    case "5":
                        Console.Clear();
                        tempBool = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Neispravan Unos molimo vas da unesete jednu od ponudjenih opcija (1-5)");
                        break;

                }
            }
        }

        public static void CreateNewException()
        {
            try
            {
                Console.Write("Unesi ID za ispad: ");
                int tempId = Int32.Parse(Console.ReadLine());
                Element tempEle = new Element();
                Console.WriteLine("Unesi podatke o elementu: ");
                Console.Write("ID: ");
                tempEle.Id = Console.ReadLine();
                Console.Write("Naziv: ");
                tempEle.Naziv = Console.ReadLine();
                Console.Write("X: ");
                tempEle.X = float.Parse(Console.ReadLine());
                Console.Write("Y: ");
                tempEle.Y = float.Parse(Console.ReadLine());
                Console.Write("Vreme ispada [Sat/Minut/Sekunda/Dan/Mesec/Godina]: ");
                string unosIspad = Console.ReadLine();
                string[] vremeTempIspad = unosIspad.Split('/');

                DateTime tempVremeIspad = new DateTime(Int32.Parse(vremeTempIspad[5]), Int32.Parse(vremeTempIspad[4]), Int32.Parse(vremeTempIspad[3]), Int32.Parse(vremeTempIspad[0]), Int32.Parse(vremeTempIspad[1]), Int32.Parse(vremeTempIspad[2]));


                List<Akcija> tempActions = new List<Akcija>();

                bool actionInsertion = true;

                string tempString;
                Console.WriteLine("------------------------------");
                while (actionInsertion)
                {
                    Akcija tempAkcija = new Akcija();
                    Console.WriteLine("Unesi podatke o akcijama: ");
                    Console.Write("Opis akcije: ");
                    tempAkcija.OpisAkcije = Console.ReadLine();
                    Console.Write("Vreme akcije [Sat/Minut/Sekunda/Dan/Mesec/Godina]: ");
                    string unos = Console.ReadLine();
                    string[] vremeTemp = unos.Split('/');

                    DateTime tempVreme = new DateTime(Int32.Parse(vremeTemp[5]), Int32.Parse(vremeTemp[4]), Int32.Parse(vremeTemp[3]), Int32.Parse(vremeTemp[0]), Int32.Parse(vremeTemp[1]), Int32.Parse(vremeTemp[2]));

                    tempAkcija.Vreme = tempVreme;

                    tempActions.Add(tempAkcija);

                    Console.Write("Ako hocete da zavrsite sa dodavanjem akcija napisite [Done]");
                    tempString = Console.ReadLine();
                    if (tempString == "Done")
                    {
                        actionInsertion = false;
                    }
                    Console.WriteLine("------------------------------");
                }

                Console.Write("Kratak opis ispada: ");
                string opis = Console.ReadLine();

                ispad.UnosPodatakaOIspadu(tempId, tempVremeIspad, Naponski_Nivo.SrednjiNapon, opis, Status.Novo, tempEle, tempActions, 0);
            }
            catch (FaultException<MyException> ex)
            {
                Console.WriteLine("Error : " + ex.Detail.Greska);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : " + e.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Pritisni dugme da se vratis na glavni meni.");
            Console.ReadKey();

        }

        public static void ListAllExceptions()
        {
            List<Ispad> listaIspada;

            try
            {
                listaIspada = ispad.PrikaziSveIspade();

                foreach (Ispad isp in listaIspada)
                {
                    Console.WriteLine("============================");
                    Console.WriteLine("ID ispada: " + isp.Id);
                    Console.WriteLine("Vreme ispada: " + isp.Vreme.Hour + ":" + isp.Vreme.Minute + ":" + isp.Vreme.Second);
                    Console.WriteLine("Datum ispada: " + isp.Vreme.Day + "." + isp.Vreme.Month + "." + isp.Vreme.Year + ".");
                    Console.WriteLine("Status ispada: " + isp.Status);
                    Console.WriteLine("Napon: " + isp.Napon);
                    Console.WriteLine("Opis ispada: " + isp.Opis);
                    Console.WriteLine("Element na kom se desio ispad: " + isp.Element.Naziv + " [" + isp.Element.Id + "]");
                    Console.WriteLine("Koordinate na kom se element nalazi: [" + isp.Element.X + ", " + isp.Element.Y + "]");
                    Console.WriteLine("============================");
                    Console.WriteLine("Sledece akcije su se desile: ");
                    int kk = 1;
                    foreach (Akcija tem in isp.ListaAkcija)
                    {
                        Console.WriteLine(kk++ + ". " + tem.OpisAkcije + " | " + "Vreme akcije: " + tem.Vreme.Hour + ":" + tem.Vreme.Minute + ":" + tem.Vreme.Second + " | " + "Datum akcije: " + tem.Vreme.Day + "." + tem.Vreme.Month + "." + tem.Vreme.Year + ".");
                    }
                    Console.WriteLine("============================");
                }
            }
            catch (FaultException<MyException> ex)
            {
                Console.WriteLine("Error : " + ex.Detail.Greska);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }

            Console.WriteLine("Pritisni dugme da se vratis na glavni meni.");
            Console.ReadKey();
        }

        public static void ListSpecificException()
        {
            try
            {
                Console.WriteLine("Izaberi ID ispada.");
                //string tempString = Console.ReadLine();
                int id = Int32.Parse(Console.ReadLine());
                Ispad temp = null;

                temp = ispad.PrikaziOdredjeniIspad(id);

                Console.WriteLine("============================");
                Console.WriteLine("ID ispada: " + id);
                Console.WriteLine("Vreme ispada: " + temp.Vreme.Hour + ":" + temp.Vreme.Minute + ":" + temp.Vreme.Second);
                Console.WriteLine("Datum ispada: " + temp.Vreme.Day + "." + temp.Vreme.Month + "." + temp.Vreme.Year + ".");
                Console.WriteLine("Status ispada: " + temp.Status);
                Console.WriteLine("Napon: " + temp.Napon);
                Console.WriteLine("Opis ispada: " + temp.Opis);
                Console.WriteLine("Element na kom se desio ispad: " + temp.Element.Naziv + " [" + temp.Element.Id + "]");
                Console.WriteLine("Koordinate na kom se element nalazi: [" + temp.Element.X + ", " + temp.Element.Y + "]");
                Console.WriteLine("============================");
                Console.WriteLine("Sledece akcije su se desile: ");
                int kk = 1;
                foreach (Akcija tem in temp.ListaAkcija)
                {
                    Console.WriteLine(kk++ + ". " + tem.OpisAkcije + " | " + "Vreme akcije: " + tem.Vreme.Hour + ":" + tem.Vreme.Minute + ":" + tem.Vreme.Second + " | " + "Datum akcije: " + tem.Vreme.Day + "." + tem.Vreme.Month + "." + tem.Vreme.Year + ".");
                }
                Console.WriteLine("============================");

                if (temp.Status == Status.Zatvoreno)
                {
                    Console.WriteLine("Nemoguce izmeniti, ispad je zatvoren!");
                }
                else
                {
                    Meni2(temp);
                    ispad.UnosPodatakaOIspadu(temp.Id, temp.Vreme, temp.Napon, temp.Opis, temp.Status, temp.Element, temp.ListaAkcija, 1);
                }
            }
            catch (FaultException<MyException> ex)
            {
                Console.WriteLine("Error : " + ex.Detail.Greska);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }

            Console.WriteLine("Pritisni dugme da se vratis na glavni meni.");
            Console.ReadKey();
        }

        public static void CreateNewDocument()
        {
            try
            {
                Console.Write("Izaberi ID ispada: ");
                //string tempString = Console.ReadLine();
                int id = Int32.Parse(Console.ReadLine());
                Ispad temp = null;

                temp = ispad.PrikaziOdredjeniIspad(id);
                ispad.KreirajDokument(temp.Id, temp.Element.Naziv, temp.ListaAkcija);
            }
            catch (FaultException<MyException> ex)
            {
                Console.WriteLine("Error : " + ex.Detail.Greska);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }

            Console.WriteLine("Pritisni dugme da se vratis na glavni meni.");
            Console.ReadKey();
        }

        public static void Meni2(Ispad poslat)
        {
            bool tempbool = true;
            while (tempbool)
            {
                Console.WriteLine("=====================================");
                Console.WriteLine("\\V//Promena elementata ispada:\\V//");
                Console.WriteLine("\t1. Promena opisa");
                Console.WriteLine("\t2. Promena elementa ispada");
                Console.WriteLine("\t3. Dodavanje novih akcija na ispad");
                Console.WriteLine("\t4. Promena status ispada");
                Console.WriteLine("\t5. Promena napona ispada");
                Console.WriteLine("\t6. Zavrsi sa menjanjem Ispada");
                Console.WriteLine("//A\\Izaberi jednu od ponudjenih funkcija.//A\\");
                string temp = Console.ReadLine();

                switch (temp)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("Unesite novi opis: ");
                        poslat.Opis = Console.ReadLine();
                        Console.Clear();
                        break;
                    case "2":
                        Console.Clear();
                        try
                        {
                            Console.Write("Unesite novi Id elementa: ");
                            poslat.Element.Id = Console.ReadLine();
                            Console.Clear();
                            Console.Write("Unesite novi Naziv elementa: ");
                            poslat.Element.Naziv = Console.ReadLine();
                            Console.Clear();
                            Console.Write("Unesite novu koordiantu X elementa: ");
                            poslat.Element.X = float.Parse(Console.ReadLine());
                            Console.Clear();
                            Console.Write("Unesite novu koordiantu Y elementa: ");
                            poslat.Element.Y = float.Parse(Console.ReadLine());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e.Message);
                            Console.ReadLine();
                        }
                        Console.Clear();
                        break;
                    case "3":
                        try
                        {
                            Console.Clear();
                            Akcija akcija = new Akcija();
                            Console.Write("Opisite vasu akciju: ");
                            akcija.OpisAkcije = Console.ReadLine();
                            Console.Write("Vreme akcije [Sat/Minut/Sekunda/Dan/Mesec/Godina]: ");
                            string unos = Console.ReadLine();
                            string[] vremeTemp = unos.Split('/');

                            DateTime tempVreme = new DateTime(Int32.Parse(vremeTemp[5]), Int32.Parse(vremeTemp[4]), Int32.Parse(vremeTemp[3]), Int32.Parse(vremeTemp[0]), Int32.Parse(vremeTemp[1]), Int32.Parse(vremeTemp[2]));

                            akcija.Vreme = tempVreme;
                            poslat.ListaAkcija.Add(akcija);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e.Message);
                            Console.ReadLine();
                        }
                        Console.Clear();
                        break;
                    case "4":
                        Console.Clear();
                        Console.WriteLine("Izaberite novi status ispada: ");
                        Console.WriteLine("\t1. Novo");
                        Console.WriteLine("\t2. Na Cekanju");
                        Console.WriteLine("\t3. U progresu");
                        Console.WriteLine("\t4. Testiranje");
                        Console.WriteLine("\t5. Zatvoreno");
                        string tempCase4 = Console.ReadLine();
                        Console.Clear();
                        if (tempCase4 == "1")
                        {
                            poslat.Status = Status.Novo;
                        }
                        else if (tempCase4 == "2")
                        {
                            poslat.Status = Status.NaCekanju;
                        }
                        else if (tempCase4 == "3")
                        {
                            poslat.Status = Status.UProgresu;
                        }
                        else if (tempCase4 == "4")
                        {
                            poslat.Status = Status.Testiranje;
                        }
                        else if (tempCase4 == "5")
                        {
                            poslat.Status = Status.Zatvoreno;
                        }
                        else
                        {
                            Console.WriteLine("Ne ispravan izbor");
                        }
                        break;
                    case "5":
                        Console.Clear();
                        Console.WriteLine("Izaberite novi Napon ispada: ");
                        Console.WriteLine("\t1. Srednji Napon");
                        Console.WriteLine("\t2. Visoki Napon");
                        Console.WriteLine("\t3. Niski Napon");
                        string tempCase5 = Console.ReadLine();
                        Console.Clear();
                        if (tempCase5 == "1")
                        {
                            poslat.Napon = Naponski_Nivo.SrednjiNapon;
                        }
                        else if (tempCase5 == "2")
                        {
                            poslat.Napon = Naponski_Nivo.VisokiNapon;
                        }
                        else if (tempCase5 == "3")
                        {
                            poslat.Napon = Naponski_Nivo.NizakNapon;
                        }
                        else
                        {
                            Console.WriteLine("Ne ispravan izbor");
                        }
                        break;
                    case "6":
                        Console.Clear();
                        tempbool = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Neispravan Unos molimo vas da unesete jednu od ponudjenih opcija (1-6)");
                        break;

                }
            }
        }
    }
}
