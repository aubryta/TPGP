using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serveur.OptionsPartie
{
    class Facile : IOptions
    {
        public int getNbChoix()
        {
            return 3;
        }
        public int getIncr()
        {
            return 10;
        }
    }
}
