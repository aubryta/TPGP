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
        private int nbChoix = 0;
        private int incrPoints = 0;
        private int scorePoints = 0;

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

            //On nettoie le panel précédent
            grid1.Children.Clear();
            
            //On crée un liste de radio bouttons avec des titres de chanson aléatoire sauf 1
            // qui comporte la bonne réponse
            /*RadioButton[] tabChoix = new RadioButton[nbChoix];
            for (int i = 0; i < nbChoix; i++)
            {
                tabChoix[i] = new RadioButton();
                tabChoix[i].Content = "salut" + i;
                tabChoix[i].Margin = new Thickness(50, 60 + (i * 25), 0, 0);
                tabChoix[i].Name = "tabchoix" + i;
                grid1.Children.Add(tabChoix[i]);
            }*/
            List<RadioButton> listeChoix = new List<RadioButton>();
            for (int i = 0; i < nbChoix; i++)
            {
                listeChoix.Add(new RadioButton());
                listeChoix[i].Content = "Salut " + i;
                listeChoix[i].Margin = new Thickness(50, 60 + (i * 25), 0, 0);
                grid1.Children.Add(listeChoix[i]);
            }

            // On crée et affiche un label avec  le score
            Label score = new Label();
            score.Margin = new Thickness(50, 20, 0, 0);
            score.Content = "Score : " + scorePoints;
            grid1.Children.Add(score);
            InitializeComponent();
        }
        
    }
}
