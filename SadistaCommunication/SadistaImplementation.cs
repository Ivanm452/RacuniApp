using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RacuniApp;


class SadistaImplementation
{
    public static bool doYourThing()
    {
        Console.WriteLine("START");
        using (SadistaFunctions sf = new SadistaFunctions(Params.SERVER_IP, Params.SERVER_PORT, Params.SERVER_USERNAME, Params.SERVER_PASSWORD, Params.SERVER_SSH_HOST_KEY_FINGERPRINT))
        {
            int i;

            // checks server status 3 times 
            // each time waiting i*TIME_TO_WAIT_FOR_SERVER_STATUS minutes
            // if server status at the end is false then it exits the function
            i = 1;
            bool serverStatus = sf.checkServerStatus();

            Console.WriteLine("CHECKING SERVER");
            while (!serverStatus && i <= 3)
            {
                Console.WriteLine("SERVER NOT AVAILABLE " + i);
                Thread.Sleep(Params.TIME_TO_WAIT_FOR_SERVER_STATUS * 60 * 1000 * i);
                i++;
                serverStatus = sf.checkServerStatus();
            }
            if (!serverStatus)
                throw new Exception("SADISTA IS NOT AVAILABLE");
            Console.WriteLine("SADISTA IS AVAILABLE");

            // check result file
            if (!sf.checkIfFileExsists(Params.SERVER_FILE_PATH + "/"
                    + DateTime.Now.ToString(Params.FILE_NAME_FORMAT) +
                    Params.FILE_EXTENSION))
            {
                throw new Exception("RESULT FILE DOES NOT EXSIST");
            }
            Console.WriteLine("RESULT FILE EXSISTS");


            // download input and result file
            sf.getFile(Params.SERVER_FILE_PATH + "/"
                    + DateTime.Now.ToString(Params.FILE_NAME_FORMAT) +
                    Params.FILE_EXTENSION, Params.PUTANJA_ZA_SKINUTO + "\\" + Params.FILE_PREFIX + DateTime.Now.ToString(Params.FILE_NAME_FORMAT) + Params.FILE_EXTENSION);
            Console.WriteLine("FILE DOWNLOADED");

        }
        return true;
    }


}
