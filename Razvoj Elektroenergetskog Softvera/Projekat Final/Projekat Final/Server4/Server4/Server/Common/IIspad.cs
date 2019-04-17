using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IIspad
    {
        [OperationContract]
        [FaultContract(typeof(MyException))]
        void UnosPodatakaOIspadu(int id, DateTime vreme, Naponski_Nivo naponski_Nivo, string opis, Status status, Element element, List<Akcija> listaAkcija, int radnja);

        [OperationContract]
        [FaultContract(typeof(MyException))]
        List<Ispad> PrikaziSveIspade();

        [OperationContract]
        [FaultContract(typeof(MyException))]
        Ispad PrikaziOdredjeniIspad(int id);

        [OperationContract]
        [FaultContract(typeof(MyException))]
        void KreirajDokument(int id, string nazivElementa, List<Akcija> spisakAkcija);
    }
}
