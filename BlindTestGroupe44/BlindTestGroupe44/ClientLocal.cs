using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace BlindTestGroupe44
{
    class ClientLocal : IClient
    {
        private MusicPlayer player = new MusicPlayer();

        private String repertoireMusique = ""; // racine de la bibliotheque
        private int nbChoix = 0; // nb de choix de reponse en fonction de la difficulté
        private int incrPoints = 0; // incrementation des points en fonction de la difficulté
        private int scorePoints = 0;
        private int choixCorrect; // index de la bonne reponse
        private IEnumerable<System.Windows.Controls.RadioButton> listeRadioButtons;

        private MainWindow wind;


        public ClientLocal(MainWindow mw)
        {
            this.wind = mw;
        }

        // Initialise la fenetre du jeu, lancement de la musique, affichage des réponses possibles
        public void initialiseTest(object sender, System.Windows.RoutedEventArgs e)
        {
            creationRadioButtons(nbChoix);
            listeRadioButtons = wind.grid2.Children.OfType<System.Windows.Controls.RadioButton>();
            wind.grid1.Visibility = Visibility.Hidden;
            wind.grid2.Visibility = Visibility.Visible;
            runGame();
            
        }

        public void runGame()
        {      
            foreach(System.Windows.Controls.RadioButton rb in listeRadioButtons){
                rb.IsChecked = false;
            }
            player.open(repertoireMusique);
            trouveAleatoire();
            player.play();
        }

        // Creation des n radiobutton représentant les réponses possible
        // le parametre n correspond au nbchoix ( en fonction de la difficulté)
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

        // correspond au bouton valider, si on a la bonne reponse, le score augmente
        // puis on passe a la chanson suivante
        public void validerBoutonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            player.stop();
            // Tant qu'on a pas de liste on vérifie R2
            if (listeRadioButtons.ElementAt(choixCorrect-1).IsChecked == true)
            {
                scorePoints += incrPoints;
                wind.scoreLabel.Content = "Score : " + scorePoints;
            }
            wind.chansonPrecedente.Content = "Chanson précédente : " + player.getChanson();      
            
            runGame();
        }



        /// Facile : 3 choix, score incrémenté de 10 points
        /// Moyen : 4 choix, score incrémenté de 12 points
        /// Difficile : 6 choix score incrémenté de 15 points
        public void commencerBoutonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (wind.facileButon.IsChecked == true)
            {
                nbChoix = 3;
                incrPoints = 10;
            }

            if (wind.moyenButon.IsChecked == true)
            {
                nbChoix = 4;
                incrPoints = 12;
            }
            if (wind.difficileButon.IsChecked == true)
            {
                nbChoix = 6;
                incrPoints = 15;
            }

            /* Si l'un des boutons est coché on peux passer à l'étape supérieur et lancer le test
             */
            if (wind.facileButon.IsChecked == true
                || wind.moyenButon.IsChecked == true
                || wind.difficileButon.IsChecked == true)
            {
                initialiseTest(sender, e);
            }
        }

        //initialise le contenu des radioButtons avec un nom de chanson (aléatoire) de la bibliothèque choisie
        //1 des radioButtons contiendra la solution a trouver
        private void trouveAleatoire()
        {
            Random rnd = new Random();
            int place = rnd.Next(1, nbChoix);
            var songsList = player.listeChanson(repertoireMusique);
            for (int i = 1; i <= nbChoix; i++)
            {
                if (i == place)
                {
                    listeRadioButtons.ElementAt(i-1).Content = player.getChanson();
                    this.choixCorrect = i;
                }
                else
                {
                    listeRadioButtons.ElementAt(i - 1).Content = songsList.ElementAt(rnd.Next(0, songsList.Count())).Name;
                }
            }
        }

        public void choisirBibliClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            repertoireMusique = fbd.SelectedPath;
            wind.commencerBouton.IsEnabled = true;
        }

        public void changerVolume(double d)
        {
            player.volume(d);
        }

        // remet le score a 0, ne change pas de bibliotheque et lance une nouvelle série.
        public void resetScore()
        {
            scorePoints = 0;
            wind.scoreLabel.Content = "Score : " + scorePoints;
            player.stop();
            runGame();
        }
    }
}
