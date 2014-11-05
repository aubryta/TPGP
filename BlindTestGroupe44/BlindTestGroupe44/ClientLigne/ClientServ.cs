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
    public class ClientServ : IClient
    {
        /// <summary>
        /// Classe correspondant au client en ligne.
        /// gere la communication avec le serveur et envoie
        /// les requetes a une instance de Traitement
        /// </summary>
        private Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private MainWindow window = new MainWindow();
        private Traitement traiteRequete = new Traitement();
        private MusicPlayer musicPlayer = new MusicPlayer();
        private Ecoute ecoute = new Ecoute();
        private String name = "";
        private Stream strm = null;
        IEnumerable<System.Windows.Controls.RadioButton> listeRadioButtons = null;      
        private int score = 0;

       /// <summary>
       /// Constructeur du ClientServ
       /// Thread d'écoute lancé
       /// </summary>
       /// <param name="mw"></param>
        public ClientServ(MainWindow mw)
        {
            this.window = mw;
            traiteRequete.setTraite(this, window, listeRadioButtons);
            ecoute.setTraitement(traiteRequete);
            //Contrairement à la version local, on choisit ici le style de musique
            //et non la bibliothéque
            window.choixBibliBouton.Content = "Style de musique";
            connexion();
        }

        /// <summary>
        /// Lorsque l'utilisateur clique sur valider
        /// envoi au serveur de la chanson "réponse"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Lorsque le client clique sur le bouton commencer
        /// envoi au serveur la difficulté
        /// et demande le lancement d'une partie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void commencerBoutonClick(object sender, RoutedEventArgs e)
        {
            String req = "";
            if (window.facileButon.IsChecked == true)
            {
                req ="FACILE";
            }

            if (window.moyenButon.IsChecked == true)
            {
                req = "MOYEN";
            }
            if (window.difficileButon.IsChecked == true)
            {
                req = "DIFFICILE";
            }

            /* Si l'un des boutons est coché on peux passer à l'étape supérieur et lancer le test
             */
            if (window.facileButon.IsChecked == true
                || window.moyenButon.IsChecked == true
                || window.difficileButon.IsChecked == true)
            {
                envoi(Requete.infoDifficulte(req));
                envoi(Requete.start());
            }
            else
                traiteRequete.message("Il faut choisir une difficulté");
        }

        public void runGame()
        {

        }

        /// <summary>
        /// Envoi au serveur le choix de style apres 
        /// que le client ait cliqué sur le bouton correspondant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void choisirDossierMusique(object sender, RoutedEventArgs e)
        {
            envoi(Requete.demandeStyle());
            window.commencerBouton.IsEnabled = true;     
        }
        private void connexion()
        {
            //On essaye de se connecter au serveur
            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connexion ....");
                tcpclnt.Connect("localhost", 25000);
                Console.WriteLine("connexion : " + tcpclnt.Connected);
                strm = tcpclnt.GetStream();
                ecoute.setStream(strm);
                Console.WriteLine("Connnexion réussi !");
                
                //On change de panel si la connexion est établie
                window.gridDebut.Visibility = Visibility.Hidden;
                window.grid1.Visibility = Visibility.Visible;
                window.BarreDeMenu.Visibility = Visibility.Visible;
                window.imageFondNom.Visibility = Visibility.Hidden;
                window.imageFond.Visibility = Visibility.Visible;

                //On lance un thread qui va écouter toutes les commandes du serveur
                Thread th = new Thread(ecoute.ecoute);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
                // Il faut définir un nom à l'utilisateur
                traiteRequete.pseudo();

            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.StackTrace);
                traiteRequete.erreur("Connexion au serveur impossible");
            }
        }
        /// <summary>
        /// Evoi d'une commande au serveur
        /// </summary>
        /// <param name="mot"></param>
        public void envoi(String mot)
        {
            Console.WriteLine("Envoi " + mot);
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(mot);
            strm.Write(ba, 0, ba.Length);
            strm.Flush();
        }
        /// <summary>
        /// Changement de volume 
        /// </summary>
        /// <param name="d"></param>
        public void changerVolume(double d)
        {
            musicPlayer.volumeFromURL(d);
        }
        public void resetScore()
        {

        }

        public MainWindow getWind()
        {
            return window;
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
        public void quitteAppli()
        {
            if (strm != null)
            {
                envoi(Requete.deconnexion());
                strm.Close();
            }
            System.Environment.Exit(0);
        }
        public String getName()
        {
            return name;
        }
        public void setName(String name)
        {
            this.name = name;
        }

        public void lireChansonUrl(String url)
        {
            musicPlayer.playFromURL(url);
        }

        internal void arretMusique()
        {
            musicPlayer.stopFromURL();     
        }

        internal void askBestScores()
        {
            envoi(Requete.bestScores());
        }
        public void initialiseJeu(object sender, RoutedEventArgs e)
        {

        }
    }
}
