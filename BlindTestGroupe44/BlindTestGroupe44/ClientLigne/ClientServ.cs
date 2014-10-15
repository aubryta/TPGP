using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using BlindTestGroupe44.ClientLigne;

namespace BlindTestGroupe44
{
    class ClientServ : IClient
    {
        /*
         * 0 - COMMENTER
         * 1 - Fenetre non redimensionnable dans les 2 clients
         * 2 - fermer proprement la connexion avec le serveur quand on quitte la fenêtre (et fermer l'appli aussi pas que la fenêtre)
         * 3 - Afficher les difficultés dynamiquement
         * 4 - Garde la connexion variable lorsqu'on termine FIN D'UNE PARTIE
         * 5 - Retirer nbChoix ?
         */ 
        private Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private MainWindow wind = new MainWindow();
        private TraiteRequete traiteRequete = new TraiteRequete();
        private Ecoute ecoute = new Ecoute();

        private Stream stm = null;
        IEnumerable<System.Windows.Controls.RadioButton> listeRadioButtons = null;
        //private int nbChoix = 0;
        //private int incrPoints = 0;
        private int score = 0;

        /*
         * 
         */
        public ClientServ(MainWindow mw)
        {
            this.wind = mw;

            traiteRequete.setTraite(this, wind, listeRadioButtons);
            ecoute.setEcoute(traiteRequete);

            //Contrairement à la version local, on choisit ici le style de musique
            //et non la bibliothéque
            wind.choixBibliBouton.Content = "Style de musique";
            connexion();
        }


        public void initialiseTest(object sender, RoutedEventArgs e)
        {
            wind.grid1.Visibility = Visibility.Hidden;
            wind.grid2.Visibility = Visibility.Visible;
        }
        public void validerBoutonClick(object sender, RoutedEventArgs e)
        {
            
            foreach (System.Windows.Controls.RadioButton rb in listeRadioButtons)
            {
                if(rb.IsChecked == true)
                {
                    envoi(Requete.proposeChanson((String) rb.Content));
                    break;
                }
            }
        }

        public void commencerBoutonClick(object sender, RoutedEventArgs e)
        {
            String req = "";
            if (wind.facileButon.IsChecked == true)
            {
                req ="FACILE";
            }

            if (wind.moyenButon.IsChecked == true)
            {
                req = "MOYEN";
            }
            if (wind.difficileButon.IsChecked == true)
            {
                req = "DIFFICILE";
            }

            /* Si l'un des boutons est coché on peux passer à l'étape supérieur et lancer le test
             */
            if (wind.facileButon.IsChecked == true
                || wind.moyenButon.IsChecked == true
                || wind.difficileButon.IsChecked == true)
            {
                envoi(Requete.infoDifficulte(req));
                envoi(Requete.start());
                initialiseTest(sender, e);
            }
        }

        public void runGame()
        {

        }
        public void choisirBibliClick(object sender, RoutedEventArgs e)
        {
            envoi(Requete.demandeStyle());
            wind.commencerBouton.IsEnabled = true;
            //Créer une nouvelle fenêtre avec un bouton par style de musique possible
            //Fermer la fenêtre une fois le choix fait.
            
        }
        private void connexion()
        {
            //On essaye de se connecter au serveur
            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connexion ....");

                tcpclnt.Connect("localhost", 25000);
                
                stm = tcpclnt.GetStream();
                ecoute.setStream(stm);
                Console.WriteLine("Connnexion réussi !");

                //On lance un thread qui va écouter toutes les commandes du serveur
                Thread th = new Thread(ecoute.ecoute);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();

            }

            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.StackTrace);
            }
        }

        public void envoi(String mot)
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(mot);
            stm.Write(ba, 0, ba.Length);
            stm.Flush();
        }

        public void changerVolume(double d)
        {

        }
        public void resetScore()
        {

        }

        public MainWindow getWind()
        {
            return wind;
        }
        public int getScore()
        {
            return score;
        }
        public void setScore(int score)
        {
            this.score = score;
        }
        public void setRadios(IEnumerable<System.Windows.Controls.RadioButton> liste)
        {
            this.listeRadioButtons = liste;
        }
        public IEnumerable<System.Windows.Controls.RadioButton> getRadios()
        {
            return listeRadioButtons;
        }
    }
}
