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

        /// <summary>
        /// Initialise la classe échange est écoute en boucle sur un client
        /// </summary>
        /// <param name="client">Le client sous format tcp</param>
        /// <param name="j">Un joueur j</param>
        /// <param name="s">Et le serveur pour accéder à la partie</param>
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
                catch
                {
                    Console.WriteLine("ERREUR RECEPTION");
                }

                if (bytesRead == 0)
                {
                    Console.WriteLine("CLIENT DECONNECTE");
                    break;
                }

                //Affiche le message reçu
                String bufferincmessage = encodeur.GetString(message, 0, bytesRead);

                traite(bufferincmessage);
            }
        }

        /// <summary>
        /// Toutes les requêtes reçu par le serveur sont traités ici
        /// </summary>
        /// <param name="message">Requête reçu</param>
        private void traite(String message)
        {
            Console.WriteLine("Recoit : " + message);
            String[] tabMessage = message.Split('?');
            if (tabMessage[0].Equals(""))
            {
                send(Requete.erreur("Message mal forme"), joueur.getStream());
            }
            else
            {
                if (tabMessage[0].Equals("CHANSON"))
                {
                    traiteChanson(tabMessage);
                }
                else if (tabMessage[0].Equals("INFO"))
                {
                    traiteInfo(tabMessage);
                }
                else if (tabMessage[0].Equals("CHOIXSTYLE"))
                {
                    GestionMusique gm = new GestionMusique();
                    List<String> listeStyle = gm.choixStyle();
                    if (listeStyle == null)
                    {
                        send(Requete.erreur("Pas de style de musique defini"), joueur.getStream());
                    }
                    else
                    {
                        send(Requete.choixStyle(listeStyle), joueur.getStream());
                    }
                }
                else if( tabMessage [0].Equals("DECONNEXION"))
                {
                    Console.WriteLine("Le joueur " + joueur.getName() + " est retiré du serveur");
                    partie.removeJoueur(joueur);
                }
            }
        }

        /// <summary>
        /// Envoi au stream d'un client un message
        /// </summary>
        /// <param name="message">Le message à envoyer</param>
        /// <param name="clientStream">Le stream du client</param>
        private void send(String message, NetworkStream clientStream)
        {
            Console.WriteLine("Envoi : " + message);
            byte[] buffer = encodeur.GetBytes(message);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        /// <summary>
        /// Traite les requêtes de choix de réponse du client
        /// </summary>
        /// <param name="tabMessage">les éléments de la requête</param>
        private void traiteChanson(String[] tabMessage)
        {
            if (tabMessage[1].Equals(partie.getChanson().Replace('_', ' ').Split('.').ElementAt(0)))
            {
                send(Requete.infoBonneChanson(partie.getChanson()), joueur.getStream());
                joueur.incrScore();
            }
            else //mauvaise chanson
            {
                send(Requete.infoMauvaiseChanson(partie.getChanson()), joueur.getStream());
            }
            //On envoi les socres à tous le monde dès qu'il y'a potentiellement un changement
            partie.envoiScores();
        }

        /// <summary>
        /// Traite les requêtes de type informations
        /// </summary>
        /// <param name="tabMessage">les éléments de la requête</param>
        private void traiteInfo(String[] tabMessage)
        {
            if (tabMessage[1].Equals("START"))
            {
                //Le joueur est pret il peut donc jouer
                //On lui envoi le nombre de réponse à afficher
                send(Requete.options(joueur.getNbChoix()), joueur.getStream());
                //On l'ajoute à la partie
                serv.getPartie(joueur.getStyle()).addJoueur(joueur);
                partie = serv.getPartie(joueur.getStyle());
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
                Console.WriteLine(tabMessage[2]);
                if (serv.existePseudo(tabMessage[2]))
                {
                    Console.WriteLine(tabMessage[2]);
                    send(Requete.infoPseudoIncorrect(), joueur.getStream());
                }
                else
                {
                    Console.WriteLine(tabMessage[2]);
                    joueur.setName(tabMessage[2]);
                }
            }
        }
    }
}
