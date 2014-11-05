using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace BlindTestGroupe44
{
    /// <summary>
    /// Interface des clients
    /// correspondant aux fonctions qu'ont en commun
    /// le client local et le client en ligne
    /// </summary>
    interface IClient
    {       
        void initialiseJeu(object sender, RoutedEventArgs e);
        void validerBoutonClick(object sender, RoutedEventArgs e);
        void commencerBoutonClick(object sender, RoutedEventArgs e);
        void runGame();
        void changerVolume(double d);
        void resetScore();
        void quitteAppli();
        // en local : bibli de l'utilisateur
        // serveur : style de musique
        void choisirDossierMusique(object sender, RoutedEventArgs e);
    }
}
