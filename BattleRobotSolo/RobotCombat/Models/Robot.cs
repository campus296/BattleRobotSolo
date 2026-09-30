using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotCombat.Models
{
    public class Robot
    {
        public int Pv { get; set; } = 100;
        public int Armure { get; set; } = 0;
        public int Degats { get; set; } = 10;
        public int Energie { get; set; } = 0;
        public int ArmureTemporaire { get; set; } = 0;

        // Constructeur vide pour la sérialisation
        public Robot()
        {
        }

        // Constructeur utilisé lors de la configuration du robot
        public Robot(int ptPv, int ptArmure, int ptForce)
        {
            Pv = 100 + (ptPv * 10);
            Armure = ptArmure * 2;
            Degats = 10 + (ptForce * 2);
            Energie = 0;
        }


        // Constructeur permettant de copier un robot (pas utilisé ici mais pouvant être utile dans le futur)
        /*public Robot(int pv, int armure, int degats, int energie)
        {
            Pv = pv;
            Armure = armure;
            Degats = degats;
            Energie = energie;
        }*/
        public void Defendre()
        {
            ArmureTemporaire = 10;
        }

        public void Recharger()
        {
            Energie += 50;

            if (Energie > 100)
            {
                Energie = 100;
            }
        }

        public void SubirDegat(int degat, bool puissante)
        {
            if (puissante)
            {
                degat *= 2;
            }

            int degatSubi = degat - (Armure + ArmureTemporaire);

            if (degatSubi < 0)
            {
                degatSubi = 0;
            }

            Pv -= degatSubi;

            if (Pv < 0)
            {
                Pv = 0;
            }

            ArmureTemporaire = 0;
        }

        public bool VerifierConfiguration()
        {
            int pointsPv = (Pv - 100) / 10;
            int pointsArmure = Armure / 2;
            int pointsDegats = (Degats - 10) / 2;

            int totalPoints = pointsPv + pointsArmure + pointsDegats;

            return totalPoints == 10;
        }
    }
}
