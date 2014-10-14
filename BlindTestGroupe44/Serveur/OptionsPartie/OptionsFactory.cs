using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serveur.OptionsPartie
{
    class OptionsFactory
    {

        public IOptions createOptions(String nom)
        {
            if (nom.Equals("FACILE"))
                return (new Facile());
            else if (nom.Equals("MOYEN"))
                return (new Moyen());
            else if (nom.Equals("DIFFICILE"))
                return (new Difficile());
            return null;

        }

        public List<String> getOptions()
        {
            List<String> listeOptions = new List<string>();
            listeOptions.Add("FACILE");
            listeOptions.Add("MOYEN");
            listeOptions.Add("DIFFICILE");
            return listeOptions;
        }
    }
}
