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
         * 0 : COMMENTER
         * 1 : vérifier que le nombre de chanson du répertoire est supérieur ou égale au nombre de choix de la dificulté
         * 2 : éviter les doublons lors de la création
         * 3 : fermer correctement les sockets pour les fermetures
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
        }
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
            client.choisirBibliClick(sender, e);
        }


        private void newGameItem(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
            System.Windows.Application.Current.Shutdown();
            
        }        

        private void volumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // ... Get Slider reference.
            var slider = sender as Slider;
            // ... Get Value.
            double value = slider.Value;
            client.changerVolume(value);
        }

        private void resetScore(object sender, RoutedEventArgs e)
        {
            client.resetScore();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (client != null)
                client.quitteAppli();
            else
                System.Environment.Exit(0);
        }

    }
}
