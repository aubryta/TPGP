using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serveur
{
    /*
     * TODO list
     * 0 - COMMENTER
     * 1 - bouton enable sur commencer tant que le choix des styles n'est pas fait
     * 2 - faire la fenêtre pour les boutons style
     * 3 - variable de classe nbchanson
     * 4 - jeufini = false 
     * 5 - temps d'une manche
     */ 
    class Serveur
    {
        private TcpListener listen;
        private Thread listenThread;
        private List<Partie> listePartie = new List<Partie>();

        public void serverStart()
        {
            this.listen = new TcpListener(IPAddress.Any, 25000);
            Console.WriteLine("Serveur connecté");
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();

            //p = new Partie();
            listePartie.Add(new Partie("Electro"));
            listePartie.Add(new Partie("Jazz"));
            foreach(Partie p in listePartie)
            {
                Thread t = new Thread(p.attendPartie);
                t.Start();
            }
        }

        private void ListenForClients()
        {
            this.listen.Start();

            while (true)
            {
                TcpClient client = this.listen.AcceptTcpClient();
                Echange e = new Echange();
                Joueur j = new Joueur();
                ThreadStart starter = delegate { e.Connexion(client, j, this); };
                new Thread(starter).Start();
            }
        }

        public Partie getPartie(String style)
        {
            for(int i = 0; i <listePartie.Count(); i++)
            {
                if (listePartie[i].getStyle().Equals(style))
                    return listePartie[i];
                    
            }
            return null;
        }
    }
}
