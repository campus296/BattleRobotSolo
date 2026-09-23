using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RobotCombat.Models
{
    public class Serveur : Joueur
    {
        public Socket? SocketClient { get; set; }
        // Constructeur
        public Serveur(string nom, int port) : base(nom, port)
        {
        }

        // Crée le robot du serveur avec les points choisis
        public Robot CreerRobot(int ptVie, int ptArmure, int ptForce)
        {
            Robot robot = new Robot(ptVie, ptArmure, ptForce);

            return robot;
        }

        // Configure le robot du serveur
        public override void ConfigRobot(int pv, int armure, int force)
        {
            Robot robot = CreerRobot(pv, armure, force);

            if (robot.VerifierConfiguration())
            {
                Partie.RobotServeur = robot;

                Console.WriteLine();
                Console.WriteLine("Configuration du robot valide.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Configuration du robot invalide.");
            }
        }

        // Récupère l'adresse IPv4 de l'ordinateur
        public string ObtenirAdresseIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "Adresse IP introuvable";
        }

        // Affiche les informations du serveur pendant l'attente
        public void AfficherEnAttente()
        {
            Console.WriteLine();
            Console.WriteLine("=== SERVEUR ===");
            Console.WriteLine($"Adresse IP : {ObtenirAdresseIP()}");
            Console.WriteLine($"Port       : {Port}");
            Console.WriteLine();
            Console.WriteLine("En attente d'un client...");
        }

        // Lance le serveur et attend la connexion d'un client
        public void LancerHerbergement()
        {

            IPAddress adresseIP = IPAddress.Any;

            EndPoint = new IPEndPoint(adresseIP, Port);

            Socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            Socket.Bind(EndPoint);
            Socket.Listen(1);

            AfficherEnAttente();

            // Attend le client
            SocketClient = Socket.Accept();

            Console.WriteLine();
            Console.WriteLine("Client connecté !");
        }
        public void RecevoirConfigurationClient()
        {
            if (SocketClient == null)
            {
                Console.WriteLine("Aucun client connecté.");
                return;
            }

            byte[] buffer = new byte[1024];

            int nbOctets = SocketClient.Receive(buffer);

            string configuration = Encoding.UTF8.GetString(buffer,0, nbOctets
            );

            string[] valeurs = configuration.Split(';');

            if (valeurs.Length != 3)
            {
                Console.WriteLine("Configuration reçue invalide.");
                return;
            }

            if (int.TryParse(valeurs[0], out int pv) &&
                int.TryParse(valeurs[1], out int armure) &&
                int.TryParse(valeurs[2], out int force))
            {
                Robot robotClient = CreerRobot(pv, armure, force);

                if (robotClient.VerifierConfiguration())
                {
                    Partie.RobotClient = robotClient;

                    Console.WriteLine("Configuration du client valide.");
                }
                else
                {
                    Console.WriteLine("Configuration du client invalide.");
                }
            }
            else
            {
                Console.WriteLine("Configuration reçue invalide.");
            }
        }

        public void VerifierAction(int action, Robot attaquant, Robot defenseur)
        {
            switch (action)
            {
                case 1:
                    // Attaque normale
                    defenseur.SubirDegat(attaquant.Degats, false);
                    break;

                case 2:
                    // Attaque puissante
                    if (attaquant.Energie >= 50)
                    {
                        attaquant.Energie -= 50;
                        defenseur.SubirDegat(attaquant.Degats, true);
                    }
                    else
                    {
                        Console.WriteLine("Pas assez d'énergie.");
                    }
                    break;

                case 3:
                    // Défense
                    attaquant.Defendre();
                    break;

                case 4:
                    // Recharge
                    attaquant.Recharger();
                    break;

                default:
                    Console.WriteLine("Action invalide.");
                    break;
            }
        }

        public void RecevoirActionClient()
        {
            if (SocketClient == null)
            {
                Console.WriteLine("Aucun client connecté.");
                return;
            }

            byte[] buffer = new byte[1024];

            int nbOctets = SocketClient.Receive(buffer);

            string message = Encoding.UTF8.GetString(buffer, 0,nbOctets);

            if (int.TryParse(message, out int action))
            {
                VerifierAction(action, Partie.RobotClient, Partie.RobotServeur);
            }
            else
            {
                Console.WriteLine("Action reçue invalide.");
            }
        }

        public void JouerTourServeur()
        {
            Console.WriteLine();
            Console.WriteLine($"=== TOUR DE {Nom} ===");

            int action = JouerAction();

            VerifierAction(
                action,
                Partie.RobotServeur,
                Partie.RobotClient
            );
        }

        public void LancerCombat()
        {
            Partie.Status = 1;

            while (Partie.RobotServeur.Pv > 0 &&
                   Partie.RobotClient.Pv > 0)
            {
                Console.WriteLine();
                Console.WriteLine("==============================");
                Console.WriteLine("       NOUVEAU TOUR");
                Console.WriteLine("==============================");

                // Le client joue
                Console.WriteLine();
                Console.WriteLine("En attente de l'action du client...");

                RecevoirActionClient();

                // Vérifie si l'action du client a terminé la partie
                if (Partie.RobotServeur.Pv <= 0)
                {
                    break;
                }

                // Le serveur joue
                JouerTourServeur();

                // Vérifie si l'action du serveur a terminé la partie
                if (Partie.RobotClient.Pv <= 0)
                {
                    break;
                }

                Console.WriteLine();
                AfficherPartie(Partie);
            }
        }
    }
}
