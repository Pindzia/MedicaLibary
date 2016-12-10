using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalLibrary.TestFolder
{
    class DataTableTemplate
    {
        private static DataTable _Table;
        public static DataTable Table
        {
            get
            {
                _Table = new DataTable("NazwaKolumny");
                _Table.Columns.Add("ComlumnName1");
                _Table.Rows.Add("Nygga");
                return _Table;
            }
        }

    }
}
