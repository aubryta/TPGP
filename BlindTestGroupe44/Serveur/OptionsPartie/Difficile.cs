using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serveur.OptionsPartie
{
    class Difficile : IOptions
    {
        public int getNbChoix()
        {
            return 6;
        }
        public int getIncr()
        {
            return 15;
        }
    }
}
