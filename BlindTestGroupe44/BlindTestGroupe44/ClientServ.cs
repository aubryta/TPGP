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

namespace BlindTestGroupe44
{
    class ClientServ : IClient
    {

        private Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private MainWindow wind = new MainWindow();
        private ASCIIEncoding encodeur = new ASCIIEncoding();
        private Stream stm;
        private bool gardeConnexion = true;
        IEnumerable<System.Windows.Controls.RadioButton> listeRadioButtons = null;
        private int nbChoix = 0;
        private int incrPoints = 0;
        private int score = 0;
        /*
         * 
         */
        public ClientServ(MainWindow mw)
        {
            this.wind = mw;
            wind.choixBibliBouton.Content = "Style de musique";
            wind.commencerBouton.IsEnabled = true;
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
                    envoi("CHANSON?" + rb.Content);
                    break;
                }
            }
        }

        public void commencerBoutonClick(object sender, RoutedEventArgs e)
        {
            String req = "";
            if (wind.facileButon.IsChecked == true)
            {
                req ="OPTIONS?FACILE";
            }

            if (wind.moyenButon.IsChecked == true)
            {
                req ="OPTIONS?MOYEN";
            }
            if (wind.difficileButon.IsChecked == true)
            {
                req ="OPTIONS?DIFFICILE";
            }

            /* Si l'un des boutons est coché on peux passer à l'étape supérieur et lancer le test
             */
            if (wind.facileButon.IsChecked == true
                || wind.moyenButon.IsChecked == true
                || wind.difficileButon.IsChecked == true)
            {
                envoi(req);
                envoi("INFO?START");
                initialiseTest(sender, e);
            }
        }

        public void runGame()
        {

        }
        public void choisirBibliClick(object sender, RoutedEventArgs e)
        {
            
            //Créer une nouvelle fenêtre avec un bouton par style de musique possible
            //Fermer la fenêtre une fois le choix fait.
            
        }
        private void ecoute()
        {
            while (gardeConnexion)
            {
                int bytesRead = 0;
                byte[] message = new byte[4096];
                bytesRead = stm.Read(message, 0, 4096);
                String bufferincmessage = encodeur.GetString(message, 0, bytesRead);
                traite(bufferincmessage);
            }
        }
        private void connexion()
        {

            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");

                tcpclnt.Connect("localhost", 25000);
                // use the ipaddress as in the server program
                
                stm = tcpclnt.GetStream();

                Thread th = new Thread(ecoute);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
                if(gardeConnexion == false)
                    tcpclnt.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.StackTrace);
            }
        }
        public void traite(String message)
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
                    score = int.Parse(tabMessage[1]);
                }
                else if (tabMessage[0].Equals("MUSIQUE"))
                {
                    List<String> chansons = new List<String>();
                    for(int i = 1; i < tabMessage.Length; i++)
                    {
                        chansons.Add(tabMessage[i]);
                    }
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() => creationRadioButtons(chansons)));
                
                    
                }
                else if (tabMessage[0].Equals("INFO"))
                {
                    if (tabMessage[1].Equals("BONNECHANSON"))
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => changeScore()));
                        Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => chansonPrecendente(tabMessage[2])));
                    }
                    else if(tabMessage[1].Equals("MAUVAISECHANSON"))
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => chansonPrecendente(tabMessage[2])));
                    }
                }
                else if (tabMessage[0].Equals("DECONNEXION"))
                {

                }
                else if (tabMessage[0].Equals("MESSAGE"))
                {
                    Console.WriteLine(message);
                }
                else if (tabMessage[0].Equals("OPTIONS"))
                {
                    nbChoix = int.Parse(tabMessage[1]);
                    incrPoints = int.Parse(tabMessage[2]);
                }
                else
                {
                    Console.WriteLine("Erreur message : " + message);
                }
            }
        }


        public void changeScore()
        {
            score += incrPoints;
            wind.scoreLabel.Content = "Score : " + score;
        }
        public void chansonPrecendente(String chanson)
        {
            wind.chansonPrecedente.Content = "Chanson précédente : " + chanson;
        }
        public void creationRadioButtons(List<String> chansons)
        {
            
            if (listeRadioButtons != null) // Si une liste de boutons existe
            {
                editRadioBoutton(chansons);
            }
            else
            { //Si la liste n'existe pas, on l'a créé, ainsi que le bouton valider
                creeRadioBouton(chansons);
            }
        }
        private void editRadioBoutton(List<String> chansons)
        {
            for (int i = 0; i < nbChoix; i++)
            {
                listeRadioButtons.ElementAt(i).IsChecked = false;
                listeRadioButtons.ElementAt(i).Content = chansons.ElementAt(i);
            }
        }
        private void creeRadioBouton(List<String> chansons)
        {
            int y = 256;
            foreach (String chanson in chansons)
            {
                System.Windows.Controls.RadioButton r = new System.Windows.Controls.RadioButton();
                r.Margin = new Thickness(256, y, 0, 0);
                r.Content = chanson;
                r.FontFamily = new System.Windows.Media.FontFamily("Arial Rounded MT Bold");
                wind.grid2.Children.Add(r);
                y = y + 40;
            }
            System.Windows.Controls.Button valider = new System.Windows.Controls.Button();
            valider.Click += validerBoutonClick;
            valider.Content = "Valider";
            valider.Width = 164;
            valider.Height = 61;
            valider.Margin = new Thickness(256, y, 0, 0);
            valider.FontFamily = new System.Windows.Media.FontFamily("Arial Rounded MT Bold");
            valider.FontSize = 16;
            valider.Visibility = Visibility.Visible;
            valider.IsEnabled = true;

            wind.grid2.Children.Add(valider);

            //on finit par ajouter tous les boutons à la liste 
            listeRadioButtons = wind.grid2.Children.OfType<System.Windows.Controls.RadioButton>();
        }
        private void envoi(String mot)
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
    }
}
