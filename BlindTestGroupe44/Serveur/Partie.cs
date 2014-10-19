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
        }
        public void attendPartie()
        {
            if (lj.Count > 0)
            {
                nouvelleManche();
                Console.WriteLine("Je lance " + style);
                Thread.Sleep(10000);
                Console.WriteLine("Je termine " + style);
                attendPartie();
            }
        }

        /*
         * Envoi les scores à tous le monde
         * puis la liste de musique
         */
        public void nouvelleManche()
        {
            envoiScores();
            envoiMusique();
        }
        public void envoiATous(String message)
        {
            foreach(Joueur j in lj)
            {
                Console.WriteLine("J'envoi à " + j.getName());
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

        private void envoi(String message, NetworkStream clientStream)
        {
            Console.WriteLine("Envoi : " + message);
            byte[] buffer = encodeur.GetBytes(message);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }
        public void addJoueur(Joueur j)
        {
            lj.Add(j);
            //Si le serveur était vide et que un premier utilisateur se connecte
            if(lj.Count == 1)
            {
                //on peux lancer la diffusion des chansons
                attendPartie();
            }
            else // Si il y'a des joueurs, on lui envoi les chansons en cours
            {
                envoi(Requete.musique(gm.listeChansons(j.getNbChoix())),
                    j.getStream());
            }
        }
        public String getChanson()
        {
            return chansonPrecedente;
        }
        
        public void envoiScores()
        {
            //Faire une commande dans le fichier requete
            //retirer les utilisateurs proprement
            String res = "INFO?SCORES";
            foreach (Joueur j2 in lj)
            {
                res += "?" + j2.getName() + "&" + j2.getScore();
            }
            envoiATous(res);
        }
        public void envoiMusique()
        {
            //On stocke la chanson qui était à trouver pour pouvoir 
            chansonPrecedente = gm.getChanson();

            //On remélange les chansons
            gm.melange();
            foreach(Joueur j in lj)
            {
                //Et on les envois à tous les joueurs
                try 
                {
                    List<String> chansons = gm.listeChansons(j.getNbChoix());
                    envoi(Requete.musique(chansons), j.getStream());
                }
                catch
                {
                    //Si le joueur n'est plus en lien avec le serveur, on l'enléve de la liste
                    lj.Remove(j);
                }
            }
        }

        public String getStyle()
        {
            return style;
        }

        public GestionMusique getGestionMusique()
        { 
            return gm;
        }
    }

    
}
