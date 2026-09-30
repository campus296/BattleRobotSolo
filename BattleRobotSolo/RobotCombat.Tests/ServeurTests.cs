using RobotCombat.Models;

namespace RobotCombat.Tests
{
    [TestClass]
    public class ServeurTests
    {
        [TestMethod]
        public void VerifierAction_AttaqueNormale_InfligeDegats()
        {
            Serveur serveur = new Serveur("Serveur", 5000);
            Robot attaquant = new Robot();
            Robot defenseur = new Robot();

            serveur.VerifierAction(1, attaquant, defenseur);

            Assert.AreEqual(90, defenseur.Pv);
        }

        [TestMethod]
        public void VerifierAction_AttaquePuissante_InfligeDoubleDegats()
        {
            Serveur serveur = new Serveur("Serveur", 5000);
            Robot attaquant = new Robot();
            Robot defenseur = new Robot();

            attaquant.Energie = 50;

            serveur.VerifierAction(2, attaquant, defenseur);

            Assert.AreEqual(80, defenseur.Pv);
        }

        [TestMethod]
        public void VerifierAction_AttaquePuissante_Consomme50Energie()
        {
            Serveur serveur = new Serveur("Serveur", 5000);
            Robot attaquant = new Robot();
            Robot defenseur = new Robot();

            attaquant.Energie = 50;

            serveur.VerifierAction(2, attaquant, defenseur);

            Assert.AreEqual(0, attaquant.Energie);
        }

        [TestMethod]
        public void VerifierAction_Defense_AjouteArmureTemporaire()
        {
            Serveur serveur = new Serveur("Serveur", 5000);
            Robot attaquant = new Robot();
            Robot defenseur = new Robot();

            serveur.VerifierAction(3, attaquant, defenseur);

            Assert.AreEqual(10, attaquant.ArmureTemporaire);
        }

        [TestMethod]
        public void VerifierAction_Recharge_Ajoute50Energie()
        {
            Serveur serveur = new Serveur("Serveur", 5000);
            Robot attaquant = new Robot();
            Robot defenseur = new Robot();

            serveur.VerifierAction(4, attaquant, defenseur);

            Assert.AreEqual(50, attaquant.Energie);
        }

        [TestMethod]
        public void AppliquerActions_DefenseEstAppliqueeAvantAttaque()
        {
            Serveur serveur = new Serveur("Serveur", 5000);

            serveur.Partie.RobotServeur = new Robot();
            serveur.Partie.RobotClient = new Robot();

            serveur.ActionClient = 1;
            serveur.ActionServeur = 3;

            serveur.AppliquerActions();

            Assert.AreEqual(100, serveur.Partie.RobotServeur.Pv);
        }

        [TestMethod]
        public void VerifierFinPartie_RobotServeurZero_ClientGagne()
        {
            Serveur serveur = new Serveur("Serveur", 5000);

            serveur.Partie.RobotServeur.Pv = 0;
            serveur.Partie.RobotClient.Pv = 100;

            serveur.VerifierFinPartie();

            Assert.AreEqual(-1, serveur.Partie.Status);
        }

        [TestMethod]
        public void VerifierFinPartie_RobotClientZero_ServeurGagne()
        {
            Serveur serveur = new Serveur("Serveur", 5000);

            serveur.Partie.RobotServeur.Pv = 100;
            serveur.Partie.RobotClient.Pv = 0;

            serveur.VerifierFinPartie();

            Assert.AreEqual(-2, serveur.Partie.Status);
        }

        [TestMethod]
        public void VerifierFinPartie_DeuxRobotsVivants_PartieContinue()
        {
            Serveur serveur = new Serveur("Serveur", 5000);

            serveur.Partie.RobotServeur.Pv = 100;
            serveur.Partie.RobotClient.Pv = 100;

            serveur.VerifierFinPartie();

            Assert.AreEqual(1, serveur.Partie.Status);
        }
    }
}
