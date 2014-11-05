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
    /// Fenetre de départ de l'application
    /// </summary>
    public partial class MainWindow : Window
    {
        private IClient client;    
 
        public MainWindow()
        {
            InitializeComponent();
        }

       
        /// <summary>
        /// mise a jour des fenetres si l'utilisateur veut jouer en local
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cliqueLocal(object sender, RoutedEventArgs e)
        {
            client = new ClientLocal(this);
            BarreDeMenu.Visibility = Visibility.Visible;
            gridDebut.Visibility = Visibility.Hidden;
            grid1.Visibility = Visibility.Visible;
            imageFondNom.Visibility = Visibility.Hidden;
            imageFond.Visibility = Visibility.Visible;            
            
        }

        /// <summary>
        /// Creation d'un clientServ si l'utilisateur veut jouer en ligne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cliqueServeur(object sender, RoutedEventArgs e)
        {
            client = new ClientServ(this);
        }

        /// <summary>
        /// initialise la barre de volume et commence la partie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void commencerBoutonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            volumeSlideBar.Value = 10;
            client.commencerBoutonClick(sender, e) ;
            
        }
        void validerBoutonClick(object sender, RoutedEventArgs e) 
        {
            client.validerBoutonClick(sender, e);
        }
        public void choisirBibliClick(object sender, RoutedEventArgs e)
        {
            client.choisirDossierMusique(sender, e);
        }
        
        /// <summary>
        /// Lancement d'une nouvelle partie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newGameItem(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
            System.Windows.Application.Current.Shutdown();
        }        

        /// <summary>
        /// Changement de volume en fonction du slideBar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void volumeChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // ... Get Slider reference.
            var sliderVolume = sender as Slider;
            // ... Get Value.
            double value = sliderVolume.Value;
            client.changerVolume(value);
        }

        /// <summary>
        /// Remet le score a 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetScore(object sender, RoutedEventArgs e)
        {
            client.resetScore();
        }

        /// <summary>
        /// quitte l'application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (client != null)
                client.quitteAppli();
            else
                System.Environment.Exit(0);
        }      

        /// <summary>
        /// permet de valider la réponse par le bouton entrée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keyDownFunctiond(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                client.validerBoutonClick(sender, e);
        }

        /// <summary>
        /// Dans le cas d'une partie en ligne
        /// On peut demander les meilleurs scores au serveur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void askBestScore(object sender, RoutedEventArgs e)
        {
            if(client.GetType()==typeof(ClientServ))
                ((ClientServ)client).askBestScores();
        }


      
      

       

    }
}
