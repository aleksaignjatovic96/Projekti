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
    public class ElementaTest
    {
        [Test]
        [TestCase("T1", "Test1", 45, 45)]
        [TestCase("T2", "Test2", 1, 1)]
        [TestCase("T3", "Test3", 25, 75)]
        public void ElementKonstruktorDobar(string id, string naziv, float x, float y)
        {
            Element element = new Element(id, naziv, x, y);

            Assert.AreEqual(element.Id, id);
            Assert.AreEqual(element.Naziv, naziv);
            Assert.AreEqual(element.X, x);
            Assert.AreEqual(element.Y, y);
        }

        [Test]
        [TestCase("", "Test1", 45, 45)]
        [TestCase("T2", "", 1, 1)]
        public void ElementKonstruktorLos(string id, string naziv, float x, float y)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Element element = new Element(id, naziv, x, y);
            });
        }

        [Test]
        [TestCase(null, "Test1", 45, 45)]
        [TestCase("T2", null, 1, 1)]

        public void ElementKonstruktorLos1(string id, string naziv, float x, float y)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Element element = new Element(id, naziv, x, y);
            });
        }
    }
}
