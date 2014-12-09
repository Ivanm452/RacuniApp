using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RacuniApp
{
    class Params
    {
        public static string PUTANJA_ZA_SKINUTO = @"C:\Users\IvanHP\Desktop\Racuni\Skinuto";
        public static string PUTANJA_ZA_REZULTAT = @"C:\Users\IvanHP\Desktop\Racuni\Rezultat";
        public static string FILE_PREFIX = "racuni_";
        public static string TEMPLATE_PATH = @"C:\Users\IvanHP\Desktop\Racuni\racuni_template.xlsx";

        public static string DB_CONNECTION_STRING = "Persist Security Info=False" +
            ";User ID=" + "sa" +
            ";Password=" + "osamosam" +
            ";Initial Catalog=" + "Monitoring" +
            ";Server=" + "Lapitopi";

        public static string SERVER_IP = "188.241.117.241";
        public static int SERVER_PORT = 8888;
        public static string SERVER_SSH_HOST_KEY_FINGERPRINT = "ssh-rsa 2048 ff:12:59:5f:fa:2d:d3:d4:88:04:1f:14:c4:57:78:2a";
        public static string SERVER_USERNAME = "root";
        public static string SERVER_PASSWORD = "oki.delamol";
        public static string SERVER_FILE_PATH = "/root/parser/racuni/racuni/output";
        public static string FILE_NAME_FORMAT = "yyyyMMdd";
        public static string FILE_EXTENSION = ".csv";

        public static int TIME_TO_WAIT_FOR_SERVER_STATUS = 1; // minutes

        public static string MAIL_ZA_SLANJE = "no-reply@cube.rs";
        public static string MAIL_PASSWORD = "cubenoreply123.";
    }
}
