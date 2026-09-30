using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RobotCombat.Models
{
    public abstract class Joueur
    {
        public string Nom { get; set; }
        public int Port { get; set; }
        public IPEndPoint? EndPoint { get; set; }
        public Socket? Socket { get; set; }
        public Partie Partie { get; set; }

        public Joueur(string nom, int port)
        {
            Nom = nom;
            Port = port;
            Partie = new Partie();
        }

        public abstract void ConfigRobot();
    }
}
