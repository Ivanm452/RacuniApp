using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SpreadsheetLight;
using System.Globalization;
using RacuniApp.CSV;

namespace RacuniApp
{
    class ExcelCommunication
    {
        public static string upisiUFajl(ArrayList rib, String fileName, String clientName)
        {
            SLDocument sl;
            sl = new SLDocument(Params.TEMPLATE_PATH);
            int i = 6;

            Console.WriteLine(DateTime.Now + ": Pocinje upisivanje u fajl");

            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, "Bankovni računi");
            sl.SetCellValue("G2", clientName);
            sl.SetCellValue("G3", DateTime.Now.ToString("dd. MMMM yyyy", CultureInfo.GetCultureInfo("sr-Latn-CS")));
            
            foreach (RacunIzBaze onerib in rib)
            {
                sl.SetCellValue(i, 1, onerib.maticniBroj);
                sl.SetCellValue(i, 2, onerib.carlCustomID);
                sl.SetCellValue(i, 3, onerib.PIB);
                sl.SetCellValue(i, 4, onerib.naziv);
                sl.SetCellValue(i, 5, onerib.banka);
                sl.SetCellValue(i, 6, onerib.brojRacuna);
                sl.SetCellValue(i, 7, onerib.statusRacuna);
                if (i % 1000 == 0)
                    Console.WriteLine(DateTime.Now + ": upisano " + i + "/" + rib.Count);
                i++;
            }

            string filePath = Params.PUTANJA_ZA_REZULTAT + "\\Qbing računi - " + fileName + " - " + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            sl.SaveAs(filePath);
            Console.WriteLine(DateTime.Now + ": Zavrseno upisivanje u fajl");
            return filePath;
        }
    }
}
