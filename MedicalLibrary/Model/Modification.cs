﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MedicaLibrary.Model
{
    public class Modification
    {

        XElement database = XElementon.Instance.getDatabase();

        //Wyświetl wszystkie modyfikacje
        public IEnumerable<XElement> Modifications()
        {
            var modification = from qmeta in database.Elements("meta")
                               from qmodifications in qmeta.Elements("modifications")
                               from qmodification in qmodifications.Elements("modification")
                               select qmodification;
            return modification;
        }

        //Wyświetl wszystkie modyfikacje o podanym id
        public IEnumerable<XElement> WithIDM(int idm)
        {
            var modification = from qmeta in database.Elements("meta")
                               from qmodifications in qmeta.Elements("modifications")
                               from qmodification in qmodifications.Elements("modification")
                               where (string)qmodification.Element("idm") == idm.ToString()
                               select qmodification;
            return modification;
        }

        //Ostatnio zmienieni pacjencji
        public IEnumerable<XElement> ChangedPatients()
        {
            var cpatients = from qmeta in database.Elements("meta")
                            from qmodifications in qmeta.Elements("modifications")
                            where (string)qmodifications.Element("node_type") == "patient"
                            select qmodifications;
            return cpatients;
        }

        //Ostatnio zmienione wizyty
        public IEnumerable<XElement> ChangedVisits()
        {
            var cvisits = from qmeta in database.Elements("meta")
                          from qmodifications in qmeta.Elements("modifications")
                          where (string)qmodifications.Element("node_type") == "visits"
                          select qmodifications;
            return cvisits;
        }

        //Ostatnio zmienione magazyny
        public IEnumerable<XElement> ChangedStorehouses()
        {
            var cstorehouses = from qmeta in database.Elements("meta")
                               from qmodifications in qmeta.Elements("modifications")
                               where (string)qmodifications.Element("node_type") == "storehouse"
                               select qmodifications;
            return cstorehouses;
        }

        //Ostatnio zmienione zasady
        public IEnumerable<XElement> ChangedRules()
        {
            var crules = from qmeta in database.Elements("meta")
                         from qmodifications in qmeta.Elements("modifications")
                         where (string)qmodifications.Element("node_type") == "rule"
                         select qmodifications;
            return crules;
        }


        public void RevertX(int idm) //reverty nigdy nie logują (reverty to de-logowanie)
        {
            var revert = this.WithIDM(idm).FirstOrDefault();
            XElement reverted = null;

            if (revert == null)
            {
                return;
            }

            //TODO - zakładam że istnieją te elementy - jeśli będzie głupia modyfikacja to rzuci exceptionem!
            var operation = (string)revert.Elements("operation").FirstOrDefault();
            var nodetype = (string)revert.Elements("node_type").FirstOrDefault();

            var olddata = revert.Element("olddata").Elements();

            List<Tuple<string, string>> datalist = new List<Tuple<string, string>>();
            if (operation == "D")
            {

                foreach (var dat in olddata)
                {
                    datalist.Add(new Tuple<string, string>(dat.Name.LocalName, (string)dat));
                }

            }
            Tuple<string, string>[] datatable = datalist.ToArray();

            //TODO - czy podczas odwracania Usuwania (czyli podczas dodawania historycznego) przywracać mu jego stare ID czy kontynuować autonumerację?
            if (nodetype == "patient")
            {
                reverted = XElementon.Instance.Patient.WithIDP(Convert.ToInt16(revert.Element("id").Value)).FirstOrDefault();
                if (operation == "D")
                    XElementon.Instance.Patient.Add(datatable, false);
            }
            else if (nodetype == "visit")
            {
                reverted = XElementon.Instance.Visit.WithIDV(Convert.ToInt16(revert.Element("id").Value)).FirstOrDefault();
                if (operation == "D")
                    XElementon.Instance.Visit.Add(Convert.ToInt16(revert.Element("idv")), datatable, false);
            }
            else if (nodetype == "storehouse")
            {
                reverted = XElementon.Instance.Storehouse.WithIDS(Convert.ToInt16(revert.Element("id").Value)).FirstOrDefault();
                if (operation == "D")
                    XElementon.Instance.Storehouse.Add(datatable, false);
            }
            else if (nodetype == "rule")
            {
                reverted = XElementon.Instance.Rule.WithIDR(Convert.ToInt16(revert.Element("id").Value)).FirstOrDefault();
                if (operation == "D")
                    XElementon.Instance.Rule.Add(Convert.ToInt16(revert.Element("ids")), datatable, false);
            }
            else if (nodetype == "field")
            {
                reverted = XElementon.Instance.Field.WithIDF(Convert.ToInt16(revert.Element("id").Value)).FirstOrDefault();
                if (operation == "D")
                    XElementon.Instance.Field.Add(datatable, false);
            }

            if (operation == "E")
            {
                foreach (var dat in olddata)
                {
                    if (reverted.Elements(dat.Name).Any()) //TODO: element vs elements
                    {
                        reverted.Element(dat.Name).Value = (string)dat;
                    }
                    else
                    {
                        reverted.Element(dat.Name).Remove();
                    }
                }
            }
            else if (operation == "A")
            {
                XElementon.Instance.DeleteX(nodetype, Convert.ToInt16(revert.Element("id").Value), false);
            }

            revert.Remove();
        }

    }

}