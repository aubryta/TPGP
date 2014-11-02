using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serveur
{
    class Requete
    {
        /// <summary>
        /// Envoi un message d'erreur au client
        /// </summary>
        /// <param name="err">le message d'erreur</param>
        /// <returns>commande correspondante</returns>
        public static String erreur(String err)
        {
            return "ERREUR?" + err;
        }

        /// <summary>
        /// Indique au client que le pseudo est déjà pris
        /// </summary>
        /// <returns>commande correspondante</returns>
        public static String infoPseudoIncorrect()
        {
            return "INFO?PSEUDOINCORRECT";
        }

        /// <summary>
        /// Envoi la liste des chansons pour la manche
        /// </summary>
        /// <param name="listeChansons">liste des chansons</param>
        /// <returns>commande correspondante</returns>
        public static String musique(List<String> listeChansons)
        {
            String res = "MUSIQUE";
            foreach(String chanson in listeChansons)
            {
                res += "?" + chanson;
            }
            return res;
        }

        /// <summary>
        /// Envoi le nombre de choix correspondant à la difficulté choisis par le client
        /// </summary>
        /// <param name="nbchoix">le nombre de choix</param>
        /// <returns>commande correspondante</returns>
        public static String options(int nbchoix)
        {
            return "OPTIONS?" + nbchoix;
        }

        /// <summary>
        /// Envoi la liste des styles disponible sur le serveur
        /// </summary>
        /// <param name="listeStyle">liste des styles</param>
        /// <returns>commande correspondante</returns>
        public static String choixStyle(List<String> listeStyle)
        {
            String res = "CHOIXSTYLE";
            foreach (String styl in listeStyle)
                res += "?" + styl;
            return res;        
        }

        /// <summary>
        /// Indique au serveur qu'il avait trouvé la bonne chanson et lui reconfirme
        /// </summary>
        /// <param name="chanson">la chanson à trouver</param>
        /// <returns>commande correspondante</returns>
        public static String infoBonneChanson(String chanson)
        {
            return "INFO?BONNECHANSON?" + chanson;
        }

        /// <summary>
        /// Indique au serveur que sa proposition de chanson était mauvaise et lui donne la bonne chanson
        /// </summary>
        /// <param name="chanson">la bonne chanson</param>
        /// <returns>commande correspondante</returns>
        public static String infoMauvaiseChanson(String chanson)
        {
            return "INFO?MAUVAISECHANSON?" + chanson;
        }

        /// <summary>
        /// Envoi à tous les joueurs tous les scores
        /// </summary>
        /// <param name="lj">Liste des joueurs</param>
        /// <returns>commande correspondante</returns>
        public static String infoScores(List<Joueur> lj)
        {
            String res = "INFO?SCORES";
            foreach (Joueur j in lj)
            {
                res += "?" + j.getName() + "&" + j.getScore();
            }
            return res;
        }

        /// <summary>
        /// Envoi la chanson à lire dans le player du client
        /// </summary>
        /// <param name="urlChanson">url de la chanson</param>
        /// <returns>commande correspondante</returns>
        public static String infoChanson(String urlChanson)
        {
            return "INFO?CHANSON?" + urlChanson;
        }

        /// <summary>
        /// Indique au client qu'une partie est finie et lui envoi tous les scores
        /// </summary>
        /// <param name="lj">liste des joueurs</param>
        /// <returns>commande correspondante</returns>
        public static String infoPartieFinie(List<Joueur> lj)
        {
            String res = "INFO?PARTIEFINIE";
            foreach(Joueur j in lj)
            {
                res += "?" + j.getName() + "=" + j.getScore();
            }
            return res;
        }

        /// <summary>
        /// Indique au client qu'une partie va commencer
        /// </summary>
        /// <returns>commande correspondante</returns>
        public static String nouvellePartie()
        {
            return "INFO?NOUVELLEPARTIE?";
        }

        /// <summary>
        /// Envoie tous les meilleurs scores de toutes les parties du serveur
        /// </summary>
        /// <param name="serv">le serveur</param>
        /// <returns>les meilleurs parties</returns>
        public static String bestScores(Serveur serv)
        {
            List<Partie> lp = serv.getParties();
            String res = "BESTSCORES";
            for (int i = 0; i < lp.Count ;i++ )
            {
                res += unePartie(lp[i]);
            }
            if (res.Equals("BESTSCORES")) //si aucune partie n'a était jouée :
                return res + "?KO";
            return res;
                
        }

        /// <summary>
        /// Envoi tous les meilleurs scores d'une partie
        /// </summary>
        /// <param name="p">la partie</param>
        /// <returns>les meilleurs scores</returns>
        public static String unePartie(Partie p)
        {
            String res = "";
            //Si le fichier existe
            if (File.Exists("bestScore" + p.getStyle() + ".xml"))
            {
                res = "?style&" + p.getStyle();
                JoueurSerialisable[] ljs = p.readBestScores();
                for (int i = 0; i < ljs.Length; i++)
                {
                    res += "?" + ljs[i].nom + "&" + ljs[i].score;
                }
            }
            return res; ;
        }
    }
}
