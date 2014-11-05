using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using WMPLib;



namespace BlindTestGroupe44
{
    /// <summary>
    /// Classe correspondant à la lecture / gestion des fichiers de musique
    /// </summary>
    class MusicPlayer
    {
        // importations a faire car c# ne permet pas la lecture de .mp3, en fait on fait appel à l'OS.
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwndCallback);  
        [DllImport("winmm.dll", EntryPoint = "waveOutSetVolume")]
        public static extern int WaveOutSetVolume(IntPtr hwo, uint dwVolume);
        
        private String nomChanson = ""; // Nom de la chanson courante
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer(); //lecteur media pour les url
        
        /// <summary>
        /// recupere la liste des chanson d'un repertoire en parcourant toute son arborescence
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IEnumerable<FileInfo> listeChanson (string path)
        {
            try
            {
                var dir = new DirectoryInfo(path);
                var extensions = new string[] { ".mp3", ".wma" }; // choix des extensions musiques
                var mp3Files = dir.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
                var sousDossier = dir.GetDirectories();
                foreach (DirectoryInfo path2 in sousDossier)
                {
                    mp3Files = mp3Files.Union(listeChanson(path2.FullName));
                }
                return mp3Files;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException());
                return null;
            }
        }

       
        /// <summary>
        /// Cette fonction ouvre un fichier chanson aléatoire a partir du chemin d'un repertoire
        /// </summary>
        /// <param name="path"></param>
        public void open(string path)
        {           
            string file = null;
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    var songsList = listeChanson(path);
                    Random R = new Random();
                    FileInfo chanson;
                    do
                    {
                       chanson = songsList.ElementAt(R.Next(0, songsList.Count()));
                    } while (nomChanson.Equals(chanson.Name)); //Evite de lancer 2 fois d'affilées la même chanson
                    
                    file = chanson.FullName;
                    nomChanson = chanson.Name.Split('.').ElementAt(0); ;
                }
                catch { }
            }            
            // Commande open en local
            string command = "open \"" + file + "\" type MPEGVideo alias MyMp3"; // c'est une commande pour ouvrir le fichier
            mciSendString(command, null, 0, 0);
        }

        /// <summary>
        /// Commande de lecture en local
        /// </summary>
        public void play()
        {
            string command = "play MyMp3";
            mciSendString(command, null, 0, 0);
        }

        /// <summary>
        /// Lecture d'une chanson via une URL
        /// </summary>
        /// <param name="urlChanson"></param>
        public void playFromURL(String urlChanson)
        {    
            wplayer.URL = urlChanson;
            wplayer.controls.play();   
            
        }

        /// <summary>
        /// Arret d'une chanson via une URL
        /// </summary>
        public void stopFromURL()
        {
            wplayer.controls.pause();
          
        }


        /// <summary>
        /// Met a jour le volume en local selon une formule 
        /// choisie arbitrairement pour avoir des bornes
        /// de volume correctes
        /// </summary>
        /// <param name="d"></param>
        public void volume(double d)
        {
            d = d * 1000;
            uint v = ((uint)d) & 0xffff;
            uint vAll = v | (v << 16);
            // Set the volume
            int retVal = WaveOutSetVolume(IntPtr.Zero, vAll);
        }

        /// <summary>
        /// Commande d'arret de la musique en local
        /// </summary>
        public void stop()
        {
            string command = "stop MyMp3";
            mciSendString(command, null, 0, 0); // il faut arreter la musique avant de fermer le fichier
            command = "close MyMp3";
            mciSendString(command, null, 0, 0); // fermeture du fichier
        }

        public String getChanson()
        {
            return nomChanson;
        }

        /// <summary>
        /// changement du volume pour un client en ligne
        /// </summary>
        /// <param name="d"></param>
        public void volumeFromURL(double d)
        {           
            wplayer.settings.volume = (int)d;       
        }
    }
   
}
