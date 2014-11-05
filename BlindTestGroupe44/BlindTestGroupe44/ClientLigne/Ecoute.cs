using BlindTestGroupe44.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BlindTestGroupe44.ClientLigne
{
    class Ecoute
    {

        private Boolean gardeConnexion = true;
        private ASCIIEncoding encodeur = new ASCIIEncoding();
        private Traitement traitement = null;
        private Stream stm = null;

        /// <summary>
        /// Instancie la classe qui va traiter les requêtes.
        /// </summary>
        /// <param name="traitement">Classe qui va traiter les requêtes</param>
        public void setTraitement(Traitement traitement)
        {
            this.traitement = traitement;
        }

        /// <summary>
        /// Initialise le stream du client
        /// </summary>
        /// <param name="stm">Le stream du client</param>
        public void setStream(Stream stm)
        {
            this.stm = stm;
        }

        /// <summary>
        /// Ecoute en boucle les messages envoyés par le serveur
        /// </summary>
        public void ecoute()
        {
            while (gardeConnexion)
            {
                int bytesRead = 0;
                byte[] message = new byte[4096];
                String bufferincmessage = "";
                try
                {
                    bytesRead = stm.Read(message, 0, 4096);
                    bufferincmessage = encodeur.GetString(message, 0, bytesRead);
                    Console.WriteLine("Je recoit " + bufferincmessage);
                }
                catch
                {
                    PopUp quitte = new PopUp();
                    quitte.setMessage("Connexion au serveur interrompue");
                    quitte.ShowDialog();
                    System.Environment.Exit(0);
                }
                parse(bufferincmessage);
            }
        }

        /// <summary>
        /// Fonction qui analyse une commande et fait le traitement correspondant
        /// En appelant la classe traiteRequete
        /// </summary>
        /// <param name="message">Le message recut par le serveur</param>
        private void parse(String message)
        {
            //La commande est "spliter" à chaque ? voir rapport commandes
            String[] tabMessage = message.Split('?');
            if (tabMessage[0].Equals(""))
            {
            }
            else
            {
                if (tabMessage[0].Equals("MUSIQUE"))
                {
                    //On reçoit la liste des chansons de la manche
                    //La manche précédente est donc termine, on peux envoyer la réponse que l'on a coché
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() => traitement.envoiReponse()));
                    List<String> chansons = new List<String>();
                    for (int i = 1; i < tabMessage.Length; i++)
                    {
                        chansons.Add(tabMessage[i]);
                    }
                    //On crée les boutons radios correspondant
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() => traitement.creationRadioButtons(chansons)));
                }
                else if (tabMessage[0].Equals("INFO"))
                {
                    //Et on affiche la chanson précédente dans le label correspondant
                    if (tabMessage[1].Equals("BONNECHANSON"))
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => traitement.chansonPrecedente(tabMessage[2])));
                    }
                    else if (tabMessage[1].Equals("MAUVAISECHANSON"))
                    {
                        //Sinon on affiche juste la chanson 
                        Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => traitement.chansonPrecedente(tabMessage[2])));
                    }
                    else if(tabMessage[1].Equals("PSEUDOINCORRECT"))
                    {
                        traitement.pseudoErreur();
                    }
                    else if(tabMessage[1].Equals("SCORES"))
                    {
                        //INFO?SCORES?joueur&score?joueur&score?...
                        Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => traitement.infoScores(tabMessage)));
                    }
                    else if (tabMessage[1].Equals("CHANSON"))
                    {
                        traitement.lireChanson(tabMessage[2]);
                    }
                    else if(tabMessage[1].Equals("PARTIEFINIE"))
                    {                        
                        List<String> scores = new List<String>();
                        for (int i = 2; i < tabMessage.Count(); i++ )
                        {
                            String []scoreTab = tabMessage[i].Split('=');
                            scores.Add(scoreTab[0]); //On ajoute le nom
                            scores.Add(scoreTab[1]); //puis le score correspondant
                        }
                        Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => traitement.partieFinie(scores)));
                    }
                    else if(tabMessage[1].Equals("NOUVELLEPARTIE"))
                    {
                        //On remet le panel pour jouer
                        Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => traitement.nouvellePartie()));
                    }
                }
                else if (tabMessage[0].Equals("DECONNEXION"))
                {
                    //Ferme la socket
                }
                else if (tabMessage[0].Equals("MESSAGE"))
                {//affiche un simple message
                    traitement.message(tabMessage[1]);
                }
                else if (tabMessage[0].Equals("OPTIONS"))
                {//initialise l'incrémentation des points à chaque bonne réponse et le nombre de bonne répone
                    traitement.initialisationOptions(int.Parse(tabMessage[1]));
                }
                else if (tabMessage[0].Equals("CHOIXSTYLE"))
                {
                    List<String> listStyles = new List<String>();
                    for (int i = 1; i < tabMessage.Count(); i++)
                    {
                        listStyles.Add(tabMessage[i]);
                    }
                    traitement.fenetreStyle(listStyles);
                }
                else if(tabMessage[0].Equals("ERREUR"))
                {
                    traitement.erreur(tabMessage[1]);
                }
                else if (tabMessage[0].Equals("BESTSCORE"))
                {
                    Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => traitement.infoBestScores(tabMessage)));


                }
                else if (tabMessage[0].Equals("FINDEPARTIE"))
                {
                    //Les scores sont en cours de calcul, on affiche le panels et on attend les scores
                    //On remet le panel pour jouer
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() => traitement.findepartie()));
                }
                else
                {
                    traitement.erreur("Commande inconnue" + message);
                }
            }
        }
    }
}
