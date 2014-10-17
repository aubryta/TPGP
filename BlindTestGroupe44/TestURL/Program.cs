using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TestURL
{
    class Program
    {
        static void Main(string[] args)
        {
            GestionURL gestion = new GestionURL();
            List<String> listchanson = gestion.listeChansons("Electro");
            foreach (String chanson in listchanson)          
                Console.WriteLine(chanson);
            Console.ReadLine();

        }
    }
}
