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
        public static String options(int nbchoix, int incr)
        {
            return "OPTIONS?" + nbchoix + "?" + incr;
        }
    }
}
