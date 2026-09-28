using RobotCombat.Models;
using System.Net;

namespace RobotCombat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== COMBAT DE ROBOTS ===");
            Console.WriteLine();
            Console.WriteLine("1 - Serveur");
            Console.WriteLine("2 - Client");
            Console.Write("Choix : ");

            int choix = System.SaisirInt();

            if (choix == 1)
            {
                Console.WriteLine();
                Console.WriteLine("=== SERVEUR ===");

                Console.Write("Nom du joueur : ");
                string nom = Console.ReadLine() ?? "";

                Console.Write("Port : ");
                int port = System.SaisirInt();

                Serveur serveur = new Serveur(nom, port);

                // Le serveur est démarré une seule fois
                serveur.LancerHerbergement();

                // Boucle permettant d'accepter plusieurs clients
                while (true)
                {
                    serveur.AttendreClient();

                    serveur.Partie = new Partie();

                    bool rejouer = true;

                    // Plusieurs parties possibles avec le même client
                    while (rejouer)
                    {
                        serveur.ConfigRobot();

                        serveur.RecevoirConfigurationClient();

                        while (serveur.Partie.Status == -10)
                        {
                            serveur.TransmettreMiseAJour();

                            if (serveur.Partie.Status != -10)
                            {
                                break;
                            }

                            serveur.RecevoirConfigurationClient();
                        }

                        if (serveur.Partie.Status == -3)
                        {
                            break;
                        }

                        serveur.TransmettreMiseAJour();

                        if (serveur.Partie.Status == -3)
                        {
                            break;
                        }

                        System.AfficherPartie(serveur.Partie);

                        while (serveur.Partie.Status == 1)
                        {
                            serveur.RecevoirAction();

                            while (serveur.Partie.Status == -11)
                            {
                                serveur.TransmettreMiseAJour();

                                if (serveur.Partie.Status != -11)
                                {
                                    break;
                                }

                                serveur.RecevoirAction();
                            }

                            if (serveur.Partie.Status != 1)
                            {
                                break;
                            }

                            serveur.JouerTourServeur();

                            serveur.AppliquerActions();

                            serveur.TransmettreMiseAJour();

                            if (serveur.Partie.Status == 1)
                            {
                                System.AfficherPartie(serveur.Partie);
                            }
                        }

                        if (serveur.Partie.Status == -3)
                        {
                            break;
                        }

                        System.AfficherFinPartie(serveur.Partie);

                        int choixRejouer = serveur.RejouerPartie();

                        if (serveur.Partie.Status == -3)
                        {
                            break;
                        }

                        if (choixRejouer == 1)
                        {
                            serveur.Partie = new Partie();
                        }
                        else
                        {
                            rejouer = false;
                        }
                    }

                    if (serveur.Partie.Status == -3)
                    {
                        System.AfficherDeconnexion();
                    }

                    serveur.SocketClient?.Close();
                    serveur.SocketClient = null;
                }
            }
            else if (choix == 2)
            {
                Console.WriteLine();
                Console.WriteLine("=== CLIENT ===");

                Console.Write("Nom du joueur : ");
                string nom = Console.ReadLine() ?? "";

                IPAddress ip = System.SaisirIP();

                Console.Write("Port : ");
                int port = System.SaisirInt();

                Client client = new Client(nom, port);

                client.SeConnecter(ip, port);

                bool rejouer = true;

                while (rejouer)
                {
                    client.ConfigRobot();

                    client.RecevoirMiseAJour();

                    while (client.Partie.Status == -10)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Configuration invalide. Veuillez recommencer.");

                        client.ConfigRobot();

                        client.RecevoirMiseAJour();
                    }

                    System.AfficherPartie(client.Partie);

                    while (client.Partie.Status == 1)
                    {
                        int action = System.JouerAction();

                        client.EnvoyerAction(action);

                        client.RecevoirMiseAJour();

                        while (client.Partie.Status == -11)
                        {
                            Console.WriteLine("Action invalide. Veuillez recommencer.");

                            action = System.JouerAction();

                            client.EnvoyerAction(action);

                            client.RecevoirMiseAJour();
                        }

                        System.AfficherPartie(client.Partie);
                    }

                    System.AfficherFinPartie(client.Partie);

                    int choixRejouer = System.SaisirRejouer();

                    client.RejouerPartie(choixRejouer);

                    if (choixRejouer == 1)
                    {
                        client.Partie = new Partie();
                    }
                    else
                    {
                        rejouer = false;
                    }
                }
            }
            else
            {
                Console.WriteLine("Choix invalide.");
            }

            Console.ReadLine();
        }
    }
}