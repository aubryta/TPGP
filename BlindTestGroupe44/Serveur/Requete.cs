using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serveur
{
    class Requete
    {
        public static String erreur(String err)
        {
            return "ERREUR?" + err;
        }
        public static String infoPseudoIncorrect()
        {
            return "INFO?PSEUDOINCORRECT";
        }
        public static String musique(List<String> listeChansons)
        {
            String res = "MUSIQUE";
            foreach(String chanson in listeChansons)
            {
                res += "?" + chanson;
            }
            return res;
        }
        public static String options(int nbchoix)
        {
            return "OPTIONS?" + nbchoix;
        }
        public static String choixStyle(List<String> listeStyle)
        {
            String res = "CHOIXSTYLE";
            foreach (String styl in listeStyle)
                res += "?" + styl;
            return res;        
        }
        public static String infoBonneChanson(String chanson)
        {
            return "INFO?BONNECHANSON?" + chanson;
        }
        public static String infoMauvaiseChanson(String chanson)
        {
            return "INFO?MAUVAISECHANSON?" + chanson;
        }

        public static String infoScores(List<Joueur> lj)
        {
            String res = "INFO?SCORES";
            foreach (Joueur j in lj)
            {
                res += "?" + j.getName() + "&" + j.getScore();
            }
            return res;
        }

        public static String infoChanson(String urlChanson)
        {
            return "INFO?CHANSON?" + urlChanson;
        }
    }
}
