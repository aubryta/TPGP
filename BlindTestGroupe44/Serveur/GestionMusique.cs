using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace Serveur
{
    /// <summary>
    /// Cette classe corrseponda la gestion des fichiers
    /// stockés sur le serveur ftp
    /// </summary>
    class GestionMusique
    {

        private String style = "";
        private String chanson = "";
        private List<String> chansonsRep = new List<string>();
        private int nbChoixMax = 0;
        private String urlBase = "ftp://ftp.magix-online.com";// url du serveur ftp
        // identifiants
        private String idUser = "";
        private String mdpUser = "";
        private int tentativeConnexion = 0;

        /// <summary>
        /// On initialise la classe avec l'identifiant et le mot de passe
        /// du serveur ftp
        /// </summary>
        /// <param name="id">identifiant utilisateur</param>
        /// <param name="mdp">mot de passe utilisateur</param>
        public GestionMusique()
        {
            this.idUser = "aubry.tom@live.fr";
            this.mdpUser = "coucou34.";
        }

        /// <summary>
        /// Retourne la liste des fichiers contenus dans le répertoire en paramètre
        /// </summary>
        /// <param name="repertoire">Adresse du répertoire</param>
        /// <returns>La liste des fichiers du serveur</returns>
        public List<String> listeSousDossier(String repertoire)
        {
            List<String> res = new List<string>();
            // on crée une requete ftp qui demande la liste des repertoire
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(repertoire);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            // identifiant
            request.Credentials = new NetworkCredential(idUser, mdpUser);
            // recupération de la réponse dans un stream
            FtpWebResponse response = null;
            // Il arrive  que la connection ne se fasse pas
            // nous arretons au bout de 3 essais
            try
            {
                response = (FtpWebResponse)request.GetResponse();
            }
            catch
            {   //On essaye de se connecter au serveur ftp 3 fois sinon on abandone et quitte l'applic
                Console.WriteLine("Echec de la connexion !");
                if (tentativeConnexion < 3)
                {
                    tentativeConnexion++;
                    Console.WriteLine("Tentative de re-connexion : " + tentativeConnexion + " " + repertoire);
                    Thread.Sleep(1000);
                    listeSousDossier(repertoire);
                }
                else
                {
                    System.Environment.Exit(1);
                }
            }
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            String liste = reader.ReadToEnd();
            // on split le resultat en fonction des caractères spéciaux et on ajoute les resultats dans la liste res
            String[] tabStyle = liste.Split(new Char[] { '\n', '\r', ' ', '\t' });
            for (int i = 0; i < (tabStyle.Length); i++)
            {
                if (!tabStyle[i].Equals("index.html") && !tabStyle[i].Equals(""))
                {
                    res.Add(tabStyle[i]);
                }
            }
            reader.Close();
            response.Close();
            return res;
        }

        /// <summary>
        /// Stocke dans une liste les chansons contenu dans le style de musique
        /// associé au style de la classe GestionMusique
        /// </summary>
        public void chercheChansons()
        {
            chansonsRep = listeSousDossier(urlBase + "/" + style);
        }

        /// <summary>
        /// Mélange la liste de chansons du style de musique concerné, 
        /// Retire également une chanson tirée aléatoirement qui sera la chanson
        /// a trouver.
        /// </summary>
        public void melange()
        {
            //Test lors de la première utilisation (il n'y a aucune chanson précédente)
            if (!chanson.Equals(""))
                chansonsRep.Add(chanson);

            Random rnd = new Random();
            String tmp = "";
            int swapIndex = 0;
            //melange
            for (int i = 0; i < nbChoixMax; i++)
            {
                tmp = chansonsRep.ElementAt(i);
                swapIndex = rnd.Next(0, chansonsRep.Count - 1);
                chansonsRep[i] = chansonsRep.ElementAt(swapIndex);
                chansonsRep[swapIndex] = tmp;
            }

            //Choisit et retire une chanson au hasard
            int alea = rnd.Next(0, chansonsRep.Count);
            chanson = chansonsRep.ElementAt(alea);
            chansonsRep.RemoveAt(alea);
        }

        /// <summary>
        /// Cherche la liste des styles du serveur depuis la racine
        /// </summary>
        /// <returns>La liste des style</returns>
        public List<String> choixStyle()
        {
            return listeSousDossier(urlBase);
        }

        /// <summary>
        /// Retourne le style associé a la gestion de la musique
        /// </summary>
        /// <returns>Le style associé</returns>
        public String getStyle()
        {
            return style;
        }

        /// <summary>
        /// Associe un style de musique à la gestion de musique d'une partie
        /// </summary>
        /// <param name="style">Le style à associer</param>
        public void setStyle(String style)
        {
            this.style = style;
        }

        /// <summary>
        /// Associe le nombre de chansons qui seront tirées au maximum dans une partie
        /// </summary>
        /// <param name="nbChoix">Le nombre de chanson possible</param>
        public void setNbChoixMax(int nbChoix)
        {
            this.nbChoixMax = nbChoix;
        }
       
        /// <summary>
        /// Retourne une liste aléatoire de chansons
        /// avec la chanson à trouver aléatoirement placée dedans
        /// </summary>
        /// <param name="nbChansons">Le nombre de chanson de la liste</param>
        /// <returns>La liste de chanson à retournée</returns>
        public List<String> listeChansons(int nbChansons)
        {
            if (chansonsRep.Count == 0)
                return null;

            List<String> res = new List<string>();
            Random rnd = new Random();
            //on cherche aléatoirement ou cette chanson va aller dans la liste
            int aleaChanson = rnd.Next(0, nbChansons);
            for (int i = 0; i < nbChansons; i++)
            {
                if (i == aleaChanson)
                    res.Add(chanson);
                else
                    res.Add(chansonsRep.ElementAt(i));
            }
            return res;
        }

        /// <summary>
        /// Retourne la chanson qui est à trouver lors de la manche
        /// </summary>
        /// <returns>La chanson qui est à trouver</returns>
        public String getChanson()
        {
            return chanson;
        }

        /// <summary>
        /// Retourne l'url de la chanson qui est a trouver
        /// </summary>
        /// <returns>L'url de la chanson qui est a trouver</returns>
        public String getUrlChanson()
        {
            return "http://tpgp.magix.net/public/" + style + "/" + chanson;
        }
    }
}
