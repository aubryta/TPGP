using BlindTestGroupe44.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BlindTestGroupe44.ClientLigne
{
    public class TraiteRequete
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
            int y = 10;
            foreach (String chanson in chansons)
            {
                System.Windows.Controls.RadioButton r = new System.Windows.Controls.RadioButton();
                r.Margin = new Thickness(10, y, 0, 0);
                r.Content = chanson;
                r.FontFamily = new System.Windows.Media.FontFamily("Arial Rounded MT Bold");
                client.getWind().gridButton.Children.Add(r);
                y = y + 40;
            }
            //on finit par ajouter tous les boutons à la liste 
            listeRadioButtons = client.getWind().gridButton.Children.OfType<System.Windows.Controls.RadioButton>();
            client.setRadios(listeRadioButtons);
        }
        public void chansonPrecedente(String chanson)
        {
            client.getWind().chansonPrecedente.Content = "Chanson précédente : " + chanson;
        }
        public void creationRadioButtons(List<String> chansons)
        {

            if (listeRadioButtons != null) // Si une liste de boutons existe
            {
                editRadioBoutton(chansons);
            }
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
            FenetreStyle fs = new FenetreStyle(this);
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
        public void activeFenetre(Boolean active)
        {
            Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() => actFenetreThread(active)));
        }
        private void actFenetreThread(Boolean active)
        {
            wind.mainGrid.IsEnabled = active;
        }
        public void pseudo()
        {
            FenetreNom fNom = new FenetreNom();
            activeFenetre(false);
            fNom.ShowDialog();
            client.envoi(Requete.infoName(fNom.getName()));
            activeFenetre(true);

        }
        public void pseudoErreur()
        {
            FenetreNom fNom = new FenetreNom();
            activeFenetre(false);
            fNom.pseudoExistant();
            fNom.ShowDialog();
            client.envoi(Requete.infoName(fNom.getName()));
            activeFenetre(true);
        }
        public void infoScores(String[] tabMessage)
        {
            int y = 10;
            wind.nomScore.Content = "";
            wind.valeurScore.Content = "";
            for(int i = 2; i < tabMessage.Length; i++)
            {
                String score = tabMessage[i];
                String[] scoreTab = score.Split('&');
                wind.nomScore.Content += scoreTab[0] + "\n";
                wind.valeurScore.Content += scoreTab[1] + "\n";
            }
        }
        public void envoiReponse()
        {
            if (listeRadioButtons != null)
            {
                foreach(RadioButton r in listeRadioButtons)
                {
                    if(r.IsChecked == true)
                    {
                        client.envoi(Requete.proposeChanson(r.Content as String)); 
                    }
                }
            }
            else
                Console.WriteLine("COUCOU JE PEUX PAS ENVOYER MON SCORE");
        }
    }
}
