﻿using BlindTestGroupe44.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        //Initialise les attributs de traiteRequete
        public void setTraite(ClientServ cs, MainWindow wind, IEnumerable<System.Windows.Controls.RadioButton> listeRadioButtons)
        {
            this.client = cs;
            this.wind = wind;
            this.listeRadioButtons = listeRadioButtons;
        }

        //Crée et initialise une liste de radioBouton
        private void creeRadioBouton(List<String> chansons)
        {
            int y = 10;
            //Pour toutes les chansons, on crée un bouton qui lui correspond
            foreach (String chanson in chansons)
            {
                System.Windows.Controls.RadioButton r = new System.Windows.Controls.RadioButton();
                r.Margin = new Thickness(10, y, 0, 0);
                r.Content = chanson;
                r.FontFamily = new System.Windows.Media.FontFamily("Arial Rounded MT Bold");
                client.getWind().gridButton.Children.Add(r);
                y = y + 40;
            }
            //on finit par ajouter tous les boutons à la fenêtre
            listeRadioButtons = client.getWind().gridButton.Children.OfType<System.Windows.Controls.RadioButton>();
            client.setRadios(listeRadioButtons);
        }

        //On affiche la chanson qui était à trouver le tour d'avant dans le panel correspondant
        // et on coupe la musique
        public void chansonPrecedente(String chanson)
        {
            client.arretMusique();
            client.getWind().chansonPrecedente.Content = "Chanson précédente : " + chanson;
        }

        //Crée ou edite les radiosbouton existant lors de l'affichage d'une nouvelle playlist
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

        //Edite une liste de radioBouton en paramètre
        private void editRadioBoutton(List<String> chansons)
        {
            for (int i = 0; i < nbRadios; i++)
            {
                listeRadioButtons.ElementAt(i).IsChecked = false;
                listeRadioButtons.ElementAt(i).Content = chansons.ElementAt(i);
            }
            client.setRadios(listeRadioButtons);
        }

        //Indique au client le nombre de possibilité qu'il aura pour choisir
        public void initialisationOptions(int nbRadios)
        {
            this.nbRadios = nbRadios;
        }

        //Crée une fenêtre avec la liste des styles disponible
        public void fenetreStyle(List<String> listeStyle)
        {
            FenetreStyle fs = new FenetreStyle(this);
            fs.setListeStyle(listeStyle);
            
            fs.ShowDialog();
            client.envoi(Requete.infoStyle(fs.getStyle()));
        }

        //Affiche une popup d'erreur avec un message err
        public void erreur(String err)
        {
            Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() => lanceFenetre(err, false)));
        }
        
        //Affiche une popup indicative avec le message mess
        public void message(String mess)
        {
            Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() => lanceFenetre(mess, true)));
        }

        //Lance une fenêtre indicative ou erreur (true ou false) avec un message mess
        private void lanceFenetre(String mess, Boolean estMessage)
        {
            PopUp pu = new PopUp();
            if (estMessage)
                pu.setMessage(mess);
            else
                pu.setErreur(mess);
            pu.ShowDialog();
        }

        //appel en tache de fond une fonction qui active ou désactive la fenêtre principale
        public void activeFenetre(Boolean active)
        {
            Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.Background, 
            new Action(() => actFenetreThread(active)));
        }

        //Active ou désactive la fenêtre principale suivant le booléen transmis
        private void actFenetreThread(Boolean active)
        {
            wind.mainGrid.IsEnabled = active;
        }

        //Crée une pop pour définir un pseudo
        public void pseudo()
        {
            FenetreNom fNom = new FenetreNom();
            //On bloque la fenêtre principale en attendant
            activeFenetre(false);
            fNom.ShowDialog();
            //Et on envoie le nom au serveur lorsque le champ est rempli et validé
            client.envoi(Requete.infoName(fNom.getName()));
            client.setName(fNom.getName());
            //On réactive la fenêtre
            activeFenetre(true);

        }

        //Meme fonctionnalité que pseudo() avec en plus une fenêtre qui affiche que le pseudo est utilisé
        //et donc non disponible
        public void pseudoErreur()
        {
            FenetreNom fNom = new FenetreNom();
            activeFenetre(false);
            fNom.pseudoExistant();
            fNom.ShowDialog();
            client.envoi(Requete.infoName(fNom.getName()));
            client.setName(fNom.getName());
            activeFenetre(true);
        }

        //Ecrit dans la grille lié aux scores de tous les joueurs, 
        //Le score plus la valeur0
        public void infoScores(String[] tabMessage)
        {
            wind.nomScore.Content = "";
            wind.valeurScore.Content = "";
            for(int i = 2; i < tabMessage.Length; i++)
            {
                String score = tabMessage[i];
                String[] scoreTab = score.Split('&');
                wind.nomScore.Content += scoreTab[0] + "\n";
                wind.valeurScore.Content += scoreTab[1] + "\n";
                //Si on recoit son propre score, on l'affiche en plus dans un label différent
                if (client.getName().Equals(scoreTab[0]))
                    wind.scoreLabel.Content = "Score : " + scoreTab[1];
            }
        }

        //Envoi la chanson proposer par l'utilisateur
        public void envoiReponse()
        {
            if (listeRadioButtons != null)
            {
                //On regarde parmis tous les radios bouton lequel est coché
                foreach (System.Windows.Controls.RadioButton r in listeRadioButtons)
                {
                    if(r.IsChecked == true)
                    {
                        client.envoi(Requete.proposeChanson(r.Content as String)); 
                    }
                }
            }
        }

        internal void lireChanson(string p)
        {
            client.lireChansonUrl(p);
        }
    }
}
