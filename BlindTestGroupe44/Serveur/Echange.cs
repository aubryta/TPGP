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


        //private GestionMusique gestMusique = new GestionMusique();
        private Joueur joueur = null;
        private Serveur serv = null;
        private Partie partie = null;

        public void Connexion(object client, object j, Serveur s)
        {
            TcpClient tcpClient = (TcpClient)client;
            this.joueur = j as Joueur;
            this.serv = s;

            NetworkStream cstm = tcpClient.GetStream();
            joueur.setStream(cstm);


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
                send(Requete.erreur("Message mal forme"), cstm);
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
                    GestionMusique gm = new GestionMusique();
                    List<String> listeStyle = gm.choixStyle(); 
                    if (listeStyle == null)
                    {
                        send(Requete.erreur("Pas de style de musique defini"), cstm);
                    }
                    else
                    {
                        foreach (String styl in listeStyle)
                            res += "?" + styl;
                        send(res, cstm);
                    }
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
            if (tabMessage[1].Equals(partie.getChanson()))
            {
                send("INFO?BONNECHANSON?" + partie.getChanson(), cstm);
                joueur.incrScore();
            }
            else //mauvaise chanson
            {
                send("INFO?MAUVAISECHANSON?" + partie.getChanson(), cstm);
            }
        }

        private void traiteInfo(String[] tabMessage,NetworkStream cstm)
        {
            if (tabMessage[1].Equals("START"))
            {
                //Le joueur est pret il peut donc jouer
                //On lui envoi le nombre de réponse à afficher
                send(Requete.options(joueur.getNbChoix(), joueur.getIncr()), joueur.getStream());
                //On l'ajoute à la partie
                serv.getPartie(joueur.getStyle()).addJoueur(joueur);
            }
            else if (tabMessage[1].Equals("STYLE"))
            {
                joueur.setStyle(tabMessage[2]);
            }
            else if (tabMessage[1].Equals("DIFFICULTE"))
            {
                joueur.setDifficulte(tabMessage[2]);
                
            }
            else if(tabMessage[1].Equals("NAME"))
            {
                /*if(!tabMessage[2].Equals("t"))
                    send("INFO?PSEUDOINCORRECT", cstm);
                 */
                joueur.setName(tabMessage[2]);

            }
        }
    }
}
