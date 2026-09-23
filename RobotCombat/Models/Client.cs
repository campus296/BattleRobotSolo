using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RobotCombat.Models
{
    public class Client : Joueur
    {
        public Client(string nom, int port) : base(nom, port)
        {
        }

        // Configure le robot du client
        public override void ConfigRobot(int pv, int armure, int force)
        {
            if (Socket == null)
            {
                Console.WriteLine("Le client n'est pas connecté.");
                return;
            }

            string configuration = $"{pv};{armure};{force}";

            byte[] donnees = Encoding.UTF8.GetBytes(configuration);

            Socket.Send(donnees);

            Console.WriteLine();
            Console.WriteLine("Configuration envoyée au serveur.");
        }

        // Vérifie et retourne une adresse IP valide
        public static IPAddress SaisirIP(string saisie)
        {
            IPAddress? adresseIP;

            while (!IPAddress.TryParse(saisie, out adresseIP))
            {
                Console.Write("Adresse IP invalide. Réessayez : ");
                saisie = Console.ReadLine() ?? "";
            }

            return adresseIP;
        }

        // Se connecte au serveur
        public void SeConnecter(IPAddress ip, int port)
        {
            EndPoint = new IPEndPoint(ip, port);

            Socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            Console.WriteLine();
            Console.WriteLine($"Connexion à {ip}:{port}...");

            // Synchrone : attend que la connexion soit établie
            Socket.Connect(EndPoint);

            Console.WriteLine("Connexion réussie !");
        }

        public void EnvoyerAction(int action)
        {
            if (Socket == null)
            {
                Console.WriteLine("Le client n'est pas connecté.");
                return;
            }

            string message = action.ToString();

            byte[] donnees = Encoding.UTF8.GetBytes(message);

            Socket.Send(donnees);
        }
    }
}
