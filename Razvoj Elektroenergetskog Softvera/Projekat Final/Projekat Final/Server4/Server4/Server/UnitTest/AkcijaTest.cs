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
    public class AkcijaTest
    {
        [Test]
        [TestCaseSource("NewAction")]
        public void AkcijaKonstruktorDobar(string opis, DateTime vreme)
        {
            Akcija akcija = new Akcija(opis, vreme);

            Assert.AreEqual(akcija.OpisAkcije, opis);
            Assert.AreEqual(akcija.Vreme, vreme);
        }

        static object[] NewAction =
        {
            new object[] { "LOGOVANJE", new DateTime(2018, 12, 12, 12, 12, 12) },
            new object[] { "LOGOVANJE", new DateTime(2018, 12, 12, 12, 12, 12) },
            new object[] { "LOGOVANJE", new DateTime(2018, 12, 12, 12, 12, 12) }
        };

        [Test]
        [TestCaseSource("NewAction1")]
        public void AkcijaKonstruktorLos(string opis, DateTime vreme)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Akcija akcija = new Akcija(opis, vreme);
            });
        }

        static object[] NewAction1 =
        {
            new object[] { "", new DateTime(2018, 12, 12, 12, 12, 12) },
        };
        
        [Test]
        [TestCaseSource("NewAction2")]       
        public void AkcijaKonstruktorLos1(string opis, DateTime vreme)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Akcija akcija = new Akcija(opis, vreme);
            });
        }


        static object[] NewAction2 =
        {
            new object[] { null, new DateTime(2018, 12, 12, 12, 12, 12) }
        };
    }
}
