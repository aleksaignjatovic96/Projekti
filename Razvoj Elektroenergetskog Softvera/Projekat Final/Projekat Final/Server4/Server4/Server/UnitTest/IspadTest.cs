using Common;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestFixture]
    public class IspadTest
    {

        [Test]
        [TestCaseSource("NewIspad")]
        public void IspadKonstruktorDobar(int _id, DateTime _vreme, string _opis, Element _element, List<Akcija> _listaAkcija)
        {
            Ispad ispad = new Ispad(_id, _vreme, _opis, _element, _listaAkcija);

            Assert.AreEqual(ispad.Id, _id);
            Assert.AreEqual(ispad.Vreme, _vreme);
            Assert.AreEqual(ispad.Opis, _opis);
            Assert.AreEqual(ispad.Element, _element);
            Assert.AreEqual(ispad.ListaAkcija, _listaAkcija);
        }

        static object[] NewIspad =
        {
            new object[] { 1, new DateTime(2018, 12, 12, 12, 12, 12), "Kvar Tranformatora", new Element("T1", "Test1", 45, 45), new List<Akcija>(){new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12)) } },
            new object[] { 2, new DateTime(2018, 12, 12, 12, 12, 12), "Kratak Spoj", new Element("T2", "Test2", 45, 45), new List<Akcija>(){new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12)) }  },
            new object[] { 3, new DateTime(2018, 12, 12, 12, 12, 12), "Nepravilno merenje" , new Element("T1", "Test1", 45, 45), new List<Akcija>() { new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12)) } }
        };

        [Test]
        [TestCaseSource("NewIspad1")]
        public void IspadKonstruktorNull(int _id, DateTime _vreme, string _opis, Element _element, List<Akcija> _listaAkcija)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Ispad ispad = new Ispad(_id, _vreme, _opis, _element, _listaAkcija);
            });
        }

        static object[] NewIspad1 =
        {
            new object[] { 1, new DateTime(2018, 12, 12, 12, 12, 12), null, new Element("T1", "Test1", 45, 45), new List<Akcija>(){new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12)) } },
            new object[] { 2, new DateTime(2018, 12, 12, 12, 12, 12), "Kratak Spoj", null, new List<Akcija>(){new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12)) }  },
            new object[] { 3, new DateTime(2018, 12, 12, 12, 12, 12), "Nepravilno merenje" , new Element("T1", "Test1", 45, 45), null }
        };

        [Test]
        [TestCaseSource("NewIspad2")]
        public void IspadKonstruktorLos(int _id, DateTime _vreme, string _opis, Element _element, List<Akcija> _listaAkcija)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Ispad ispad = new Ispad(_id, _vreme, _opis, _element, _listaAkcija);
            });
        }

        static object[] NewIspad2 =
        {
            new object[] { 1, new DateTime(2018, 12, 12, 12, 12, 12), "", new Element("T1", "Test1", 45, 45), new List<Akcija>{new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12)) } },
            new object[] { 3, new DateTime(2018, 12, 12, 12, 12, 12), "Nepravilno merenje" , new Element("T1", "Test1", 45, 45), new List<Akcija>{new Akcija() } }
        };
    }
}
