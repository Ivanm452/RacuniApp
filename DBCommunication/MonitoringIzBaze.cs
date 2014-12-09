using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacuniApp.DBCommunication
{
    class MonitoringIzBaze
    {
        public string naziv;
        public List<string> mailoviZaSlanje;
        public List<string> idNadgledaneFirme;

        public MonitoringIzBaze()
        {
            this.naziv = "";
            this.mailoviZaSlanje = new List<string>();
            this.idNadgledaneFirme = new List<string>();
        }

        public MonitoringIzBaze(string naziv, List<string> mailoviZaSlanje, List<string> idNadgledaneFirme)
        {
            this.naziv = naziv;
            this.mailoviZaSlanje = mailoviZaSlanje;
            this.idNadgledaneFirme = idNadgledaneFirme;
        }
    }
}
