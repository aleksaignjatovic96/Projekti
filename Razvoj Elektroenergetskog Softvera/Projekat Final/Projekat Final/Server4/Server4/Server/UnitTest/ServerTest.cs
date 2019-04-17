using Common;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;
using System.IO;
using System.ServiceModel;

namespace UnitTest
{
    [TestFixture]
    public class ServerTest
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            var dir = Path.GetDirectoryName(typeof(WCFService).Assembly.Location);
            Directory.SetCurrentDirectory(dir);

        }

        [Test]
        [TestCaseSource("NewUnos")]
        public void UnosIspadaDobar(int id, DateTime vreme, Naponski_Nivo naponski_Nivo, string opis, Status status, Common.Element element, List<Akcija> listaAkcija, int radnja)
        {
            WCFService testInstance = new WCFService();
            Assert.DoesNotThrow(() => testInstance.UnosPodatakaOIspadu(id, vreme, naponski_Nivo, opis, status, element, listaAkcija, radnja));
        }

        static object[] NewUnos =
        {
            new object[] { 78, new DateTime(2018, 12, 12, 12, 12, 12), Naponski_Nivo.SrednjiNapon, "Kvar Tranformatora", Status.Novo, new Element("TU4", "Test1", 45, 45), new List<Akcija>(){new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12)) }, 0 },
            new object[] { 78, new DateTime(2018, 12, 12, 12, 12, 12), Naponski_Nivo.VisokiNapon, "Kratak Spoj", Status.NaCekanju, new Element("TU5", "Test2", 45, 45), new List<Akcija>(){new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12)) } , 1 },
            new object[] { 78, new DateTime(2018, 12, 12, 12, 12, 12), Naponski_Nivo.NizakNapon, "Nepravilno merenje", Status.UProgresu, new Element("TU6", "Test3", 45, 45), new List<Akcija>() { new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12)) }, 1 }
        };

        [Test]
        [TestCaseSource("NewUnos1")]
        public void UnosIspadaLos(int id, DateTime vreme, Naponski_Nivo naponski_Nivo, string opis, Status status, Common.Element element, List<Akcija> listaAkcija, int radnja)
        {
            WCFService testInstance = new WCFService();
            Assert.Throws<FaultException<MyException>>(() => testInstance.UnosPodatakaOIspadu(id, vreme, naponski_Nivo, opis, status, element, listaAkcija, radnja));
        }
        static object[] NewUnos1 =
        {
            new object[] { 78, new DateTime(2018, 12, 12, 12, 12, 12), Naponski_Nivo.SrednjiNapon, "Kvar Tranformatora", Status.Novo, new Element("TU4", "Test1", 45, 45), new List<Akcija>(){new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12)) }, 0 },
            new object[] { 128, new DateTime(2018, 12, 12, 12, 12, 12), Naponski_Nivo.VisokiNapon, "Kratak Spoj", Status.NaCekanju, new Element("TU5", "Test2", 45, 45), new List<Akcija>(){new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12)) } , 1 },
            new object[] { 77, new DateTime(2018, 12, 12, 12, 12, 12), Naponski_Nivo.VisokiNapon, "Kratak Spoj", Status.NaCekanju, new Element("TU6", "Test2", 45, 45), new List<Akcija>(){new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12)) } , 0 }
        };

        [Test]
        [TestCaseSource("NewPrikaz")]
        public void PrikazOdredjenogDobar(int id)
        {
            WCFService testInstance = new WCFService();
            Assert.DoesNotThrow(() => testInstance.PrikaziOdredjeniIspad(id));
        }
        static object[] NewPrikaz =
        {
            new object[] { 78 }
        };

        [Test]
        [TestCaseSource("NewPrikaz1")]
        public void PrikazOdredjenogLos(int id)
        {
            WCFService testInstance = new WCFService();
            Assert.Throws<FaultException<MyException>>(() => testInstance.PrikaziOdredjeniIspad(id));
        }
        static object[] NewPrikaz1 =
        {
            new object[] { 77 }
        };

        [Test]
        [TestCaseSource("NewKreiraj")]
        public void KreirajDokumentDobar(int id, Element element, List<Akcija> listaAkcija)
        {
            WCFService testInstance = new WCFService();
            Assert.DoesNotThrow(() => testInstance.KreirajDokument(id, element.Naziv, listaAkcija));
        }
        static object[] NewKreiraj =
        {
            new object[] { 78, new Element("TU6", "Test1", 45, 45), new List<Akcija>() { new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12)) } }
        };
    }
}
