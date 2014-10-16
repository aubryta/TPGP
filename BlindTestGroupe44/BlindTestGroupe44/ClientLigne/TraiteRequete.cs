using BlindTestGroupe44.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BlindTestGroupe44.ClientLigne
{
    class TraiteRequete
    {
        private ASCIIEncoding encodeur = new ASCIIEncoding();
        private ClientServ client = null;
        private int incrPoints = 0;
        private int nbRadios = 0;
        private MainWindow wind = null;

        IEnumerable<System.Windows.Controls.RadioButton> listeRadioButtons = null;

        public void setTraite(ClientServ cs, MainWindow wind, IEnumerable<System.Windows.Controls.RadioButton> listeRadioButtons)
        {
            this.client = cs;
            this.wind = wind;
            this.listeRadioButtons = listeRadioButtons;
        }
        private void creeRadioBouton(List<String> chansons)
        {
            int y = 256;
            foreach (String chanson in chansons)
            {
                System.Windows.Controls.RadioButton r = new System.Windows.Controls.RadioButton();
                r.Margin = new Thickness(256, y, 0, 0);
                r.Content = chanson;
                r.FontFamily = new System.Windows.Media.FontFamily("Arial Rounded MT Bold");
                client.getWind().grid2.Children.Add(r);
                y = y + 40;
            }
            System.Windows.Controls.Button valider = new System.Windows.Controls.Button();
            valider.Click += client.validerBoutonClick;
            valider.Content = "Valider";
            valider.Width = 164;
            valider.Height = 61;
            valider.Margin = new Thickness(256, y, 0, 0);
            valider.FontFamily = new System.Windows.Media.FontFamily("Arial Rounded MT Bold");
            valider.FontSize = 16;
            valider.Visibility = Visibility.Visible;
            valider.IsEnabled = true;

            client.getWind().grid2.Children.Add(valider);

            //on finit par ajouter tous les boutons à la liste 
            listeRadioButtons = client.getWind().grid2.Children.OfType<System.Windows.Controls.RadioButton>();
            client.setRadios(listeRadioButtons);
        }
        public void chansonPrecendente(String chanson)
        {
            client.getWind().chansonPrecedente.Content = "Chanson précédente : " + chanson;
        }
        public void creationRadioButtons(List<String> chansons)
        {

            if (listeRadioButtons != null) // Si une liste de boutons existe
                editRadioBoutton(chansons);
            else
            { 
                //Si la liste n'existe pas, on l'a créé, ainsi que le bouton valider
                //et on affiche le panel lié
                wind.grid1.Visibility = Visibility.Hidden;
                wind.grid2.Visibility = Visibility.Visible;
                creeRadioBouton(chansons);
            }
        }
        private void editRadioBoutton(List<String> chansons)
        {
            for (int i = 0; i < nbRadios; i++)
            {
                listeRadioButtons.ElementAt(i).IsChecked = false;
                listeRadioButtons.ElementAt(i).Content = chansons.ElementAt(i);
            }
            client.setRadios(listeRadioButtons);
        }
        public void changeScore()
        {   //Met à jour le label des points et incrémente le score
            client.setScore(client.getScore() + incrPoints);
            client.getWind().scoreLabel.Content = "Score : " + client.getScore();
        }
        public void initialisationOptions(int nbRadios, int incr)
        {
            this.incrPoints = incr;
            this.nbRadios = nbRadios;
        }
        public void fenetreStyle(List<String> listRadios)
        {
            FenetreStyle fs = new FenetreStyle(wind);
            fs.setListeStyle(listRadios);
            
            fs.ShowDialog();
            client.envoi(Requete.infoStyle(fs.getStyle()));
        }
        public void erreur(String err)
        {
            Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() => lanceFenetre(err, false)));
        }
        public void message(String err)
        {
            Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() => lanceFenetre(err, true)));
        }
        private void lanceFenetre(String mess, Boolean estMessage)
        {
            PopUp pu = new PopUp();
            if (estMessage)
                pu.setMessage(mess);
            else
                pu.setErreur(mess);
            pu.ShowDialog();
        }

    }
}
