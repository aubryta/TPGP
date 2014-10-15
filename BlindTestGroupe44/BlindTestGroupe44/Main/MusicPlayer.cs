using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;



namespace BlindTestGroupe44
{
    class MusicPlayer
    {
        // importation a faire car c# ne permet pas la lecture de .mp3, en fait on fait appel à l'OS.
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwndCallback);
        // je ne sais pas a quoi servent les deux derniers arguments

        [DllImport("winmm.dll", EntryPoint = "waveOutSetVolume")]
        public static extern int WaveOutSetVolume(IntPtr hwo, uint dwVolume);
        
        private String nomChanson = ""; // Nom de la chanson courante


        // recupere la liste des chanson d'un repertoire en parcourant toute son arborescence
        public IEnumerable<FileInfo> listeChanson (string path)
        {
            var dir = new DirectoryInfo(path);
            var extensions = new string[] { ".mp3", ".wma" }; // choix dse extension
            var mp3Files = dir.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower())); // le *.* permet de dire que tu cherche un fic
            var sousDossier = dir.GetDirectories();  
            foreach(DirectoryInfo path2 in sousDossier){
                mp3Files = mp3Files.Union(listeChanson(path2.FullName));
            }
            return mp3Files;
        }

        // Cette fonction ouvre une chanson aléatoire a aprtir du chemin d'un repertoire
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
                    nomChanson = chanson.Name;

                }
                catch { }
            }            
            string command = "open \"" + file + "\" type MPEGVideo alias MyMp3"; // c'est une commande pour ouvrir le fichier
            mciSendString(command, null, 0, 0);
        }

        // lancer la musique
        public void play()
        {
            string command = "play MyMp3";
            mciSendString(command, null, 0, 0);
        }

        // le paramettre d est une valeur comprise entre 0 et 10 ( ce sont les propriétés d'un slider)
        // on le multiplie arbitrairement par une valeur pour que le son puisse etre raisonablement fort
        // on le "converti" en un unsigned integer pour pouvoir faire le changement de volume.
        public void volume(double d)
        {
            d = d * 1000;
            uint v = ((uint)d) & 0xffff;
            uint vAll = v | (v << 16);
            // Set the volume
            int retVal = WaveOutSetVolume(IntPtr.Zero, vAll);

        }
        // arreter la musique
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
    }
   
}
