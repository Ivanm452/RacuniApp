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
                ArrayList rib;

                SadistaImplementation.doYourThing();

                UcitavanjeGenerisanje.uploadSource(Params.PUTANJA_ZA_SKINUTO + "\\" + Params.FILE_PREFIX + DateTime.Now.ToString(Params.FILE_NAME_FORMAT) + Params.FILE_EXTENSION);
                UcitavanjeGenerisanje.generisiRezultat();

                List<MonitoringIzBaze> monitoringIzBaze = UcitavanjeGenerisanje.getMonitoringIzBaze();

                foreach (MonitoringIzBaze mib in monitoringIzBaze)
                {
                    rib = UcitavanjeGenerisanje.getRezultat(mib.idMonitoring);
                    UcitavanjeGenerisanje.email_send(mib.naziv, 
                        ExcelCommunication.upisiUFajl(rib, mib.naziv,mib.naziv), 
                        mib.mailoviZaSlanje); 
                }
            }     
       
            Console.ReadKey();
        }
    }
}
