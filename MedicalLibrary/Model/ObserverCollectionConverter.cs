using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Xml;
using System.Security.Cryptography;
using System.IO;
using System.Collections.ObjectModel; //ObservableCollection

namespace MedicaLibrary.Model
{
    public sealed class ObserverCollectionConverter
    {


        public static ObserverCollectionConverter Instance
        {
            get
            {
                if (instance==null)
                {
                    instance = new ObserverCollectionConverter();
                }
                return instance;
            }
        }

        private static ObserverCollectionConverter instance;

        public ObservableCollection<XElement> Observe(IEnumerable<XElement> data)
        {
            ObservableCollection<XElement> Coll = new ObservableCollection<XElement>();
            foreach (var dat in data)
            {
                Coll.Add(dat);
            }

            return Coll;
        }
    }
}
