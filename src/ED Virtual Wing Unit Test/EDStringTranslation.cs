using ED_Virtual_Wing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ED_Virtual_Wing_Unit_Test
{
    [TestClass]
    public class EDStringTranslation
    {
        [TestMethod]
        [DataRow("$cmdr_decorate:#name=Fazza;", "CMDR Fazza")]
        [DataRow("$RolePanel2_crew; $cmdr_decorate:#name=Dark Session;", "Crew CMDR Dark Session")]
        [DataRow("$POIScenario_Watson_Damaged_Eagle_01_Salvage_Medium; $USS_ThreatLevel:#threatLevel=2;", "Distress Beacon [Threat 2]")]
        [DataRow("$npc_name_decorate:#name=WILL ROGERS;", "WILL ROGERS")]
        public void TestTranslation(string input, string expectedOutput)
        {
            Assert.IsTrue(EDTranslatedString.IsLocalizedString(input));
            Assert.AreEqual(expectedOutput, EDTranslatedString.ProcessLocalisationString(input));
        }
    }
}
