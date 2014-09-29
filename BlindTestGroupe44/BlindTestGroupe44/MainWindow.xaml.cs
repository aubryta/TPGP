using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
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

        private String repertoireMusique = ""; // racine de la bibliotheque
        private int nbChoix = 0; // nb de choix de reponse en fonction de la difficulté
        private int incrPoints = 0; // incrementation des points en fonction de la difficulté
        private int scorePoints = 0;
        private String nomChanson = "";
        private int choixCorrect; // index de la bonne reponse
      
        public MainWindow()
        {           
            InitializeComponent();
            commencerBoutton.IsEnabled = false;
        }

        /// <summary>
        /// 
        /// Facile : 3 choix, score incrémenté de 10 points
        /// Moyen : 4 choix, score incrémenté de 12 points
        /// Difficile : 6 choix score incrémenté de 15 points
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
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

        // Creation des n radiobutton représentant les réponses possible
        // le parametre n correspond au nbchoix ( en fonction de la difficulté)
        private void creationRadioButtons(int n)
        {
            int y = 126;
            for (int i = 0; i < n; i++)
            {
                System.Windows.Controls.RadioButton r = new System.Windows.Controls.RadioButton();                
                r.Margin = new Thickness(256, 126 +i*40, 0, 0);               
                grid2.Children.Add(r);                        
                y = y + 40;               
            }
            System.Windows.Controls.Button valider = new System.Windows.Controls.Button();
            valider.Click += Button_Click;
            valider.Content = "Valider";
            valider.Width = 164;
            valider.Height = 61;
            valider.Margin = new Thickness(256, y, 0, 0);
            valider.Visibility = Visibility.Visible;
            valider.IsEnabled = true;
            grid2.Children.Add(valider);

        }

       
        // Initialise la fenetre du jeu, lancement de la musique, affichage des réponses possibles
        private void initialiseTest(object sender, RoutedEventArgs e)
        {
            creationRadioButtons(nbChoix);
            grid1.Visibility = Visibility.Hidden;         
            grid2.Visibility = Visibility.Visible;
            player.open(repertoireMusique);
            player.play();
            trouveAleatoire();
        }

        //initialise le contenu des radioButtons avec un nom de chanson (aléatoire) de la bibliothèque choisie
        //1 des radioButtons contiendra la solution a trouver
        private void trouveAleatoire()
        {
            Random rnd = new Random();
            int place = rnd.Next(1, nbChoix);         
            var songsList = player.listeChanson(repertoireMusique);
            for (int i = 1; i <= nbChoix; i++ )
            {
                if (i == place)
                {
                   getRi(i).Content = player.getChanson();
                   choixCorrect = i;
                }
                else
                {
                    getRi(i).Content = songsList.ElementAt(rnd.Next(0, songsList.Count())).Name;
                }               
            }        
        }

        // recupere le RadioButton a l'index i dans la grille
        private System.Windows.Controls.RadioButton getRi(int i)
        {
           var radios = grid2.Children.OfType<System.Windows.Controls.RadioButton>();       
            switch (i)
            {
                case 1:
                    return radios.ElementAt(0);
                case 2:
                    return radios.ElementAt(1);
                case 3:
                    return radios.ElementAt(2);
                case 4:
                    return radios.ElementAt(3);
                case 5:
                    return radios.ElementAt(4);
                case 6:
                    return radios.ElementAt(5);
                default:
                    return radios.ElementAt(1);
            }
        }

        //correspond au bouton valider, si on a la bonne reponse, le score augmente
        // puis on passe a la chanson suivante
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            player.stop();
            // Tant qu'on a pas de liste on vérifie R2
            if (getRi(this.choixCorrect).IsChecked == true)
            {
                scorePoints += incrPoints;
                scoreLabel.Content = "Score : " + scorePoints;
            }
            chansonPrecedente.Content = "reponse precedente : " + player.getChanson();
            initialiseTest(sender, e);
        }

        private void choisirBibli(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog() ;
            fbd.ShowDialog();
            repertoireMusique = fbd.SelectedPath;
            commencerBoutton.IsEnabled = true;
        }


    }
}
