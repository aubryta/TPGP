using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Serveur
{
    class Partie
    {

        private ASCIIEncoding encodeur = new ASCIIEncoding();
        private List<Joueur> lj = new List<Joueur>();
        private String chansonPrecedente = "";
        private String style = "";
        private GestionMusique gm = null;

        private Boolean partieFinie = false;
        private int cptManche = 0;
        /// <summary>
        /// Constructeur, initialise la classe Partie
        /// </summary>
        /// <param name="style">le style qui correspond à cette classe</param>
        public Partie(String style)
        {
            gm = new GestionMusique();
            this.style = style;
            gm.setStyle(style);
            gm.chercheChansons();
            gm.setNbChoixMax(6);
        }

        /// <summary>
        /// Gere les manches, la fin d'une partie, les envois d'informations aux joueurs
        /// </summary>
        public void runGame()
        {
            if (lj.Count > 0)
            {
                //Si la partie est finie
                if (cptManche >= 3)
                {
                    finDePartie();
                }
                else
                {
                    cptManche++;
                    nouvelleManche();
                    Thread.Sleep(5000);
                    runGame();
                }
            }
        }

        /// <summary>
        /// Envoi à tous les joueurs, les scores, 
        /// puis la liste de musique
        /// </summary>
        public void nouvelleManche()
        {
            envoiMusique();
        }

        /// <summary>
        /// Envoi a tous les joueurs un message
        /// </summary>
        /// <param name="message">message à envoyer</param>
        public void envoiATous(String message)
        {
            foreach (Joueur j in lj)
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

        /// <summary>
        /// Envoi un message au joueur via son stream
        /// </summary>
        /// <param name="message">message a envoyer</param>
        /// <param name="clientStream">stream sur lequel est connnecté le client</param>
        private void envoi(String message, NetworkStream clientStream)
        {
            Console.WriteLine("Envoi : " + message);
            byte[] buffer = encodeur.GetBytes(message);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        /// <summary>
        /// Ajoute un joueur à la liste et commence une partie si c'est le premier joueur
        /// </summary>
        /// <param name="j">joueur à ajouter</param>
        public void addJoueur(Joueur j)
        {
            lj.Add(j);
            //Si le serveur était vide et que un premier utilisateur se connecte
            if (lj.Count == 1)
            {
                //Si c'est le premier joueur, la manche doit être remise à 0
                cptManche = 0;
                partieFinie = false;
                //on peux lancer la diffusion des chansons
                Thread th = new Thread(runGame);
                th.Start();
            }
            else // Si il y'a des joueurs, on lui envoi les chansons en cours
            {
                if (partieFinie)
                {
                    envoiATous(Requete.infoPartieFinie(lj));
                }
                else
                {
                    envoi(Requete.musique(gm.listeChansons(j.getNbChoix())),
                        j.getStream());
                    envoi(Requete.infoChanson(gm.getUrlChanson()), j.getStream());
                }
            }
            //Dans tous les cas on initialise les scores 
            envoiScores();
        }

        /// <summary>
        /// Retire le joueur j de la liste des joueurs si ce n'est pas déjà fait
        /// </summary>
        /// <param name="j">joueur a retirer</param>
        public void removeJoueur(Joueur j)
        {
            try
            {
                lj.Remove(j);
            }
            catch
            {
                Console.WriteLine("Erreur : Le joueur " + j.getName() + " n'existe déjà plus");
            }
        }

        /// <summary>
        /// Retourne la chanson de la manche précédente
        /// </summary>
        /// <returns>chanson précédente</returns>
        public String getChanson()
        {
            return chansonPrecedente;
        }

        /// <summary>
        /// Envoi à tous les joueurs, tous les scores
        /// </summary>
        public void envoiScores()
        {
            envoiATous(Requete.infoScores(lj));
        }

        /// <summary>
        /// Melange et envoi un panel de musique à chaque joueur, avec
        /// plus ou moins de chansons par joueurs.
        /// </summary>
        public void envoiMusique()
        {
            //On stocke la chanson qui était à trouver pour pouvoir 
            chansonPrecedente = gm.getChanson();

            //On remélange les chansons
            gm.melange();
            List<Joueur> ljTmp = new List<Joueur>();
            foreach (Joueur j in lj)
            {
                //Et on les envois à tous les joueurs
                try
                {
                    List<String> chansons = gm.listeChansons(j.getNbChoix());
                    envoi(Requete.infoChanson(gm.getUrlChanson()), j.getStream());
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

        /// <summary>
        /// Renvoi le style de musique de cette partie
        /// </summary>
        /// <returns>style de musique</returns>
        public String getStyle()
        {
            return style;
        }

        /// <summary>
        /// Retourne le gestionnaire de musique qui lui est associé
        /// </summary>
        /// <returns>gestionnaire de musique d'une partie</returns>
        public GestionMusique getGestionMusique()
        {
            return gm;
        }

        /// <summary>
        /// Mets tous les scores des joueurs à zéro
        /// </summary>
        private void resetScores()
        {
            foreach (Joueur j in lj)
            {
                j.setScore(0);
            }
        }

        /// <summary>
        /// Si le pseudo en paramètre existe déjà parmis la liste de joueur, renvoi faux sinon vrai
        /// </summary>
        /// <param name="pseudo">le pseudo a vérifier</param>
        /// <returns>la validité du pseudo</returns>
        public Boolean existePseudo(String pseudo)
        {
            foreach (Joueur j in lj)
            {
                if (!j.getName().Equals("")) //En gros son pseudo qui n'est pas encore initialisé
                {
                    if (j.getName().Equals(pseudo))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// A la fin d'une partie, affiche pendant 5 secondes les scores et réinitialise tout
        /// </summary>
        public void finDePartie()
        {
            chansonPrecedente = gm.getChanson();
            envoiATous(Requete.finDePartie());
            partieFinie = true;
            //On attend de recevoir tous les scores
            Thread.Sleep(2000);
            //On envoi le récapitulatif des scores
            envoiATous(Requete.infoPartieFinie(lj));
            //On écrit éventuellement les meilleurs scores dans le fichier xml correspondant
            ecritScore();
            resetScores();
            Thread.Sleep(7500);
            //Après avoir attendu 7.5 secondes, on recommence une partie
            cptManche = 0;
            partieFinie = false;
            envoiATous(Requete.nouvellePartie());
            envoiScores();
            runGame();
        }

        /// <summary>
        /// Regarde parmis la liste des joueurs si il existe un joueur pouvant faire partie des meilleurs scores et l'écrit si oui.
        /// </summary>
        public void ecritScore()
        {
            //On ajoute tous les joueurs à une liste
            List<JoueurSerialisable> ljs = new List<JoueurSerialisable>();
            for (int i = 0; i < lj.Count; i++)
            {
                JoueurSerialisable js = new JoueurSerialisable
                {
                    nom = lj[i].getName(),
                    score = lj[i].getScore()
                };
                ljs.Add(js);
            }

            //On récupére les anciens joueurs et on les ajoutent à cette même liste
            JoueurSerialisable[] tabjs = readBestScores();
            for (int i = 0; i < tabjs.Length; i++)
            {
                ljs.Add(tabjs[i]);
            }

            ljs = bestJoueur(ljs);

            //Puis on les écrit dans le fichier à la fin
            XmlSerializer xs = new XmlSerializer(typeof(List<JoueurSerialisable>));
            using (StreamWriter wr = new StreamWriter("bestScore" + style + ".xml"))
            {
                xs.Serialize(wr, ljs);
            }
        }

        /// <summary>
        /// Lit et renvoie la liste des meilleurs joueurs de ce style la
        /// </summary>
        /// <returns>Liste des meilleurs joueurs de ce style</returns>
        public JoueurSerialisable[] readBestScores()
        {
            JoueurSerialisable[] ljs = {} ;
            if (File.Exists("bestScore" + style + ".xml"))
            {
                XmlSerializer xRead = new XmlSerializer(typeof(JoueurSerialisable[]));
                using (StreamReader rd = new StreamReader("bestScore" + style + ".xml"))
                {
                    ljs = xRead.Deserialize(rd) as JoueurSerialisable[];
                }
                return ljs;
            }
            return ljs;
        }

        /// <summary>
        /// Récupére les meilleurs joueurs d'une liste et les renvois
        /// </summary>
        /// <param name="ljs">La liste à analyser</param>
        /// <returns>Les meilleurs joueurs</returns>
        public List<JoueurSerialisable> bestJoueur(List<JoueurSerialisable> ljs)
        {
            //On calcul le minimum entre 10 et la taille de la liste
            int tailleAffiche = 10;
            if (ljs.Count < 10)
                tailleAffiche = ljs.Count;

            //On tri et on ajoute les 10 premiers à une nouvelle liste
            List<JoueurSerialisable> newljs = new List<JoueurSerialisable>();
            for (int i = 0; i < tailleAffiche; i++)
            {
                int best = -1;
                JoueurSerialisable js = null;
                for (int j = 0; j < ljs.Count; j++)
                {
                    if (ljs[j].score > best)
                    {
                        js = ljs[j];
                        best = ljs[j].score;
                    }

                }
                newljs.Add(js); //On vient donc de récuperer le meilleur joueur de la liste
                ljs.Remove(js);
            }
            return newljs;
        }
    }
}
