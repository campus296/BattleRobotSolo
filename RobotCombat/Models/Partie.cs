using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotCombat.Models
{
    public class Partie
    {
        public int Status { get; set; }
        public Robot RobotServeur { get; set; }
        public Robot RobotClient { get; set; }

        public Partie()
        {
            Status = 2;
            RobotServeur = new Robot();
            RobotClient = new Robot();
        }

        public Partie(Robot robotServeur, Robot robotClient)
        {
            Status = 2;
            RobotServeur = robotServeur;
            RobotClient = robotClient;
        }
    }
}
