using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Diagnostics;
using BlindTestGroupe44.Main;
namespace BlindTestGroupe44
{
    /// <summary>
    /// Cette classe correspond au 
    /// client local. 
    /// </summary>
    class ClientLocal : IClient
    {
        private MusicPlayer player = new MusicPlayer();
        private String repertoireMusique = ""; // racine de la bibliotheque
        private int nbChoix = 0; // nb de choix de reponse en fonction de la difficulté
        private int incrPoints = 0; // incrementation des points en fonction de la difficulté
        private int scorePoints = 0;
        private int choixCorrect; // index de la bonne reponse
        private IEnumerable<System.Windows.Controls.RadioButton> listeRadioButtons;
        private Stopwatch watch = new Stopwatch();
        private MainWindow wind;

        /// <summary>
        /// Initialisation de l'interface utilisateur
        /// </summary>
        /// <param name="mw"></param>
        public ClientLocal(MainWindow mw)
        {            
            this.wind = mw;
            wind.BarreDeMenu.IsEnabled=false;
            //Les panels d'affichage de mutlijoueur ne sont pas utilisés :
            wind.gridScores.Visibility = Visibility.Hidden;
            wind.gridButton.Visibility = Visibility.Hidden;            
          
        }

        /// <summary>
        /// Initialise la fenetre du jeu, lancement de la musique, affichage des réponses possibles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void initialiseJeu(object sender, System.Windows.RoutedEventArgs e)
        {
            creationRadioButtons(nbChoix);
            listeRadioButtons = wind.grid2.Children.OfType<System.Windows.Controls.RadioButton>();
            wind.grid1.Visibility = Visibility.Hidden;
            wind.grid2.Visibility = Visibility.Visible;           
            runGame();
        }

        /// <summary>
        /// correspond a une question 
        /// choix d'une musique aléatoire
        /// on démarre le chronometre
        /// et on démarre la musique
        /// </summary>
        public void runGame()
        {      
            foreach(System.Windows.Controls.RadioButton rb in listeRadioButtons){
                rb.IsChecked = false;
            }
            player.open(repertoireMusique);
            watch.Start();
            trouveAleatoire();
            player.play();
        }

        
        /// <summary>
        ///Creation des n radiobutton représentant les réponses possible
        /// le parametre n correspond au nbchoix ( en fonction de la difficulté)
        /// </summary>
        /// <param name="n"></param>
        public void creationRadioButtons(int n)
        {
            int y = 256;
            for (int i = 0; i < n; i++)
            {
                System.Windows.Controls.RadioButton r = new System.Windows.Controls.RadioButton();
                r.Margin = new Thickness(256, 256 + i * 40, 0, 0);
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
        }

              
       
        /// <summary>
        /// correspond au bouton valider, si on a la bonne reponse, le score augmente
        /// puis on passe a la chanson suivante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void validerBoutonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            watch.Stop();
            int x = (int)(watch.ElapsedTicks / 1000000); // on recupere le temps écoulé depuis le debut de la manche.
            player.stop();
            if (listeRadioButtons.ElementAt(choixCorrect - 1).IsChecked == true)
            {
                scorePoints += (incrPoints / x);
                wind.scoreLabel.Content = "Score : " + scorePoints + " \t Vous avez répondu en " + watch.ElapsedMilliseconds / 1000 + " secondes";
            }
            watch.Reset();
            wind.chansonPrecedente.Content = "Chanson précédente : " + player.getChanson();
            runGame();
        }
               
        /// <summary>
        /// met a jour le nombre de choix et le coeficient de gain de points
        /// en fonction de la difficulté
        ///  Facile : 3 choix, Coefficient 100 
        /// Moyen : 4 choix, Coefficient 200 
        /// Difficile : 6 choix Coefficient 350       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void commencerBoutonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (wind.facileButon.IsChecked == true)
            {
                nbChoix = 3;
                incrPoints = 100;
            }
            if (wind.moyenButon.IsChecked == true)
            {
                nbChoix = 4;
                incrPoints = 200;
            }
            if (wind.difficileButon.IsChecked == true)
            {
                nbChoix = 6;
                incrPoints = 350;
            }
            /* Si l'un des boutons est coché on peux passer à l'étape supérieure et lancer le jeu
             */
            if (wind.facileButon.IsChecked == true
                || wind.moyenButon.IsChecked == true
                || wind.difficileButon.IsChecked == true)
            {
                wind.BarreDeMenu.IsEnabled = true;
                initialiseJeu(sender, e);
            }
        }
     
        /// <summary>
        /// initialise le contenu des radioButtons avec un nom de chanson (aléatoire) de la bibliothèque choisie
        /// Un des radioButtons contiendra la solution a trouver
        /// </summary>
        private void trouveAleatoire()
        {
            Random rnd = new Random();
            int place = rnd.Next(1, nbChoix); // place ou sera la chanson réponse parmi les button
            var songsList = player.listeChanson(repertoireMusique);
            List<String>chansonsBouton = new List<String>();
            for (int i = 1; i <= nbChoix; i++)
            {
                if (i == place)
                {
                    chansonsBouton.Add(player.getChanson());
                    String nomChanson = player.getChanson();
                    listeRadioButtons.ElementAt(i - 1).Content = nomChanson.Split('.').ElementAt(0); ;
                    chansonsBouton.Add(player.getChanson());
                    this.choixCorrect = i;
                }
                else
                {
                    String element = songsList.ElementAt(rnd.Next(0, songsList.Count())).Name;
                    while (chansonsBouton.Contains(element))
                    {
                        element = songsList.ElementAt(rnd.Next(0, songsList.Count())).Name;
                    }
                    chansonsBouton.Add(element);
                    listeRadioButtons.ElementAt(i - 1).Content = element.Split('.').ElementAt(0); ;
                }
            }
        }

        /// <summary>
        /// Lorsque l'utilisateur clique sur choisir bibliotheque
        /// une fenetre de dialogue s'ouvre et il choisit la racine de sa
        /// bibliothèque de musique. Parcours recursif des fichiers pour stocker les chansons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void choisirDossierMusique(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            repertoireMusique = fbd.SelectedPath;          
            var songsList = player.listeChanson(repertoireMusique);
            //Si le dossier est valide
            if (songsList != null)
            {
                // Si le repertoir ne contient pas au moins 10 chansons
                // message d'erreur 
                if (songsList.Count() < 10)
                {
                    //System.Windows.Forms.MessageBox.Show("La bibliothèque choisie ne contient pas assez de chansons");
                    PopUp popup = new PopUp();
                    popup.setErreur("La bibliothèque choisie ne contient pas assez de chansons");
                    popup.ShowDialog();
                    repertoireMusique = "";
                }
                if (!repertoireMusique.Equals(""))
                    wind.commencerBouton.IsEnabled = true;
            }  
        }

        /// <summary>
        /// Appelle la fonction de changemetn de volume du MusicPLayer
        /// </summary>
        /// <param name="d"></param>
        public void changerVolume(double d)
        {
            player.volume(d);
        }

        /// <summary>
        /// remet le score a 0, ne change pas de bibliotheque et lance une nouvelle série.
        /// </summary>
        public void resetScore()
        {
            scorePoints = 0;
            wind.scoreLabel.Content = "Score : " + scorePoints;
            player.stop();
            runGame();
        }

        /// <summary>
        /// quitte l'application
        /// </summary>
        public void quitteAppli()
        {
            System.Environment.Exit(0);
        }
    }
}
