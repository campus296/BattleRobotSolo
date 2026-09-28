using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace RobotCombat.Models
{
    public class Client : Joueur
    {
        public Client(string nom, int port) : base(nom, port)
        {
        }

        public override void ConfigRobot()
        {
            var config = System.SaisirConfigRobot();

            if (Socket == null)
            {
                return;
            }

            string configuration = $"{config.pv};{config.armure};{config.degats}";

            byte[] donnees = Encoding.UTF8.GetBytes(configuration);

            Socket.Send(donnees);
        }

        public void SeConnecter(IPAddress ip, int port)
        {
            EndPoint = new IPEndPoint(ip, port);

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine();
            Console.WriteLine($"Connexion à {ip}:{port}...");

            Socket.Connect(EndPoint);

            Console.WriteLine("Connexion réussie !");
        }

        public void EnvoyerAction(int action)
        {
            if (Socket == null)
            {
                return;
            }

            string message = action.ToString();

            byte[] donnees = Encoding.UTF8.GetBytes(message);

            Socket.Send(donnees);
        }

        public void RecevoirMiseAJour()
        {
            if (Socket == null)
            {
                return;
            }

            byte[] buffer = new byte[4096];

            int nbOctets = Socket.Receive(buffer);

            string json = Encoding.UTF8.GetString(buffer, 0, nbOctets);

            Partie? partieRecue = JsonSerializer.Deserialize<Partie>(json);

            if (partieRecue != null)
            {
                Partie = partieRecue;
            }
        }

        public void RejouerPartie(int choix)
        {
            if (Socket == null)
            {
                return;
            }

            byte[] donnees = Encoding.UTF8.GetBytes(choix.ToString());

            Socket.Send(donnees);
        }
    }
}
