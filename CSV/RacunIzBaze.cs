using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RacuniApp.CSV
{
    class RacunIzBaze
    {
        public string maticniBroj;
        public string carlCustomID;
        public string naziv;
        public string PIB;
        public string banka;
        public string brojRacuna;
        public string statusRacuna;
        public string statusRacunaSC;

        public RacunIzBaze(string maticniBroj, string carlCustomID, string naziv, string PIB, string banka, string brojRacuna, string statusRacuna, string statusRacunaSC)
        {
            this.maticniBroj = maticniBroj;
            this.carlCustomID = carlCustomID;
            this.naziv = naziv;
            this.PIB = PIB;
            this.banka = banka;
            this.brojRacuna = brojRacuna;
            this.statusRacuna = statusRacuna;
            this.statusRacunaSC = statusRacunaSC;
        }

        public override string ToString()
        {
            return maticniBroj + " " + carlCustomID + " " + naziv + " " + PIB + " " + banka + " " + brojRacuna + " " + statusRacuna;

        }

    }
}
