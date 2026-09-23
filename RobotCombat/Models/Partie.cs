using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotCombat.Models
{
    public class Partie
    {
        public int Statut { get; set; }
        public Robot RobotServeur { get; set; }
        public Robot RobotClient { get; set; }

        public Partie()
        {
            Statut = 2;
            RobotServeur = new Robot();
            RobotClient = new Robot();
        }

        public Partie(Robot robotServeur, Robot robotClient)
        {
            Statut = 2;
            RobotServeur = robotServeur;
            RobotClient = robotClient;
        }
    }
}
