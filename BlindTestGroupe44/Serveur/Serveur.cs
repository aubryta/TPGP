﻿using System;
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

            //On crée une partie par style de musique
            GestionMusique gm = new GestionMusique();
            List<String> styles = gm.choixStyle();
            foreach (String style in styles)
                listePartie.Add(new Partie(style));

            //Et on lance toutes les parties
            foreach(Partie p in listePartie)
            {
                Thread t = new Thread(p.runGame);
                t.Start();
            }
        }

        private void ListenForClients()
        {
            this.listen.Start();
            //On écoute en boucle la connexion des clients
            while (true)
            {
                TcpClient client = this.listen.AcceptTcpClient();
                Echange e = new Echange();
                Joueur j = new Joueur();
                ThreadStart starter = delegate { e.Connexion(client, j, this); };
                new Thread(starter).Start();
            }
        }

        //Retourne la partie du style définie en paramètre
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
