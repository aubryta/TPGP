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
        private IClient client;
      
        /*
         * Truc à faire : 
         * 1 : vérifier que le nombre de chanson du répertoire est supérieur ou égale au nombre de choix de la dificulté
         * 2 : vérifier que le score est bien incrémenter (possible bugg (coquille))
         *      -> le score s'incrémente bien la première fois mais aps après il me semble (tu réinisialise pas le choixcorrect ?)
         * 3 : Changer la chanson précédente de ligne (les titres long peuvent cacher le score)
         * 4 : éviter les doublons lors de la création
         * 
         * 
         */ 
        public MainWindow()
        {
            InitializeComponent();
        }

       

        private void cliqueLocal(object sender, RoutedEventArgs e)
        {
            client = new ClientLocal(this);
            
            gridDebut.Visibility = Visibility.Hidden;
            grid1.Visibility = Visibility.Visible;
        }

        private void cliqueServeur(object sender, RoutedEventArgs e)
        {
            client = new ClientServ(this);

            gridDebut.Visibility = Visibility.Hidden;
            grid1.Visibility = Visibility.Visible;
        }
        public void commencerBoutonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            client.commencerBoutonClick(sender, e) ;
        }
        void validerBoutonClick(object sender, RoutedEventArgs e) 
        {
            client.validerBoutonClick(sender, e);
        }
        public void choisirBibliClick(object sender, RoutedEventArgs e)
        {
            client.choisirBibliClick(sender, e);
        }
        

    }
}
