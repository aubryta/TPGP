using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serveur
{
    class Serveur
    {
        private TcpListener listen;
        private Thread listenThread;
        private ASCIIEncoding encodeur = new ASCIIEncoding();
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
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));

                clientThread.Start(client);
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;
            

            while (true)
            {
                bytesRead = 0;
                try
                {
                    //Attend la reception d'un message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERREUR RECEPTION" + e.ToString());
                    break;
                }

                if (bytesRead == 0)
                {
                    Console.WriteLine("CLIENT DECONNECTE");
                    break;
                }
                
                //Affiche le message reçu
                

                String bufferincmessage = encodeur.GetString(message, 0, bytesRead);

                Console.WriteLine(bufferincmessage);
                send("MESSAGE?salut", clientStream);
            }
        }

        private void send(String reponse, NetworkStream clientStream)
        {
            Console.WriteLine(reponse);
            byte[] buffer = encodeur.GetBytes(reponse);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }
    }
}
