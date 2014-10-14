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

        public void serverStart()
        {
            this.listen = new TcpListener(IPAddress.Any, 25000);
            Console.WriteLine("client se connect");
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }

        private void ListenForClients()
        {
            this.listen.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = this.listen.AcceptTcpClient();
                //
                ///////////////////////////////////////////////////

                //create a thread to handle communication
                //with connected client
                Echange e = new Echange();
                Thread clientThread = new Thread(new ParameterizedThreadStart(e.Connexion));

                clientThread.Start(client);
            }
        }

        
    }
}
