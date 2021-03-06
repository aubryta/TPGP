﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlindTestGroupe44.ClientLigne
{
    /// <summary>
    /// Classe servant a la création de
    /// Requete a envoyé au serveur selon
    /// la norme choisie
    /// </summary>
    class Requete
    {
        public static String infoStyle(String style)
        {
            return "INFO?STYLE?" + style;
        }
        public static String proposeChanson(String chanson)
        {
            return "CHANSON?" + chanson;
        }
        public static String infoDifficulte(String dif)
        {
            return "INFO?DIFFICULTE?" + dif;
        }
        public static String start()
        {
            return "INFO?START";
        }
        public static String demandeStyle()
        {
            return "CHOIXSTYLE?";
        }
        public static String infoName(String name)
        {
            return "INFO?NAME?" + name;
        }
        public static String bestScores()
        {
            return "BESTSCORE?";
        }
        public static String deconnexion()
        {
            return "DECONNEXION?";
        }
    }
}
