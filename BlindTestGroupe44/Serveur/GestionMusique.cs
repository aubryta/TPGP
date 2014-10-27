using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Serveur
{
    class GestionMusique
    {

        private String style = "";
        private String chanson = "";
        private List<String> chansonsRep = new List<string>();       
        private int nbChoixMax = 0;
        private String urlBase = "ftp://ftp.magix-online.com";
        /*
         * Pour utiliser cette classe, 
         * 1 - il faut d'abord initialiser la liste (chercheChansons)
         * 2 - trouver aléatoirement une chanson et mélanger la liste (melange)
         * 3 - on utilise le nombre de choix voulu (listechanson(nbChoix) : vu que la liste est mélangé à chaque fois, 
         *      les nbChoix premier seront utilisés hors la chanson à trouver
         * 4 - on peux comparer la réponse de l'utilisateur avec la chanson getChanson()
         * 5 - pour recommencer on reprend à l'étape 2
         */

        public List<String> listeSousDossier(String racine)
        {
            List<String> res = new List<string>();
            // on crée une requete ftp qui demande la liste des repertoire
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(racine);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            // identifiant
            request.Credentials = new NetworkCredential("aubry.tom@live.fr", "coucou34.");
            // recupération de la réponse dans un stream
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            String liste = reader.ReadToEnd();
            // on split le resultat et on ajoute les resultats dans la liste
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


        public void chercheChansons()
        {
            chansonsRep = listeSousDossier(urlBase + "/" + style);
        }

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


        public List<String> choixStyle()
        {
            return listeSousDossier(urlBase);
        }

        public String getStyle()
        {
            return style;
        }
        public void setStyle(String style)
        {
            this.style = style;
        }

        public void setNbChoixMax(int nbChoix)
        {
            this.nbChoixMax = nbChoix;
        }
        // la liste chanson rep, et retire aléatoirement une chanson pour la 
        //placer dans chanson
       
        
        //Va rechercher dans le répertoire toutes les chansons et les stockent dans "chansonsRep"
       


        //Retourne une liste aléatoire de chanson
        //Sauvegarde une chanson parmis cette liste dans "chanson".
        public List<String> listeChansons(int nbChansons)
        {
            if (chansonsRep.Count == 0)
                return null;

            List<String> res = new List<string>();
            Random rnd = new Random();
            //on cherche aléatoirement ou cette chanson va aller dans la liste
            int aleaChanson = rnd.Next(0, nbChansons);
            for (int i = 0; i < nbChansons; i ++ )
            {
                if (i == aleaChanson)
                    res.Add(chanson);
                else
                    res.Add(chansonsRep.ElementAt(i));
            }
            return res;
        }

        public String getChanson()
        {
            return chanson;
        }

        public String getUrlChanson()
        {
            return "http://tpgp.magix.net/public/" + style + "/" + chanson;
        }
    }
}
