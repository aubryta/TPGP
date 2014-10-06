using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace BlindTestGroupe44
{
    class ClientServ : IClient
    {

        private Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private MainWindow wind = new MainWindow();
        private ASCIIEncoding encodeur = new ASCIIEncoding();

        public ClientServ(MainWindow mw)
        {
            this.wind = mw;
            connexion();
        }


        public void initialiseTest(object sender, RoutedEventArgs e)
        {

        }
        public void validerBoutonClick(object sender, RoutedEventArgs e)
        {
            connexion();
        }
        public void commencerBoutonClick(object sender, RoutedEventArgs e)
        {

        }
        public void runGame()
        {

        }


        public void creerBoutonRadio(List<string> listeChanson, int numChanson)
        {

        }

        
        public void choisirBibliClick(object sender, RoutedEventArgs e)
        {

        }
        private void connexion()
        {

            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");

                tcpclnt.Connect("localhost", 25000);
                // use the ipaddress as in the server program
                int bytesRead = 0;
                byte[] message = new byte[4096];
                String str = "salut";
                Stream stm = tcpclnt.GetStream();

                for(int i = 0; i < 10; i++)
                {
                    envoi(str + i, stm);


                    bytesRead = stm.Read(message, 0, 4096);
                    //recoit(stm);
                    String bufferincmessage = encodeur.GetString(message, 0, bytesRead);
                    traite(bufferincmessage, stm);

                }
                
                tcpclnt.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.StackTrace);
            }
        }
        public void traite(String message, Stream stm)
        {
            
            String[] tabMessage = message.Split('?');
            if(tabMessage[0].Equals(""))
            { 
                Console.WriteLine("***** Erreur lecture commande *****");

            }
            else
            {
                if(tabMessage[0].Equals("SCORE"))
                {

                }
                else if (tabMessage[0].Equals("MUSIQUE"))
                {

                }
                
                else if (tabMessage[0].Equals("DECONNEXION"))
                {

                }
                else if (tabMessage[0].Equals("MESSAGE"))
                {
                    Console.WriteLine(message);
                }
                else
                {
                    Console.WriteLine("Erreur message : " + message);
                }
            }
        }


        private void envoi(String mot, Stream stm)
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(mot);
            stm.Write(ba, 0, ba.Length);
        }

        private void recoit(Stream stm)
        {
            byte[] bb = new byte[100];
            int k = stm.Read(bb, 0, 100);
            for (int j = 0; j < k; j++)
                Console.Write(Convert.ToChar(bb[j]));
        }
        public void changerVolume(double d)
        {

        }

        public void resetScore()
        {

        }
    }
}
