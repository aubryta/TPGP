using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace TestURL{
   
    
    class GestionURL
    {

        private String urlBase = "ftp://ftp.magix-online.com";

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

        public List<String> listStyle()
        {
            return listeSousDossier(urlBase);
        }
       
        

        public List<String> listeChansons (String style){
            return listeSousDossier(urlBase + "/" + style);
        }

        public String getUrlChanson(String style, String chanson){
            return (urlBase + "/" + style+"/"+chanson);
        }


    }


    
}
