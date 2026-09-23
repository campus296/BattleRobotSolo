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

        public static int SaisirInt()
        {
            int valeur;

            while (!int.TryParse(Console.ReadLine(), out valeur))
            {
                Console.Write("Veuillez entrer un nombre entier : ");
            }

            return valeur;
        }

        public int JouerAction()
        {
            int action;

            do
            {
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

        public abstract void ConfigRobot(int pv, int armure, int force);

        public void AfficherPartie(Partie partie)
        {
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
        public void SaisirConfigurationRobot()
        {
            int pv;
            int armure;
            int force;

            do
            {
                Console.WriteLine();
                Console.WriteLine("=== CONFIGURATION DU ROBOT ===");
                Console.WriteLine("Vous avez 10 points à distribuer.");
                Console.WriteLine();

                Console.Write("Points en PV : ");
                pv = SaisirInt();

                Console.Write("Points en armure : ");
                armure = SaisirInt();

                Console.Write("Points en force : ");
                force = SaisirInt();

                if (pv < 0 || armure < 0 || force < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Les points ne peuvent pas être négatifs.");
                }
                else if (pv + armure + force != 10)
                {
                    Console.WriteLine();
                    Console.WriteLine("Vous devez distribuer exactement 10 points.");
                }

            } while (pv < 0 || armure < 0 || force < 0 || pv + armure + force != 10);

            ConfigRobot(pv, armure, force);
        }
    }
}
