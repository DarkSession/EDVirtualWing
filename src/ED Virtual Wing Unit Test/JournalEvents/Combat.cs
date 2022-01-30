using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.PlayerJournal.Events.Combat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ED_Virtual_Wing_Unit_Test.JournalEvents
{
    [TestClass]
    public class Combat
    {
        protected ApplicationTestDbContext ApplicationTestDbContext { get; } = new();

        [TestMethod]
        [DataRow(@"{ ""timestamp"": ""2016-06-10T14:32:03Z"", ""event"": ""Died"", ""KillerName"": ""$ShipName_Police_Independent;"", ""KillerShip"": ""viper"", ""KillerRank"": ""Deadly"" }")]
        public async Task DiedEvent(string json)
        {
            Died? died = JsonConvert.DeserializeObject<Died>(json);
            Commander commander = new();
            await (died?.ProcessEntry(commander, ApplicationTestDbContext.DbContext) ?? ValueTask.CompletedTask);
            Assert.AreEqual(GameActivity.Dead, commander.GameActivity);
        }
    }
}   
