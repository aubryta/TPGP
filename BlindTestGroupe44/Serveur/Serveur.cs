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
    class Serveur
    {
        private TcpListener listen;
        private Thread listenThread;
        private ASCIIEncoding encodeur = new ASCIIEncoding();
        private NetworkStream clientStream = null;
        private String chanson = "";
        private bool jeuFini = false;
        private GestionMusique gestMusique = new GestionMusique();

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
                Thread clientThread = new Thread(new ParameterizedThreadStart(Connexion));

                clientThread.Start(client);
            }
        }

        private void Connexion(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream cstm = tcpClient.GetStream();

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
                traite(bufferincmessage, cstm);
            }
        }

        private void traite(String message, NetworkStream cstm)
        {
            String[] tabMessage = message.Split('?');
            if (tabMessage[0].Equals(""))
            {
                send("MESSAGE?Message mal formé", cstm);
                Console.WriteLine("***** Erreur lecture commande *****");
            }
            else
            {
                if (tabMessage[0].Equals("OPTIONS"))
                {
                    if (tabMessage[1].Equals("FACILE"))
                        send("OPTIONS?3?10", cstm);
                    else if (tabMessage[1].Equals("MOYEN"))
                        send("OPTIONS?4?12", cstm);
                    else if (tabMessage[1].Equals("DIFFICILE"))
                        send("OPTIONS?6?15", cstm);
                    else
                        send("ERREUR difficulté inconnu", cstm);
                }
                else if (tabMessage[0].Equals("CHANSON"))
                {
                    if (tabMessage[1].Equals(chanson))
                    {
                        send("INFO?BONNECHANSON?" + chanson, cstm);
                    }
                    else //mauvaise chanson
                    {
                        send("INFO?MAUVAISECHANSON?" + chanson, cstm);
                    }
                    if (jeuFini == false)
                    {
                        chercheChanson();
                        String res = "MUSIQUE";
                        foreach (String chansonCourante in chercheChanson())
                        {
                            res += "?" + chansonCourante;
                        }
                        send(res, cstm);
                    }
                }
                else if (tabMessage[0].Equals("INFO"))
                {
                    if (tabMessage[1].Equals("START"))
                    {
                        String res = "MUSIQUE";
                        foreach (String chanson in chercheChanson())
                            res += "?" + chanson;
                        send(res, cstm);
                    }
                    
                }
                else if(tabMessage[0].Equals("CHOIXSTYLE"))
                {
                    string res = "CHOIXSTYLE";
                    foreach (String style in gestMusique.choixStyle())
                        res += "?" + style;
                    send(res, cstm);
                }
                else
                    send("MESSAGE?Message non traité", cstm);
            }
        }

        /// <summary>
        /// Trouve aléatoirement une playlist de chanson
        /// </summary>
        /// <returns></returns>

        private List<String> chercheChanson()
        {
            List<String> listechanson = new List<string>();
            chanson = "katy"; // on définit la bonne chanson à katy
            listechanson.Add("salut");
            listechanson.Add("katy");
            listechanson.Add("george");
            return listechanson;
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
