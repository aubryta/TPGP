using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestURL;

namespace Serveur
{
    class GestionMusique
    {

        private String style = "";
        private String chanson = "";
        private List<String> chansonsRep = new List<string>();
        private GestionURL gm = new GestionURL();

        /*
         * Pour utiliser cette classe, 
         * 1 - il faut d'abord initialiser la liste (chercheChansons)
         * 2 - trouver aléatoirement une chanson et mélanger la liste (melange)
         * 3 - on utilise le nombre de choix voulu (listechanson(nbChoix) : vu que la liste est mélangé à chaque fois, 
         *      les nbChoix premier seront utilisés hors la chanson à trouver
         * 4 - on peux comparer la réponse de l'utilisateur avec la chanson getChanson()
         * 5 - pour recommencer on reprend à l'étape 2
         */ 
        public List<String> choixStyle()
        {
            
            //************
            /*
            List<String> listeStyle = new List<string>();
            DirectoryInfo dir = new DirectoryInfo("Musique");
            DirectoryInfo[] styles = dir.GetDirectories();
            if (styles.Length == 0)
                return null;
            foreach (DirectoryInfo fichier in styles)
            {
                listeStyle.Add(fichier.Name);
            }
            
            return listeStyle;
             */
            return gm.listStyle();
        }

        public String getStyle()
        {
            return style;
        }
        public void setStyle(String style)
        {
            this.style = style;
        }

        // la liste chanson rep, et retire aléatoirement une chanson pour la 
        //placer dans chanson
        public void melange(int nbChoix)
        {
            //Test lors de la première utilisation (il n'y a aucune chanson précédente)
            if (chanson != null)
                chansonsRep.Add(chanson);

            Random rnd = new Random();
            String tmp = "";
            int swapIndex = 0;
            //melange
            for (int i = 0; i < nbChoix; i++)
            {
                tmp = chansonsRep.ElementAt(i);
                swapIndex = rnd.Next(0, chansonsRep.Count - 1);
                chansonsRep[i] = chansonsRep.ElementAt(swapIndex);
                chansonsRep[swapIndex] = tmp;
            }

            //Choisit et retire une chanson au hasard
            int alea = rnd.Next(0, chansonsRep.Count);
            chanson = chansonsRep.ElementAt(alea);
            chansonsRep.RemoveAt(alea);
        }
        
        //Va rechercher dans le répertoire toutes les chansons et les stockent dans "chansonsRep"
        public void chercheChansons()
        {
            
           /* DirectoryInfo dir = new DirectoryInfo("Musique/" + style + "/");
            FileInfo[] listeChansons = dir.GetFiles();
            List<String> listeMusique = new List<string>();
            for (int i = 0; i < listeChansons.Length; i++)
            {
                chansonsRep.Add(listeChansons[i].Name);
            }*/
            chansonsRep = gm.listeChansons(style);
        }



        //Retourne une liste aléatoire de chanson
        //Sauvegarde une chanson parmis cette liste dans "chanson".
        public List<String> listeChansons(int nbChansons)
        {
            if (chansonsRep.Count == 0)
                return null;
            melange(nbChansons);
            List<String> res = new List<string>();
            Random rnd = new Random();
            //on cherche aléatoirement ou cette chanson va aller dans la liste
            int aleaChanson = rnd.Next(0, nbChansons);
            for (int i = 0; i < nbChansons; i ++ )
            {
                if (i == aleaChanson)
                    res.Add(chanson);
                else
                    res.Add(chansonsRep.ElementAt(i));
            }
            return res;
        }
        public String getChanson()
        {
            return chanson;
        }
    }
}
