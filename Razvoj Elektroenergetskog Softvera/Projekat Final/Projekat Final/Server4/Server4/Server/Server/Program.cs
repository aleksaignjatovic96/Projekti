using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {

            string adresa = "net.tcp://localhost:6015/IIspad";
            ServiceHost host = new ServiceHost(typeof(WCFService));
            host.AddServiceEndpoint(typeof(IIspad),
                                    new NetTcpBinding(),
                                    new Uri(adresa));
            host.Open();
            Console.WriteLine("Host je otvoren na adresi : {0}", adresa);
            Console.ReadLine();
            host.Close();
        }
    }
}
