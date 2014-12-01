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
                SadistaImplementation.doYourThing();
                UcitavanjeGenerisanje.uploadSource(Params.PUTANJA_ZA_SKINUTO + "\\" + Params.FILE_PREFIX + DateTime.Now.ToString(Params.FILE_NAME_FORMAT) + Params.FILE_EXTENSION);
                UcitavanjeGenerisanje.generisiRezultat();
                ArrayList rib = UcitavanjeGenerisanje.getRezultat();
                ExcelCommunication.upisiUFajl(rib);

               /* ArrayList nfl = NadgledanaFirma.uploadSource(Params.PUTANJA_ZA_SKINUTO + "\\" + Params.FILE_PREFIX + DateTime.Now.ToString(Params.FILE_NAME_FORMAT) + Params.FILE_EXTENSION);
                string s = "";
                foreach (RacunIzCSV nf in nfl)
                    s += "'" + nf.maticniBroj + "',";
                s = s.Substring(0, s.Length - 1);
                
                DBCommunication.DBBasicFunctions.connection.Open();
                string query;
                SqlCommand command;
                SqlDataReader reader;
                Console.WriteLine("Pocinje sa bazom");
                query = "SELECT IDNadgledanaFirma, MaticniBroj FROM [NadgledanaFirma] WHERE MaticniBroj in (@maticniBroj)";
                command = new SqlCommand(query, DBBasicFunctions.connection);
                command.Parameters.AddWithValue("@maticniBroj", s);
                reader = command.ExecuteReader();
                if (reader.Read())
                    idNadgledanaFirma = reader[0].ToString().Trim();
                reader.Close();
                command.Dispose();

                DBCommunication.DBBasicFunctions.connection.Close();
                Console.WriteLine("Zavrsio sa bazom");*/
               

            }

            Console.ReadKey();
        }
    }
}
