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
    }
}
