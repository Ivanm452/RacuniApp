using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvHelper;

namespace RacuniApp.CustomClasses
{
    public class RacunIzCSV
    {
        public string maticniBroj;
        public string tipRacuna;
        public string PIB;
        public string banka;
        public string brojRacuna;
        public string statusRacunaSC;
        public string datumOtvaranja;
        public string datumPromene;
        public string datumAzuriranja;
        public string idNadgledanaFirma;

        public RacunIzCSV(string maticniBroj, string tipRacuna, string PIB, string banka, string brojRacuna, string statusRacunaSC, string datumOtvaranja, string datumPromene, string datumAzuriranja)
        {
            this.maticniBroj = maticniBroj;
            this.tipRacuna = tipRacuna;
            this.PIB = PIB;
            this.banka = banka;
            this.brojRacuna = brojRacuna;
            this.statusRacunaSC = statusRacunaSC;
            this.datumOtvaranja = datumOtvaranja;
            this.datumPromene = datumPromene;
            this.datumAzuriranja = datumAzuriranja;
        }

        public RacunIzCSV(CsvReader csv)
        {
            maticniBroj = tipRacuna = PIB = banka = brojRacuna = statusRacunaSC = datumOtvaranja = datumPromene = datumAzuriranja = "";

            try
            {
                if (csv[0] != null)
                    maticniBroj = csv[0].Trim();

                if (csv[1] != null)
                    tipRacuna = csv[1].Trim();

                if (csv[2] != null)
                    PIB = csv[2].Trim();

                if (csv[3] != null)
                    banka = csv[3].Trim();

                if (csv[4] != null)
                    brojRacuna = csv[4].Trim();

                if (csv[5] != null)
                    statusRacunaSC = csv[5].Trim();

                if (csv[6] != null)
                    datumOtvaranja = csv[6].Trim();

                if (csv[7] != null)
                    datumPromene = csv[7].Trim();

                if (csv[8] != null)
                    datumAzuriranja = csv[8].Trim();
            }
            catch (Exception e) { Console.WriteLine("Greska u RacuniIzCsv! " + e.ToString()); }

        }

        public override string ToString()
        {
            return maticniBroj + " " + tipRacuna + " " + PIB + " " + banka + " " + brojRacuna + " " + statusRacunaSC + " " + datumOtvaranja +
                " " + datumPromene + " " + datumAzuriranja;

        }


    }
}
