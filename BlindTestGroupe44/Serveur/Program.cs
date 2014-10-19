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
        static void Main(string[] args)
        {
            Serveur serv = new Serveur();
            serv.serverStart();
        }
    }
}
