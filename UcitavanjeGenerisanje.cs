﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RacuniApp.CustomClasses;
using System.IO;
using CsvHelper;
using RacuniApp.DBCommunication;
using System.Collections;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace RacuniApp
{
    class UcitavanjeGenerisanje
    {
        public static void uploadSource(String filePath)
        {
            Console.WriteLine(DateTime.Now + ": Pocinje upload novog source-a...");

            RacunIzCSV rcsv;
            StreamReader sr;
            CsvReader csvread;
            int lineCount;
            int i;

            // inicijalizacija
            rcsv = null;
            sr = new StreamReader(filePath);
            csvread = new CsvReader(sr);
            lineCount = File.ReadLines(filePath).Count();
            i = 1;

            DBBasicFunctions.openConnection();
            DBBasicFunctions.shiftOld();

            csvread.Configuration.HasHeaderRecord = false;

            while (csvread.Read())
            {
                rcsv = new RacunIzCSV(csvread);
                while (rcsv.maticniBroj == null) //ako maticni broj ne postoji
                    rcsv = new RacunIzCSV(csvread);

                DBBasicFunctions.writeRacuniIzCSV(rcsv);

                if(i%1000==0)
                    Console.WriteLine("Upisao " + i + "/" + lineCount);

                i++;
            }

            DBBasicFunctions.closeConnection();
            Console.WriteLine(DateTime.Now + ": Zavrsen upload source-a.");
        }


        public static void generisiRezultat()
        {
            Console.WriteLine(DateTime.Now + ": Pocinje generisanje rezultata...");
            DBBasicFunctions.openConnection();

            DBBasicFunctions.generisiRezultat();

            DBBasicFunctions.closeConnection();
            Console.WriteLine(DateTime.Now + ": Zavrseno generisanje rezultata.");

        }

        public static ArrayList getRezultat(int IDMonitoring)
        {
            ArrayList rib;

            Console.WriteLine(DateTime.Now + ": Pocinje hvatanje rezultata...");
            DBBasicFunctions.openConnection();

            rib = DBBasicFunctions.getRezultat(IDMonitoring);

            DBBasicFunctions.closeConnection();
            Console.WriteLine(DateTime.Now + ": Zavrseno hvatanje rezultata.");

            return rib;
        }

        public static List<MonitoringIzBaze> getMonitoringIzBaze()
        {
            List<MonitoringIzBaze> monitoringIzBaze = new List<MonitoringIzBaze>();
            MonitoringIzBaze mib;

            DBBasicFunctions.openConnection();
            List<int> monitoringPoVrsti = DBBasicFunctions.getMonitoringIDPoVrsti(8);

            foreach (int monitoringID in monitoringPoVrsti)
            {
                mib = new MonitoringIzBaze();
                mib.idMonitoring = monitoringID;
                mib.naziv = DBBasicFunctions.getNazivPoIDMonitoring(monitoringID);
                mib.mailoviZaSlanje = DBBasicFunctions.getMailPoIDMonitoring(monitoringID);
                monitoringIzBaze.Add(mib);
                mib = null;
                
            }

            DBBasicFunctions.closeConnection();
            return monitoringIzBaze; 
        }

        public static bool email_send(string klijent, string attachmentPath, List<string> lstMail)
        {
            Console.WriteLine("Pocinje slanje maila za: " + klijent);
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Params.MAIL_SMTP_SERVER);
                mail.From = new MailAddress(Params.MAIL_ZA_SLANJE);

                foreach (string s in lstMail)
                    mail.To.Add(s.Trim());

                mail.Subject = "Racuni " + klijent + " " + DateTime.Now.ToString("dd.MM.yyy");

                // Set delivery notifications for success and failed messages
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure;
                mail.Headers.Add("Read-Receipt-To", Params.MAIL_READ_RECEPIENT);
                mail.Headers.Add("Disposition-Notification-To", Params.MAIL_READ_RECEPIENT);

                LinkedResource theEmailImage = new LinkedResource(Params.MAIL_LOGO_PATH);
                theEmailImage.ContentId = "myImageID";

                string body =
                @"<html>
                <body>
                <table width=""100%"">
                <tr>
                <td style=""font-family:Century Gothic"">
                Poštovani, <br><br>
                U prilogu Vam šaljemo izveštaj o računima<br><br>
                Molimo Vas da sve eventualne sugestije i pohvale šaljete na claims@cube.rs<br><br>
                Srdačno Vas pozdravljamo.<br><br>
                Vaš CUBE.<br><br>
                ____________________________________________________________________________<br>
                <img src=cid:myImageID><br></td></tr>
                <tr>
                <td style=""font-family:Century Gothic; font-size: 10pt"">
                CUBE Risk Managment Solutions d.o.o.<br>
                Jurija Gagarina 28<br>
                11070 Novi Beograd<br>
                Serbia<br><br>
                T:           +381 11 414 2823<br><br>
                info@cube.rs<br>
                www.cube.rs<br>
                ____________________________________________________________________________<br>
                Please do not reply to this message.<br>
                Molimo Vas da ne odgovarate na ovaj mail.<br><br></td><tr>
                <tr><td style=""font-family:Century Gothic; font-size: 9pt"">
                Disclaimer:<br>
                This document should only be read by those persons to whom it is addressed and is not intended to be relied upon by any person without subsequent written confirmation of its contents. Accordingly, ”CUBE Risk Management Solutions d.o.o.”, disclaims all responsibility and accept no liability (including in negligence) for the consequences for any person acting, or refraining from acting, on such information prior to the receipt by those persons of subsequent written confirmation. If you have received this E-mail message in error, destroy and delete the message from your computer. Any form of reproduction, dissemination, copying, disclosure, modification, distribution and/or publication of this E-mail message is strictly prohibited. The contents of this e-mail do not necessarily represent the views of the “CUBE Risk Management Solutions d.o.o.”<br><br>
                Odricanje od odgovornosti:<br>
                Ovaj dokument namenjen je samo licima kojima je upucen i za pozivanje na isti od strane bilo kog lica, neophodna je naknadna pismena potvrda njegovog sadzaja. Shodno tome,” CUBE Risk Management Solutions d.o.o.” odrice svaku odgovornost i ne prihvata bilo kakvu obavezu (ukljucujuci slucaj nepaznje) za posledice koje moze pretrpeti bilo koje lice zbog cinjenja ili necinjenja na bazi takve informacije pre nego sto takva lica prime dodatnu pismenu potrvdu. Ukoliko ste greskom primili ovu elektronsku poruku, unistite i izbrisite istu sa vaseg racunara. Svako umnozavanje, sirenje, kopiranje, obelodanjivanje, izmena, distribucija i/ili objavljivanje ove elektronske poruke je strogo zabranjeno.Sadrzaj ove elektronske poruke ne predstavlja nuzno stavove “CUBE Risk Management Solutions d.o.o.”-a.
                </td>
                </tr>
                </table>
                </body>
                </html>";

                mail.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                htmlView.LinkedResources.Add(theEmailImage);

                mail.AlternateViews.Add(htmlView);

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(attachmentPath);
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(Params.MAIL_ZA_SLANJE, Params.MAIL_PASSWORD);
                SmtpServer.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                SmtpServer.Send(mail);
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); return false; }
            Console.WriteLine("Zavrseno slanje maila za: " + klijent);
            return true;
        }

    }
}
