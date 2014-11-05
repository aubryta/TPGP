using BlindTestGroupe44.Main;
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

namespace BlindTestGroupe44.ClientLigne
{
    /// <summary>
    /// Logique d'interaction pour FenetreNom.xaml
    /// </summary>
    public partial class FenetreNom : Window
    {

        private String name = null;
        public FenetreNom()
        {
            InitializeComponent();
           
        }

        /// <summary>
        /// Lorsque le pseudo est déjà utilisé, on affiche une popup pour le signaler
        /// </summary>
        public void pseudoExistant()
        {
            PopUp popup = new PopUp();
            popup.setErreur("Ce pseudo existe déjà");
            popup.ShowDialog();

        }
       
        /// <summary>
        /// Lorsque le bouton valider est cliqué, si le pseudo à la bonne taille
        /// on valide et on quitte la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            name = champPseudo.Text;
            if(name.Length >= 10 || name.Length <=0)
            {
                PopUp popup = new PopUp();
                popup.setErreur("Le pseudo ne fait pas la bonne taille");
                popup.ShowDialog();
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// Retourne le pseudo qui a été validé
        /// </summary>
        /// <returns>Le nom à retourner</returns>
        public String getName()
        {
            return name;
        }

        private void champPseudo_GotFocus(object sender, RoutedEventArgs e)
        {
            champPseudo.Text = "";
        }
        /// <summary>
        /// Cette fonction permet de valider
        /// le pseudo via la touche entrée plutot
        /// que de cliquer sur valider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keyEnterDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                name = champPseudo.Text;
                if (name.Length >= 10 || name.Length <= 0)
                {
                    PopUp pu = new PopUp();
                    pu.setErreur("Le pseudo ne fait pas la bonne taille");
                    pu.ShowDialog();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }




    }
}
