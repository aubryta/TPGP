using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serveur
{
    class Partie
    {

        private ASCIIEncoding encodeur = new ASCIIEncoding();
        private List<Joueur> lj = new List<Joueur>();
        private String chansonPrecedente = "";
        private String style = "";
        private GestionMusique gm = new GestionMusique();

        public Partie(String style)
        {
            this.style = style;
            gm.setStyle(style);
            gm.chercheChansons();
            gm.setNbChoixMax(6);
        }

        //Gere les manches, la fin d'une partie, les envois d'informations aux joueurs
        public void runGame()
        {
            if (lj.Count > 0)
            {
                nouvelleManche();
                Thread.Sleep(10000);
                runGame();
            }
        }
        /*
         * Envoi les scores à tous le monde
         * puis la liste de musique
         */
        public void nouvelleManche()
        {
            envoiMusique();
        }

        //Envoi a tous les joueurs un stream
        public void envoiATous(String message)
        {
            foreach(Joueur j in lj)
            {
                try
                {
                    if (j.getStream() != null)
                        envoi(message, j.getStream());
                }
                catch 
                {
                    lj.Remove(j);
                }
            }
        }

        //Envoi un message au joueur via son stream
        private void envoi(String message, NetworkStream clientStream)
        {
            Console.WriteLine("Envoi : " + message);
            byte[] buffer = encodeur.GetBytes(message);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        //Ajoute un joueur à la liste et commence une partie si c'est le premier joueur
        public void addJoueur(Joueur j)
        {
            lj.Add(j);
            //Si le serveur était vide et que un premier utilisateur se connecte
            if(lj.Count == 1)
            {
                //on peux lancer la diffusion des chansons
                Thread th = new Thread(runGame);
                th.Start();
            }
            else // Si il y'a des joueurs, on lui envoi les chansons en cours
            {
                envoi(Requete.musique(gm.listeChansons(j.getNbChoix())),
                    j.getStream());
                envoi(Requete.infoChanson(gm.getUrlChanson(gm.getChanson())), j.getStream());
            }
            //Dans tous les cas on initialise les scores 
            envoiScores();
        }

        //Retire le joueur j de la liste des joueurs si ce n'est pas déjà fait
        public void removeJoueur(Joueur j)
        {
            try
            {
                lj.Remove(j);
            }
            catch
            {
                Console.WriteLine("Erreur, le joueur " + j.getName() + " n'existe déjà plus");
            }
        }

        //Retourne la chanson précédente
        public String getChanson()
        {
            return chansonPrecedente;
        }
        //Envoi à tous les joueurs, tous les scores
        public void envoiScores()
        {
            envoiATous(Requete.infoScores(lj));
        }
        public void envoiMusique()
        {
            //On stocke la chanson qui était à trouver pour pouvoir 
            chansonPrecedente = gm.getChanson();

            //On remélange les chansons
            gm.melange();
            List<Joueur> ljTmp = new List<Joueur>();
            foreach(Joueur j in lj)
            {
                //Et on les envois à tous les joueurs
                try 
                {
                    List<String> chansons = gm.listeChansons(j.getNbChoix());
                    envoi(Requete.infoChanson(gm.getUrlChanson(gm.getChanson())),j.getStream());
                    envoi(Requete.musique(chansons), j.getStream());
                }
                catch
                {
                    //Si le joueur n'est plus en lien avec le serveur, on l'enléve de la liste
                    //joueurASuprr.Add(j.getName());
                    ljTmp.Add(j);
                }
            }

            foreach (Joueur j in ljTmp)
            {
                lj.Remove(j);
            }
        }

        //Renvoi le style de musique de cette partie
        public String getStyle()
        {
            return style;
        }

        //Retourne le gestionnaire de musique qui lui est associé
        public GestionMusique getGestionMusique()
        { 
            return gm;
        }
    }
}
