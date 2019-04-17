using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class ListaSvihIspada
    {
        private List<Ispad> lista = new List<Ispad>();

        public ListaSvihIspada() { }

        public List<Ispad> UkupnaListaSvihIspada
        {
            get { return lista; }
            set { lista = value; }
        }

        public void DodajIspad(Ispad U)
        {
            if (U == null)
            {
                throw new ArgumentNullException("Argument ne sme biti null");
            }

            lista.Add(U);
        }
    }
}
