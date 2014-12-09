using RacuniApp.CustomClasses;
using RacuniApp.DBCommunication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacuniApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (DBBasicFunctions.checkDatabaseStatus())
            {
                /* SadistaImplementation.doYourThing();
                 UcitavanjeGenerisanje.uploadSource(Params.PUTANJA_ZA_SKINUTO + "\\" + Params.FILE_PREFIX + DateTime.Now.ToString(Params.FILE_NAME_FORMAT) + Params.FILE_EXTENSION);
                 UcitavanjeGenerisanje.generisiRezultat();*/

                // citanje svih monitoringa racuna

                // hvatanje rezultata za svaki

                // generisanje fajla za svaki

                // slanje maila za svaki 




                 ArrayList rib = UcitavanjeGenerisanje.getRezultat();
                 ExcelCommunication.upisiUFajl(rib);
            }
            
            Console.ReadKey();
        }
    }
}
