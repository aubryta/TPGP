using Serveur.OptionsPartie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serveur
{
    class Joueur
    {
        private NetworkStream stm = null;
        private String name = "";
        private int score = 0;
        private int nbChoix = 0;
        private int incrPoint = 0;
        private IOptions io = null;
        private String style = "";

        public void setDifficulte(String difficulte)
        {
            
            OptionsFactory of = new OptionsFactory();
            io = of.createOptions(difficulte);
            this.nbChoix = io.getNbChoix();
            this.incrPoint = io.getIncr();

        }
        public int getIncr()
        {
            return incrPoint;
        }
        public void setStyle(String style)
        {
            this.style = style ;
        }

        public String getStyle()
        {
            return style;
        }
        public void setStream(NetworkStream stm)
        {
            this.stm = stm;
        }

        public void setName(String name)
        {
            this.name = name;
        }

        public void setScore(int score)
        {
            this.score = score;
        }

        public void setNbChoix(int nbChoix)
        {
            this.nbChoix = nbChoix;
        }

        public String getName()
        {
            return name;
        }

        public int getScore()
        {
            return score;
        }

        public int getNbChoix()
        {
            return nbChoix;
        }

        public NetworkStream getStream()
        {
            return stm;
        }
        public int incrScore()
        {
            score += incrPoint;
            return score;
        }

    }
}
