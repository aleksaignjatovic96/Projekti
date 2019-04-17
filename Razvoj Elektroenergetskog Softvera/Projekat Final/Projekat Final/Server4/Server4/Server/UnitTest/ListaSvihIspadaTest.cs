using Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestFixture]
    public class ListaSvihIspadaTest
    {
        [Test]
        [TestCaseSource("Dodaj")]
        public void DodajDobar(Ispad i)
        {
            ListaSvihIspada lista = new ListaSvihIspada();
            Assert.DoesNotThrow(() => lista.DodajIspad(i));
        }

        static object[] Dodaj =
        {
            new object[] {new Ispad(78, new DateTime(2018, 12, 12, 12, 12, 12), "Kvar Tranformatora", new Element("TU4", "Test1", 45, 45), new List<Akcija>(){new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12)) })},
            new object[] { new Ispad(128, new DateTime(2018, 12, 12, 12, 12, 12), "Kratak Spoj", new Element("TU5", "Test2", 45, 45), new List<Akcija>(){new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12))} )},
            new object[] { new Ispad(77, new DateTime(2018, 12, 12, 12, 12, 12), "Kratak Spoj", new Element("TU6", "Test2", 45, 45), new List<Akcija>(){new Akcija("Pad sistema", new DateTime(2018, 12, 12, 12, 12, 12))})}
        };

        [Test]
        [TestCaseSource("Dodaj1")]
        public void DodajNull(Ispad i)
        {
            ListaSvihIspada lista = new ListaSvihIspada();
            Assert.Throws<ArgumentNullException>(() => lista.DodajIspad(i));
        }

        static object[] Dodaj1 =
        {
            new object[] { null }
        };
    }
}
