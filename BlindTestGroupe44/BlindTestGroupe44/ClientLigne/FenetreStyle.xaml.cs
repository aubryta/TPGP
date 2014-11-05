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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BlindTestGroupe44.ClientLigne
{
    /// <summary>
    /// Fenetre qui affichie les différents 
    /// styles de musique offerts sur le serveur
    /// </summary>
    public partial class FenetreStyle : Window
    {
        private IEnumerable<System.Windows.Controls.Button> listeButtons = null;
        private Traitement traiteReq = null;
        private String style = null;

        public FenetreStyle(Traitement traiteReq)
        {
            InitializeComponent();
            this.traiteReq = traiteReq;
            //On désactive la grille principale pour ne pas pouvoir y faire
            //de modification pendant le choix du style de musique
            traiteReq.activeFenetre(false); 
        }

       
        /// <summary>
        ///  Crée une liste de bouton avec leurs paramètres suivant la liste de String
        /// en arguments
        /// </summary>
        /// <param name="listeStyle"></param>
        public void setListeStyle(List<String> listeStyle)
        {
            int y = 15;
            int x = 15;
            //On crée un radiobouton par style
            foreach(String nomStyle in listeStyle)
            {
                System.Windows.Controls.Button button = new System.Windows.Controls.Button();
                if (x == 15)
                {
                    //Si le bouton est le premier de la ligne
                    button.Margin = new Thickness(x, y, 230, (this.gridButton.Height - y - 35));
                    x = 230;
                }
                else
                {
                    //Si il est le deuxième
                    button.Margin = new Thickness(x, y, 15, (this.gridButton.Height - y - 35));
                    x = 15;
                    y = y + 50;
                }
                button.Content = nomStyle;
                button.Click += Button_Click;
                button.FontFamily = new System.Windows.Media.FontFamily("Arial Rounded MT Bold");
                this.gridButton.Children.Add(button);
                
            }

            listeButtons = this.gridButton.Children.OfType<System.Windows.Controls.Button>();
        }

      
        /// <summary>
        /// Récupère les informations d'un bouton checké et ferme la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Initialise la variable style avec le style du bouton correspondants
            style = ((sender as Button).Content as String);
            // Réactive la fenêtre principale
            traiteReq.activeFenetre(true);
            //Ferme la fenêtre des style
            this.Close();
        }
       
        public String getStyle()
        {
            return style;
        }

    }
}
