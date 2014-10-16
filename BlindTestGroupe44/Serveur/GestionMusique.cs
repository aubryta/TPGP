using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Serveur
{
    class GestionMusique
    {

        private String style = "";
        private String chanson = "";
        private List<String> chansonsRep = new List<string>();

        /*
         * 0 - COMMENTER
         * 1 - trouver aléatoire et doublon dans les chansons
         * 2 - couvrir le fait répertoire vide
         * 
         */ 
        public List<String> choixStyle()
        {
            
            //************
            List<String> listeStyle = new List<string>();
            DirectoryInfo dir = new DirectoryInfo("Musique");
            DirectoryInfo[] styles = dir.GetDirectories();

            foreach (DirectoryInfo fichier in styles)
            {
                listeStyle.Add(fichier.Name);
            }
            return listeStyle;
        }

        public String getStyle()
        {
            return style;
        }
        public void setStyle(String style)
        {
            this.style = style;
        }
        
        //Va rechercher dans le répertoire toutes les chansons et les stockent dans "chansonsRep"
        public void chercheChansons()
        {
            DirectoryInfo dir = new DirectoryInfo("Musique/" + style + "/");
            FileInfo[] listeChansons = dir.GetFiles();
            List<String> listeMusique = new List<string>();
            for (int i = 0; i < listeChansons.Length; i++)
            {
                chansonsRep.Add(listeChansons[i].Name);
            }
        }

        //Retourne une liste aléatoire de chanson
        //Sauvegarde une chanson parmis cette liste dans "chanson".
        public List<String> listeChansons(int nbChansons)
        {
            List<String> res = new List<string>();
            Random rnd = new Random();
            
            for (int i = 0; i < nbChansons; i ++ )
            {
                Console.WriteLine("umber" + chansonsRep.Count);
                int alea = rnd.Next(0, chansonsRep.Count);
                res.Add(chansonsRep.ElementAt(alea));
                chansonsRep.RemoveAt(alea);
                Console.WriteLine("umber" + chansonsRep.Count);
            }
            foreach (String chan in res)
                chansonsRep.Add(chan);

            int placeChanson = rnd.Next(0, nbChansons);
            chanson = res.ElementAt(placeChanson);
                
            return res;
        }
        public String getChanson()
        {
            return chanson;
        }
    }
}
