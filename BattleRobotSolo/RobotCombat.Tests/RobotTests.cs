using RobotCombat.Models;

namespace RobotCombat.Tests
{
    [TestClass]
    public class RobotTests
    {
        [TestMethod]
        public void ConfigurerRobot_10Points_ConfigurationValide()
        {
            Robot robot = new Robot(4, 3, 3);

            bool resultat = robot.VerifierConfiguration();

            Assert.IsTrue(resultat);
        }

        [TestMethod]
        public void ConfigurerRobot_MoinsDe10Points_ConfigurationInvalide()
        {
            Robot robot = new Robot(2, 3, 3);

            bool resultat = robot.VerifierConfiguration();

            Assert.IsFalse(resultat);
        }

        [TestMethod]
        public void ConfigurerRobot_PlusDe10Points_ConfigurationInvalide()
        {
            Robot robot = new Robot(5, 4, 3);

            bool resultat = robot.VerifierConfiguration();

            Assert.IsFalse(resultat);
        }

        [TestMethod]
        public void CreerRobot_AvecPoints_CalculeBonnesStatistiques()
        {
            Robot robot = new Robot(4, 3, 3);

            Assert.AreEqual(140, robot.Pv);
            Assert.AreEqual(6, robot.Armure);
            Assert.AreEqual(16, robot.Degats);
            Assert.AreEqual(0, robot.Energie);
        }

        [TestMethod]
        public void AttaqueNormale_SansArmure_ReduitPv()
        {
            Robot robot = new Robot();

            robot.SubirDegat(10, false);

            Assert.AreEqual(90, robot.Pv);
        }

        [TestMethod]
        public void AttaqueNormale_AvecArmure_ReduitDegats()
        {
            Robot robot = new Robot();
            robot.Armure = 4;

            robot.SubirDegat(10, false);

            Assert.AreEqual(94, robot.Pv);
        }

        [TestMethod]
        public void AttaquePuissante_DoubleDegats()
        {
            Robot robot = new Robot();

            robot.SubirDegat(10, true);

            Assert.AreEqual(80, robot.Pv);
        }

        [TestMethod]
        public void AttaquePuissante_AvecArmure_ReduitDegats()
        {
            Robot robot = new Robot();
            robot.Armure = 4;

            robot.SubirDegat(10, true);

            Assert.AreEqual(84, robot.Pv);
        }

        [TestMethod]
        public void Defendre_Ajoute10ArmureTemporaire()
        {
            Robot robot = new Robot();

            robot.Defendre();

            Assert.AreEqual(10, robot.ArmureTemporaire);
        }

        [TestMethod]
        public void Defense_ProtegeContreProchaineAttaque()
        {
            Robot robot = new Robot();

            robot.Defendre();
            robot.SubirDegat(15, false);

            Assert.AreEqual(95, robot.Pv);
        }

        [TestMethod]
        public void Defense_ApresAttaque_Disparait()
        {
            Robot robot = new Robot();

            robot.Defendre();
            robot.SubirDegat(15, false);

            Assert.AreEqual(0, robot.ArmureTemporaire);
        }

        [TestMethod]
        public void Recharger_Ajoute50Energie()
        {
            Robot robot = new Robot();

            robot.Recharger();

            Assert.AreEqual(50, robot.Energie);
        }

        [TestMethod]
        public void Recharger_DeuxFois_Donne100Energie()
        {
            Robot robot = new Robot();

            robot.Recharger();
            robot.Recharger();

            Assert.AreEqual(100, robot.Energie);
        }

        [TestMethod]
        public void Recharger_NeDepassePas100Energie()
        {
            Robot robot = new Robot();

            robot.Recharger();
            robot.Recharger();
            robot.Recharger();

            Assert.AreEqual(100, robot.Energie);
        }

        [TestMethod]
        public void SubirDegat_PvNeDescendPasSousZero()
        {
            Robot robot = new Robot();

            robot.SubirDegat(200, false);

            Assert.AreEqual(0, robot.Pv);
        }
    }
}