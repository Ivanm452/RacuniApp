using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RacuniApp.CustomClasses;
using System.IO;
using CsvHelper;
using RacuniApp.DBCommunication;
using System.Collections;

namespace RacuniApp
{
    class UcitavanjeGenerisanje
    {

        public static void uploadSource(String filePath)
        {
            ArrayList racunList;
            string concenatedMBs;
            Dictionary<String, String> recnik = new Dictionary<string, string>();

            racunList = readSource(filePath);
            concenatedMBs = "";

            // generise recnik
            // u njega dodaje samo distinktne vrednosti
            foreach (RacunIzCSV rcsv in racunList)
                if (!recnik.ContainsKey(rcsv.maticniBroj.Trim()))
                    recnik.Add(rcsv.maticniBroj.Trim(), "");

            // generise MBove za query
            for (int i = 0; i < recnik.Count; i++)
                concenatedMBs += "'" + recnik.ElementAt(i).Key + "',";
            concenatedMBs = concenatedMBs.Substring(0, concenatedMBs.Length - 1); // da bi se sklonio poslednji zarez

            // dodeljuje svakom MBu u recniku odgovarajuci ID 
            Console.WriteLine(DateTime.Now + ": Pocinje dodeljivanje...");

            DBBasicFunctions.connection.Open();            
            DBBasicFunctions.assignIDs(ref recnik, concenatedMBs);
            foreach (RacunIzCSV rcsv in racunList) // ovo bi moglo da se optimizuje
                rcsv.idNadgledanaFirma = recnik[rcsv.maticniBroj];

            Console.WriteLine(DateTime.Now + ": Zavrsetak dodeljivanja.");


            // pomera stare zapise i upisuje u bazu
            Console.WriteLine(DateTime.Now + ": Pocinje upisivanje u bazu...");

            DBBasicFunctions.shiftOld();
            foreach (RacunIzCSV rcsv in racunList)
                if (rcsv.idNadgledanaFirma != null)
                    DBBasicFunctions.insertIntoRacuni(rcsv);
            DBBasicFunctions.connection.Close();

            Console.WriteLine(DateTime.Now + ": Zavrseno upisivanje u bazu...");
        }


        public static void generisiRezultat()
        {
            Console.WriteLine(DateTime.Now + ": Pocinje generisanje rezultata...");
            DBBasicFunctions.openConnection();

            DBBasicFunctions.generisiRezultat();

            DBBasicFunctions.closeConnection();
            Console.WriteLine(DateTime.Now + ": Zavrseno generisanje rezultata.");

        }

        public static ArrayList getRezultat()
        {
            ArrayList rib;

            Console.WriteLine(DateTime.Now + ": Pocinje hvatanje rezultata...");
            DBBasicFunctions.openConnection();

            rib = DBBasicFunctions.getRezultat();

            DBBasicFunctions.closeConnection();
            Console.WriteLine(DateTime.Now + ": Zavrseno hvatanje rezultata.");

            return rib;
        }


        public static ArrayList readSource(String filePath)
        {
            Console.WriteLine(DateTime.Now + ": Pocinje upload novog source-a...");

            ArrayList racunList;    // ovde se cuva niz racuna koji se procita iz CSV-a
            RacunIzCSV rcsv;        // ovo je jedan procitani racun iz CSV-a
            StreamReader sr;
            CsvReader csvread;
            int lineCount;          // ukupan broj linija u CSV-u
            int i;

            // inicijalizacija
            racunList = new ArrayList();
            rcsv = null;
            sr = new StreamReader(filePath);
            csvread = new CsvReader(sr);
            lineCount = File.ReadLines(filePath).Count();
            i = 1;

            csvread.Configuration.HasHeaderRecord = false; // postavlja se ako CSV fajl nema header

            while (csvread.Read())
            {
                rcsv = new RacunIzCSV(csvread);
                while (rcsv.maticniBroj == null) //ako maticni broj ne postoji cita se sledeci red
                    rcsv = new RacunIzCSV(csvread);

                racunList.Add(rcsv);

                if (i % 1000 == 0)
                    Console.WriteLine("Procitao " + i + "/" + lineCount);

                i++;
            }

            Console.WriteLine(DateTime.Now + ": Zavrsen upload source-a.");
            return racunList;
        }

    }
}
