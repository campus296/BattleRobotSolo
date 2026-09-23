using RobotCombat.Models;
using System.Net;

namespace RobotCombat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("================================");
            Console.WriteLine("       COMBAT DE ROBOTS");
            Console.WriteLine("================================");
            Console.WriteLine();

            Console.WriteLine("1 - Héberger une partie");
            Console.WriteLine("2 - Rejoindre une partie");
            Console.WriteLine();

            Console.Write("Votre choix : ");
            int choix = Joueur.SaisirInt();

            Console.Clear();

            if (choix == 1)
            {
                Console.WriteLine("=== CRÉATION DU SERVEUR ===");
                Console.WriteLine();

                Console.Write("Nom du joueur : ");
                string nom = Console.ReadLine() ?? "Serveur";

                Console.Write("Port : ");
                int port = Joueur.SaisirInt();

                Serveur serveur = new Serveur(nom, port);

                serveur.LancerHerbergement();

                serveur.SaisirConfigurationRobot();

                Console.WriteLine();
                Console.WriteLine("En attente de la configuration du client...");

                serveur.RecevoirConfigClient();

                serveur.LancerCombat();

                Console.WriteLine();
                serveur.AfficherPartie(serveur.Partie);

                Console.ReadLine();
            }
            else if (choix == 2)
            {
                Console.WriteLine("=== CONNEXION AU SERVEUR ===");
                Console.WriteLine();

                Console.Write("Nom du joueur : ");
                string nom = Console.ReadLine() ?? "Client";

                Console.Write("Adresse IP du serveur : ");
                string saisieIP = Console.ReadLine() ?? "";

                IPAddress adresseIP = Client.SaisirIP(saisieIP);

                Console.Write("Port du serveur : ");
                int port = Joueur.SaisirInt();

                Client client = new Client(nom, port);

                client.SeConnecter(adresseIP, port);

                client.SaisirConfigurationRobot();

                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine($"=== TOUR DE {client.Nom} ===");

                    int action = client.JouerAction();

                    client.EnvoyerAction(action);

                    Console.WriteLine("Action envoyée.");

                    // Temporaire pour synchroniser avec le serveur
                    Console.WriteLine();
                    Console.WriteLine("Appuyez sur Entrée pour continuer...");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Choix invalide.");
            }
        }
    }
}
