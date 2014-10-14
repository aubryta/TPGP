using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serveur.OptionsPartie
{
    class Moyen : IOptions
    {
        public int getNbChoix()
        {
            return 4;
        }
        public int getIncr()
        {
            return 12;
        }
    }
}
