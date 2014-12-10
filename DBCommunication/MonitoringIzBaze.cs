using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacuniApp.DBCommunication
{
    class MonitoringIzBaze
    {
        public int idMonitoring;
        public string naziv;
        public List<string> mailoviZaSlanje;

        public MonitoringIzBaze()
        {
            idMonitoring = -666;
            this.naziv = "";
            this.mailoviZaSlanje = new List<string>();
        }
    }
}
