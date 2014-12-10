
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using RacuniApp.CustomClasses;
using System.Globalization;
using System.Collections;
using RacuniApp.CSV;

namespace RacuniApp.DBCommunication
{
    public class DBBasicFunctions
    {
        public static SqlConnection connection;

        public static void shiftOld()
        {
            Console.WriteLine(DateTime.Now + ": Pomeranje starih");

            string query;
            SqlCommand command;
            query = "DELETE Racun WHERE DanRacuna = @danRacuna";
            command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@danRacuna", "N_1");
            command.ExecuteNonQuery();

            query = "UPDATE Racun SET DanRacuna = @danRacunaNovi WHERE DanRacuna = @DanRacunaStari";
            command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@danRacunaNovi", "N_1");
            command.Parameters.AddWithValue("@danRacunaStari", "N");
            command.Dispose();
            command.ExecuteNonQuery();
        }

        public static void writeRacuniIzCSV(RacunIzCSV rcsv)
        {
            string idNadgledanaFirma = null;

            // provera u nadgledana firma
            idNadgledanaFirma = proveraMBuNadgledanaFirma(rcsv.maticniBroj);

            // insert u nadgledana firma
            if (idNadgledanaFirma == null)
                idNadgledanaFirma = insertIntoNadgledanaFirma(rcsv.maticniBroj);

            // provera i insert u osnovne informacije
            if (!proveraIDNadgledanaUOsnovneInformacije(idNadgledanaFirma))
                insertIntoOsnovneInformacije(rcsv, idNadgledanaFirma);

            insertIntoRacuni(rcsv, idNadgledanaFirma, "N");
        }

        public static string proveraMBuNadgledanaFirma(String maticniBrojZaProveru)
        {
            string idNadgledanaFirma = null;
            string query;
            SqlCommand command;
            SqlDataReader reader;

            query = "SELECT IDNadgledanaFirma FROM [NadgledanaFirma] WHERE MaticniBroj = @maticniBroj";
            command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@maticniBroj", maticniBrojZaProveru);
            reader = command.ExecuteReader();
            if (reader.Read())
                idNadgledanaFirma = reader[0].ToString().Trim();
            reader.Close();
            command.Dispose();
            return idNadgledanaFirma;
        }

        // insertuje maticni broj u tabelu NadgledanaFirma
        public static string insertIntoNadgledanaFirma(string maticnibroj)
        {
            string idNadgledanaFirma = null;
            string query;
            SqlCommand command;

            query = "INSERT INTO NadgledanaFirma (MaticniBroj) OUTPUT INSERTED.IDNadgledanaFirma VALUES(@maticniBroj)";
            command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@maticniBroj", maticnibroj);
            idNadgledanaFirma = command.ExecuteScalar().ToString().Trim();
            command.Dispose();
            return idNadgledanaFirma;
        }

        public static bool proveraIDNadgledanaUOsnovneInformacije(string idNadgledanaFirma)
        {
            string query;
            SqlCommand command;
            SqlDataReader reader;

            query = "SELECT IDNadgledanaFirma FROM [OsnovneInformacije] WHERE IDNadgledanaFirma = @idNadgledanaFirma";
            command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@idNadgledanaFirma", idNadgledanaFirma);
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                command.Dispose();
                return true;
            }
            reader.Close();
            command.Dispose();
            return false;
        }

        public static void insertIntoOsnovneInformacije(RacunIzCSV rcsv, string idNadgledanaFirma)
        {
            string query;
            SqlCommand command;

            query = "INSERT INTO [OsnovneInformacije](IDNadgledanaFirma, PIB)" +
                "VALUES(@idNadgledanaFirma,@pib)";
            command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@idNadgledanaFirma", idNadgledanaFirma.Trim());
            command.Parameters.AddWithValue("@pib", (rcsv.PIB == null) ? DBNull.Value : (object)rcsv.PIB);
            command.ExecuteNonQuery();

            command.Dispose();
        }

        public static void insertIntoRacuni(RacunIzCSV rcsv, String idNadgledanaFirma, String danRacuna)
        {
            DateTime datumOtvaranjaDT, datumPromeneDT, datumAzuriranjaDT;
            datumOtvaranjaDT = datumPromeneDT = datumAzuriranjaDT = DateTime.Now;

            string query;
            SqlCommand command;

            try
            {
                if (rcsv.datumOtvaranja != null && rcsv.datumOtvaranja.Length == 10)
                {
                    datumOtvaranjaDT = DateTime.ParseExact(rcsv.datumOtvaranja, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }

                if (rcsv.datumPromene != null && rcsv.datumPromene.Length == 10)
                {
                    datumPromeneDT = DateTime.ParseExact(rcsv.datumPromene, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                if (rcsv.datumAzuriranja != null && rcsv.datumAzuriranja.Length == 10)
                {
                    datumAzuriranjaDT = DateTime.ParseExact(rcsv.datumAzuriranja, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
            }
            catch (Exception e) { Console.WriteLine(rcsv.maticniBroj + " NEUSPENO UBACIVANJE " + e.ToString()); return; }


            query = "INSERT INTO [Racun](IDNadgledanaFirma,NazivBanke,BrojRacuna, DatumOtvaranja,DanRacuna, StatusRacunaSC) " +
                "VALUES(@idNadgledanaFirma,@nazivBanke,@brojRacuna,@datumOtvaranja,@danRacuna,@statusRacunaSC)";

            command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@idNadgledanaFirma", idNadgledanaFirma);
            command.Parameters.AddWithValue("@nazivBanke", (rcsv.banka == null) ? DBNull.Value : (object)rcsv.banka.Replace("\"", "'").Trim());// ovo se radi zbog AIK banke
            command.Parameters.AddWithValue("@brojRacuna", (rcsv.brojRacuna == null) ? DBNull.Value : (object)rcsv.brojRacuna.Trim());
            command.Parameters.AddWithValue("@datumOtvaranja", (rcsv.datumOtvaranja == null) ? DBNull.Value : (object)datumOtvaranjaDT);
            command.Parameters.AddWithValue("@danRacuna", danRacuna.Trim());
            command.Parameters.AddWithValue("@statusRacunaSC", (rcsv.statusRacunaSC == null) ? DBNull.Value : (object)rcsv.statusRacunaSC.Trim());
            command.ExecuteNonQuery();

            command.Dispose();
        }

        public static void generisiRezultat()
        {
            string query;
            SqlCommand command;

            // brisanje prethodnih racuna
            query = "TRUNCATE TABLE RezultatRacuna";
            command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();

            // otvoreni racuni
            query = "INSERT INTO RezultatRacuna (IDNadgledanaFirma,NazivBanke,BrojRacuna,StatusRacuna)  select R_N.IDNadgledanaFirma, R_N.NazivBanke, R_N.BrojRacuna, 'Otvoren račun' " +
                "FROM RACUN R_N WHERE R_N.DanRacuna = 'N' AND R_N.StatusRacunaSC like '%Uklju_en u platni promet%' " +
                "AND R_N.DatumOtvaranja >= (SELECT MAX(R_N_1.DatumUnosa) FROM Racun R_N_1 WHERE R_N_1.DanRacuna = 'N_1')";
            command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();

            // zatvoreni racuni
            query = "INSERT INTO RezultatRacuna (IDNadgledanaFirma,NazivBanke,BrojRacuna,StatusRacuna) SELECT R_N_1.IDNadgledanaFirma, R_N_1.NazivBanke, R_N_1.BrojRacuna, 'Zatvoren račun'  FROM Racun R_N_1 " +
            "LEFT JOIN Racun R_N ON R_N_1.BrojRacuna = R_N.BrojRacuna and R_N.DanRacuna = 'N' JOIN OsnovneInformacije oi on oi.IDNadgledanaFirma = R_N_1.IDNadgledanaFirma  WHERE R_N_1.DanRacuna = 'N_1' " +
            " and oi.Status_N not like '%risan%'" +
            " AND (R_N_1.StatusRacunaSC like '%Uklju_en u platni promet%' OR R_N_1.StatusRacunaSC like '%Blokirana zadu%enja%' OR R_N_1.StatusRacunaSC like '%Blokiran po osnovu prinudne naplate%')  AND R_N.IDRacun IS NULL";
            command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();

            command.Dispose();

        }

        public static ArrayList getRezultat(int IDMonitoring)
        {
            string query;
            SqlCommand command;
            SqlDataReader reader;
            ArrayList rib = new ArrayList();

            query = "  SELECT nf.MaticniBroj, oi.CarlCustomID, oi.naziv, oi.pib, rr.NazivBanke,  rr.BrojRacuna, rr.StatusRacuna, rr.StatusRacunaSC  FROM RezultatRacuna rr " +
            "INNER JOIN NadgledanaFirma nf ON nf.IDNadgledanaFirma = rr.IDNadgledanaFirma  INNER JOIN OsnovneInformacije oi ON oi.IDNadgledanaFirma = rr.IDNadgledanaFirma INNER JOIN MonitoringFirma mf on mf.IDNadgledanaFirma = nf.IDNadgledanaFirma " +
            "WHERE mf.IDMonitoring = @IDMonitoring";
            command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@IDMonitoring", IDMonitoring);

            reader = command.ExecuteReader();
            while (reader.Read())
                rib.Add(new RacunIzBaze(reader[0].ToString().Trim(), reader[1].ToString().Trim(), reader[2].ToString().Trim(), reader[3].ToString().Trim(), reader[4].ToString().Trim(), reader[5].ToString().Trim(), reader[6].ToString().Trim(), reader[7].ToString().Trim()));
            reader.Close();
            command.Dispose();
            return rib;
        }


        public static Boolean checkDatabaseStatus()
        {

            Console.WriteLine(DateTime.Now + ": Checking database status...");

            for (int i = 0; i < 3; i++)
            {
                if (openConnection())
                    break;
                Console.WriteLine(DateTime.Now + ": Database error. Checking again in 2 minutes");
                Thread.Sleep(2 * 60 * 1000);
            }
            closeConnection();
            return true;
        }

        public static Boolean openConnection()
        {
            Console.WriteLine(DateTime.Now + ": Pokusavam da ostvarim komunikaciju sa bazom...");

            connection = new SqlConnection(Params.DB_CONNECTION_STRING);
            try
            {
                connection.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(DateTime.Now + ": Neuspesno povezivanje za bazom");
                Console.WriteLine(ex.ToString());
                return false;
            }

            Console.WriteLine(DateTime.Now + ": Uspesno povezivanje za bazom");
            return true;
        }

        public static Boolean closeConnection()
        {
            Console.WriteLine(DateTime.Now + ": Pokusavam da zatvorim komunikaciju sa bazom...");

            if (connection != null)
            {
                try
                {
                    connection.Close();
                    Console.WriteLine(DateTime.Now + ": Uspesno zatvorena komunikacija");
                    return true;

                }
                catch (SqlException ex)
                {
                    Console.WriteLine(DateTime.Now + ": Neuspesno zatvaranje komunikacije.");
                    Console.WriteLine(ex.ToString());
                    return false;
                }

            }
            else
            {
                Console.WriteLine(DateTime.Now + ": Greska prilikom zatvaranja komunikacije. Varijabla connection je NULL.");
                return false;
            }
        }

        public static List<int> getMonitoringIDPoVrsti(int idVrstaMonitoringa)
        {
            string query;
            SqlCommand command;
            SqlDataReader reader;
            List<int> monitoringID = new List<int>();

            query = "SELECT IDMonitoring FROM Monitoring WHERE IDVrstaMonitoringa = @idVrstaMonitoringa";

            command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@idVrstaMonitoringa", idVrstaMonitoringa);
            reader = command.ExecuteReader();
            while (reader.Read())
                monitoringID.Add(Int32.Parse(reader[0].ToString().Trim()));
            reader.Close();
            command.Dispose();
            return monitoringID; 
        }

        public static string getNazivPoIDMonitoring(int IDMonitoring)
        {
            string query;
            SqlCommand command;
            SqlDataReader reader;
            string naziv = "";

            query = "SELECT Naziv FROM Monitoring WHERE IDMonitoring = @IDMonitoring";

            command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@IDMonitoring", IDMonitoring);
            reader = command.ExecuteReader();
            if (reader.Read())
                naziv = reader[0].ToString().Trim();
            reader.Close();
            command.Dispose();
            return naziv; 
        }

        public static List<string> getMailPoIDMonitoring(int IDMonitoring)
        {
            string query;
            SqlCommand command;
            SqlDataReader reader;
            List<string> mail = new List<string>();

            query = "SELECT Mail FROM Mail where IDMonitoring = @IDMonitoring";

            command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@IDMonitoring", IDMonitoring);
            reader = command.ExecuteReader();
            while (reader.Read())
                mail.Add(reader[0].ToString().Trim());
            reader.Close();
            command.Dispose();
            return mail;
        }

       
    }
}
