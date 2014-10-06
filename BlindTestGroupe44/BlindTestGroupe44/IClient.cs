using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace BlindTestGroupe44
{
    interface IClient
    {
        void initialiseTest(object sender, RoutedEventArgs e);
        void validerBoutonClick(object sender, RoutedEventArgs e);
        void commencerBoutonClick(object sender, RoutedEventArgs e);
        void runGame();
        void changerVolume(double d);
        void resetScore();

        // en local : bibli de l'utilisateur
        // serveur : style de musique
        void choisirBibliClick(object sender, RoutedEventArgs e);
    }
}
