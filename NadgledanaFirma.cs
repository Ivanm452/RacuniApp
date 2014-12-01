using CsvHelper;
using RacuniApp.CustomClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacuniApp
{
    class NadgledanaFirma
    {
        

        public static ArrayList uploadSource(String filePath)
        {
            ArrayList NadgledanaFirmaList = new ArrayList();

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

           // DBBasicFunctions.openConnection();
            //DBBasicFunctions.shiftOld();

            csvread.Configuration.HasHeaderRecord = false;

            while (csvread.Read())
            {
                rcsv = new RacunIzCSV(csvread);
                while (rcsv.maticniBroj == null) //ako maticni broj ne postoji
                    rcsv = new RacunIzCSV(csvread);

                //DBBasicFunctions.writeRacuniIzCSV(rcsv);

                NadgledanaFirmaList.Add(rcsv);

                if (i % 1000 == 0)
                    Console.WriteLine("Upisao " + i + "/" + lineCount);

                i++;
            }

            //DBBasicFunctions.closeConnection();
            Console.WriteLine(DateTime.Now + ": Zavrsen upload source-a.");
            return NadgledanaFirmaList;
        }

    }
}
