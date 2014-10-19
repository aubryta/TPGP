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
        private TraiteRequete traiteReq = null;
        private Stream stm = null;

        public void setEcoute(TraiteRequete traiteReq)
        {
            this.traiteReq = traiteReq;
        }

        public void setStream(Stream stm)
        {
            this.stm = stm;
        }

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
        /*
         * Fonction qui analyse une commande et fait le traitement correspondant
         */
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
                    //La manche est donc termine, on peux envoyer la réponse que l'on a coché
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() => traiteReq.envoiReponse()));
                    List<String> chansons = new List<String>();
                    for (int i = 1; i < tabMessage.Length; i++)
                    {
                        chansons.Add(tabMessage[i]);
                    }
                    //On crée les boutons radios correspondant
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() => traiteReq.creationRadioButtons(chansons)));
                }
                else if (tabMessage[0].Equals("INFO"))
                {
                    //Si la chanson est bonne, on incrémente le score
                    //Et on affiche la chanson précédente dans le label correspondant
                    if (tabMessage[1].Equals("BONNECHANSON"))
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => traiteReq.chansonPrecedente(tabMessage[2])));
                    }
                    else if (tabMessage[1].Equals("MAUVAISECHANSON"))
                    {
                        //Sinon on affiche juste la chanson 
                        Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => traiteReq.chansonPrecedente(tabMessage[2])));
                    }
                    else if(tabMessage[1].Equals("PSEUDOINCORRECT"))
                    {
                        traiteReq.pseudoErreur();
                    }
                    else if(tabMessage[1].Equals("SCORES"))
                    {
                        //INFO?SCORES?joueur&score?joueur&score?...
                        Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => traiteReq.infoScores(tabMessage)));
                    }
                }
                else if (tabMessage[0].Equals("DECONNEXION"))
                {
                    //Ferme la socket
                }
                else if (tabMessage[0].Equals("MESSAGE"))
                {//affiche un simple message
                    traiteReq.message(tabMessage[1]);
                }
                else if (tabMessage[0].Equals("OPTIONS"))
                {//initialise l'incrémentation des points à chaque bonne réponse et le nombre de bonne répone
                    traiteReq.initialisationOptions(int.Parse(tabMessage[1]));
                }
                else if (tabMessage[0].Equals("CHOIXSTYLE"))
                {
                    List<String> listStyles = new List<String>();
                    for (int i = 1; i < tabMessage.Count(); i++)
                    {
                        listStyles.Add(tabMessage[i]);
                    }
                    traiteReq.fenetreStyle(listStyles);
                }
                else if(tabMessage[0].Equals("ERREUR"))
                {
                    traiteReq.erreur(tabMessage[1]);
                }
                else
                {
                    traiteReq.erreur("Commande inconnue" + message);
                }
            }
        }
    }
}
