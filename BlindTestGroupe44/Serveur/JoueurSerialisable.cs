using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serveur
{
    /// <summary>
    /// Sert a la serialisation des joueurs et des scores
    /// </summary>
    public class JoueurSerialisable
    {
        public String nom { get; set; }
        public int score { get; set; }
    }
}
