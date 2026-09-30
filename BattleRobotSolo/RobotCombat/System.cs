using RobotCombat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RobotCombat
{
    public static class System
    {
        public static int SaisirInt()
        {
            int valeur;

            while (!int.TryParse(Console.ReadLine(), out valeur))
            {
                Console.Write("Veuillez entrer un nombre entier : ");
            }

            return valeur;
        }

        public static (int pv, int armure, int degats) SaisirConfigRobot()
        {
            Console.WriteLine();
            Console.WriteLine("=== CONFIGURATION DU ROBOT ===");
            Console.WriteLine("Vous avez 10 points à distribuer.");

            Console.Write("Points en PV : ");
            int pv = SaisirInt();

            Console.Write("Points en armure : ");
            int armure = SaisirInt();

            Console.Write("Points en dégâts : ");
            int degats = SaisirInt();

            return (pv, armure, degats);
        }

        public static int JouerAction()
        {
            int action;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1 - Attaquer");
                Console.WriteLine("2 - Attaque puissante");
                Console.WriteLine("3 - Défendre");
                Console.WriteLine("4 - Recharger");
                Console.Write("Action : ");

                action = SaisirInt();

                if (action < 1 || action > 4)
                {
                    Console.WriteLine("Action invalide.");
                }

            } while (action < 1 || action > 4);

            return action;
        }

        public static void AfficherPartie(Partie partie)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("----- SERVEUR -----");
            Console.WriteLine($"PV : {partie.RobotServeur.Pv}");
            Console.WriteLine($"Armure : {partie.RobotServeur.Armure}");
            Console.WriteLine($"Énergie : {partie.RobotServeur.Energie}");

            Console.WriteLine();

            Console.WriteLine("----- CLIENT -----");
            Console.WriteLine($"PV : {partie.RobotClient.Pv}");
            Console.WriteLine($"Armure : {partie.RobotClient.Armure}");
            Console.WriteLine($"Énergie : {partie.RobotClient.Energie}");
        }

        public static IPAddress SaisirIP()
        {
            IPAddress? adresseIP;

            do
            {
                Console.Write("Adresse IP du serveur : ");
                string saisie = Console.ReadLine() ?? "";

                if (IPAddress.TryParse(saisie, out adresseIP))
                {
                    return adresseIP;
                }

                Console.WriteLine("Adresse IP invalide.");

            } while (true);
        }

        public static void AfficherEnAttente()
        {
            Console.WriteLine();
            Console.WriteLine("En attente du client...");
        }

        public static void AfficherFinPartie(Partie partie)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("=== FIN DE LA PARTIE ===");

            if (partie.Status == -1)
            {
                Console.WriteLine("Le client a gagné !");
            }
            else if (partie.Status == -2)
            {
                Console.WriteLine("Le serveur a gagné !");
            }

            Console.WriteLine();
            Console.WriteLine("----- SERVEUR -----");
            Console.WriteLine($"PV : {partie.RobotServeur.Pv}");

            Console.WriteLine();

            Console.WriteLine("----- CLIENT -----");
            Console.WriteLine($"PV : {partie.RobotClient.Pv}");
        }

        public static int SaisirRejouer()
        {
            int choix;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1 - Rejouer");
                Console.WriteLine("2 - Quitter");
                Console.Write("Choix : ");

                choix = SaisirInt();

                if (choix != 1 && choix != 2)
                {
                    Console.WriteLine("Choix invalide.");
                }

            } while (choix != 1 && choix != 2);

            return choix;
        }

        public static void AfficherDeconnexion()
        {
            Console.Clear();
            Console.WriteLine("Le client s'est déconnecté.");
        }
    }
}