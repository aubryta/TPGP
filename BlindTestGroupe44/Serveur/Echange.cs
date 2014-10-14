using Serveur.OptionsPartie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serveur
{
    /*
     * Cette classe échange avec le client, 
     * il ecoute traite et envoi des requêtes
     */
    class Echange
    {

        private ASCIIEncoding encodeur = new ASCIIEncoding();



        private bool jeuFini = false;
        private GestionMusique gestMusique = new GestionMusique();
        private int nbChanson = 3;

        public void Connexion(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream cstm = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;


            while (true)
            {
                bytesRead = 0;
                try
                {
                    //Attend la reception d'un message
                    bytesRead = cstm.Read(message, 0, 4096);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERREUR RECEPTION" + e.ToString());
                    break;
                }

                if (bytesRead == 0)
                {
                    Console.WriteLine("CLIENT DECONNECTE");
                    break;
                }

                //Affiche le message reçu


                String bufferincmessage = encodeur.GetString(message, 0, bytesRead);

                traite(bufferincmessage, cstm);
            }
        }

        private void traite(String message, NetworkStream cstm)
        {
            Console.WriteLine("Recoit : " + message);
            String[] tabMessage = message.Split('?');
            if (tabMessage[0].Equals(""))
            {
                send("MESSAGE?Message mal formé", cstm);
                Console.WriteLine("***** Erreur lecture commande *****");
            }
            else
            {
                if (tabMessage[0].Equals("CHANSON"))
                {
                    traiteChanson(tabMessage, cstm);
                }
                else if (tabMessage[0].Equals("INFO"))
                {
                    traiteInfo(tabMessage, cstm);
                }
                else if (tabMessage[0].Equals("CHOIXSTYLE"))
                {
                    String res = "CHOIXSTYLE";
                    foreach (String styl in gestMusique.choixStyle())
                        res += "?" + styl;
                    send(res, cstm);
                }
            }
        }

        /// <summary>
        /// Trouve aléatoirement une playlist de chanson
        /// </summary>
        /// <returns></returns>

        private void send(String reponse, NetworkStream clientStream)
        {
            Console.WriteLine("Envoi : " + reponse);
            byte[] buffer = encodeur.GetBytes(reponse);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        private void traiteChanson(String[] tabMessage, NetworkStream cstm)
        {
            if (tabMessage[1].Equals(gestMusique.getChanson()))
            {
                send("INFO?BONNECHANSON?" + gestMusique.getChanson(), cstm);
            }
            else //mauvaise chanson
            {
                send("INFO?MAUVAISECHANSON?" + gestMusique.getChanson(), cstm);
            }
            if (jeuFini == false)
            {
                String res = "MUSIQUE";
                foreach (String chansonCourante in gestMusique.listeChansons(nbChanson))
                {
                    res += "?" + chansonCourante;
                }
                send(res, cstm);
            }
        }

        private void traiteInfo(String[] tabMessage,NetworkStream cstm)
        {
            if (tabMessage[1].Equals("START"))
            {
                String res = "MUSIQUE";
                foreach (String chanson in gestMusique.listeChansons(nbChanson))
                    res += "?" + chanson;
                send(res, cstm);
            }
            else if (tabMessage[1].Equals("STYLE"))
            {
                gestMusique.setStyle(tabMessage[2]);
            }
            else if (tabMessage[1].Equals("DIFFICULTE"))
            {
                OptionsFactory of = new OptionsFactory();
                IOptions io = of.createOptions(tabMessage[2]);

                if (io == null)
                    send("ERREUR difficulté inconnu", cstm);
                else
                    send("OPTIONS?" + io.getNbChoix() + "?" + io.getIncr(), cstm);
            }
        }
    }
}
