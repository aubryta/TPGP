using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serveur
{
    class Program
    {
        /// <summary>
        /// Crée et lance l'application serveur
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Serveur serv = new Serveur();
            serv.serverStart();
        }
    }
}
