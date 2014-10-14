using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serveur
{
    class GestionMusique
    {

        private String style = "";
        private String chanson = "";

        /*
         * 0 - COMMENTER
         * 1 - trouver aléatoire et doublon dans les chansons
         * 
         */ 
        public List<String> choixStyle()
        {
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
        public List<String> listeChansons(int nbChansons)
        {
            if(!style.Equals(""))
            {
                Console.WriteLine("Musique/" + style);
                DirectoryInfo dir = new DirectoryInfo("Musique/" + style + "/");
                FileInfo[] listeChansons = dir.GetFiles();
                List<String> listeMusique = new List<string>();
                for (int i = 0; i < nbChansons; i++)
                {
                    listeMusique.Add(listeChansons.ElementAt(i).Name);
                    Console.WriteLine(listeChansons.ElementAt(i).Name);
                }
                Random rnd = new Random();
                int placeChanson = rnd.Next(0, nbChansons -1);
                chanson = listeMusique.ElementAt(placeChanson);
                return listeMusique;
            }
            return null;
        }
        public String getChanson()
        {
            return chanson;
        }
    }
}
