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
            Console.WriteLine(DateTime.Now + ": Pocinje upload novog source-a...");

            RacunIzCSV rcsv;
            StreamReader sr;
            CsvReader csvread;
            int lineCount;
            int i;

            // inicijalizacija
            rcsv = null;
            sr = new StreamReader(filePath);
            csvread = new CsvReader(sr);
            lineCount = File.ReadLines(filePath).Count();
            i = 1;

            DBBasicFunctions.openConnection();
            DBBasicFunctions.shiftOld();

            csvread.Configuration.HasHeaderRecord = false;

            while (csvread.Read())
            {
                rcsv = new RacunIzCSV(csvread);
                while (rcsv.maticniBroj == null) //ako maticni broj ne postoji
                    rcsv = new RacunIzCSV(csvread);

                DBBasicFunctions.writeRacuniIzCSV(rcsv);

                if(i%1000==0)
                    Console.WriteLine("Upisao " + i + "/" + lineCount);

                i++;
            }

            DBBasicFunctions.closeConnection();
            Console.WriteLine(DateTime.Now + ": Zavrsen upload source-a.");
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

    }
}
