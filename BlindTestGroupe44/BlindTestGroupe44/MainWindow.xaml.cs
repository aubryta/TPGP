using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace BlindTestGroupe44
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MusicPlayer player = new MusicPlayer();

        //private String repertoireMusique = "C:\\Users\\Chantal\\Desktop\\Michael Jackson";
        private String repertoireMusique = "I:\\Music\\C2C\\2012 - Tetra";
        private int nbChoix = 0;
        private int incrPoints = 0;
        private int scorePoints = 0;
        private String nomChanson = "";
        /*
         * 
         * Problemes
         * Génération de bouton dynamique
         * Utiliser un panel plutot qu'une grille serait probablement plus adapté
         * 
         */ 
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// Facile : 3 choix, score incrémenté de 10 points
        /// Moyen : 4 choix, score incrémenté de 12 points
        /// Difficile : 6 choix score incrémenté de 15 points
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.facileButton.IsChecked == true)
            {
                nbChoix = 3;
                incrPoints = 10;
            }

            if (this.moyenButton.IsChecked == true)
            {
                nbChoix = 4;
                incrPoints = 12;
            }
            if (this.difficileButton.IsChecked == true)
            {
                nbChoix = 6;
                incrPoints = 15;
            }

            /* Si l'un des boutons est coché on peux passer à l'étape supérieur et lancer le test
             */
            if (this.facileButton.IsChecked == true
                || this.moyenButton.IsChecked == true
                || this.difficileButton.IsChecked == true)
            {
                initialiseTest(sender, e);
            }
            

        }
        private void initialiseTest(object sender, RoutedEventArgs e)
        {

            grid1.Visibility = Visibility.Hidden;
            grid2.Visibility = Visibility.Visible;

            player.open(repertoireMusique);
            player.play();
            trouveAleatoire();

        }

        private void trouveAleatoire()
        {
            Random rnd = new Random();
            int place = rnd.Next(0, 4); // la on laissera comme ça quand y'aura une liste
            place = 1; // en attendant on met le 1; (place 2);
            r2.Content = player.getChanson() ;
            try
            {
                // Tant qu'on a pas une liste on vérifie pas les doublons
                var songsList = player.listeChanson(repertoireMusique);
                r1.Content = songsList.ElementAt(rnd.Next(0, songsList.Count())).Name;
                r3.Content = songsList.ElementAt(rnd.Next(0, songsList.Count())).Name;
                r4.Content = songsList.ElementAt(rnd.Next(0, songsList.Count())).Name;

            }
            catch { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            player.stop();
            // Tant qu'on a pas de liste on vérifie R2
            if (r2.IsChecked == true)
            {
                scorePoints += incrPoints;
                scoreLabel.Content = "Score : " + scorePoints;
            }

            chansonPrecedente.Content = player.getChanson();
            initialiseTest(sender, e);

        }
    }
}
