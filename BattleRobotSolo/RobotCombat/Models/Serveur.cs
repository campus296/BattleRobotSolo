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
    public class Serveur : Joueur
    {
        public Socket? SocketClient { get; set; }

        public int ActionClient { get; set; }
        public int ActionServeur { get; set; }

        public Serveur(string nom, int port) : base(nom, port)
        {
        }

        public Robot CreerRobot(int ptVie, int ptArmure, int ptDegats)
        {
            Robot robot = new Robot(ptVie, ptArmure, ptDegats);

            return robot;
        }

        public override void ConfigRobot()
        {
            Robot robot;

            do
            {
                var config = System.SaisirConfigRobot();

                robot = CreerRobot(config.pv, config.armure, config.degats);

                if (!robot.VerifierConfiguration())
                {
                    Console.WriteLine("Configuration invalide. Vous devez distribuer exactement 10 points.");
                }

            } while (!robot.VerifierConfiguration());

            Partie.RobotServeur = robot;
        }

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

        public void LancerHerbergement()
        {
            EndPoint = new IPEndPoint(IPAddress.Any, Port);

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Socket.Bind(EndPoint);
            Socket.Listen(1);

            Console.WriteLine($"Adresse IP : {ObtenirAdresseIP()}");
            Console.WriteLine($"Port : {Port}");
        }

        public void RecevoirConfigurationClient()
        {
            if (SocketClient == null)
            {
                Partie.Status = -3;
                return;
            }

            try
            {
                byte[] buffer = new byte[1024];

                int nbOctets = SocketClient.Receive(buffer);

                if (nbOctets == 0)
                {
                    Partie.Status = -3;
                    return;
                }

                string json = Encoding.UTF8.GetString(buffer, 0, nbOctets);

                var options = new JsonSerializerOptions
                {
                    IncludeFields = true
                };

                var config = JsonSerializer.Deserialize<(int pv, int armure, int force)>(json, options);

                Robot robotClient = CreerRobot(config.pv, config.armure, config.force);

                if (robotClient.VerifierConfiguration())
                {
                    Partie.RobotClient = robotClient;
                    Partie.Status = 1;
                }
                else
                {
                    Partie.Status = -10;
                }
            }
            catch
            {
                Partie.Status = -10;
            }
        }

        public void RecevoirAction()
        {
            if (SocketClient == null)
            {
                Partie.Status = -3;
                return;
            }

            try
            {
                byte[] buffer = new byte[1];

                int nbOctets = SocketClient.Receive(buffer);

                if (nbOctets == 0)
                {
                    Partie.Status = -3;
                    return;
                }

                int action = buffer[0];

                if (action < 1 || action > 4)
                {
                    Partie.Status = -11;
                }
                else if (action == 2 && Partie.RobotClient.Energie < 50)
                {
                    Partie.Status = -11;
                }
                else
                {
                    ActionClient = action;
                    Partie.Status = 1;
                }
            }
            catch (SocketException)
            {
                Partie.Status = -3;
            }
        }

        public void VerifierAction(int action, Robot attaquant, Robot defenseur)
        {
            switch (action)
            {
                case 1:
                    defenseur.SubirDegat(attaquant.Degats, false);
                    break;

                case 2:
                    attaquant.Energie -= 50;
                    defenseur.SubirDegat(attaquant.Degats, true);
                    break;

                case 3:
                    attaquant.Defendre();
                    break;

                case 4:
                    attaquant.Recharger();
                    break;
            }
        }

        public void JouerTourServeur()
        {
            Console.WriteLine();
            Console.WriteLine($"=== TOUR DE {Nom} ===");

            do
            {
                ActionServeur = System.JouerAction();

                if (ActionServeur == 2 && Partie.RobotServeur.Energie < 50)
                {
                    Console.WriteLine("Pas assez d'énergie.");
                }

            } while (ActionServeur == 2 && Partie.RobotServeur.Energie < 50);
        }

        public void AppliquerActions()
        {
            if (ActionClient == 3)
            {
                Partie.RobotClient.Defendre();
            }

            if (ActionServeur == 3)
            {
                Partie.RobotServeur.Defendre();
            }

            if (ActionClient != 3)
            {
                VerifierAction(ActionClient, Partie.RobotClient, Partie.RobotServeur);
            }

            if (ActionServeur != 3)
            {
                VerifierAction(ActionServeur, Partie.RobotServeur, Partie.RobotClient);
            }

            VerifierFinPartie();
        }

        public void VerifierFinPartie()
        {
            if (Partie.RobotServeur.Pv <= 0)
            {
                Partie.Status = -1;
            }
            else if (Partie.RobotClient.Pv <= 0)
            {
                Partie.Status = -2;
            }
            else
            {
                Partie.Status = 1;
            }
        }

        public void TransmettreMiseAJour()
        {
            if (SocketClient == null)
            {
                Partie.Status = -3;
                return;
            }

            try
            {
                string json = JsonSerializer.Serialize(Partie);

                byte[] donnees = Encoding.UTF8.GetBytes(json);

                SocketClient.Send(donnees);
            }
            catch (SocketException)
            {
                Partie.Status = -3;
            }
        }

        public int RejouerPartie()
        {
            if (SocketClient == null)
            {
                Partie.Status = -3;
                return 2;
            }

            try
            {
                byte[] buffer = new byte[1];

                int nbOctets = SocketClient.Receive(buffer);

                if (nbOctets == 0)
                {
                    Partie.Status = -3;
                    return 2;
                }

                int choix = buffer[0];

                if (choix == 1 || choix == 2)
                {
                    return choix;
                }

                return 2;
            }
            catch (SocketException)
            {
                Partie.Status = -3;
                return 2;
            }
        }

        public void AttendreClient()
        {
            if (Socket == null)
            {
                return;
            }

            System.AfficherEnAttente();

            SocketClient = Socket.Accept();

            Console.WriteLine("Client connecté !");
        }
    }
}