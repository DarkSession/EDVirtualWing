using ED_Virtual_Wing.Models;
using ED_Virtual_Wing.PlayerJournal.Events.Startup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ED_Virtual_Wing_Unit_Test.JournalEvents
{
    [TestClass]
    public class Startup
    {
        protected ApplicationTestDbContext ApplicationTestDbContext { get; } = new();

        [TestMethod]
        [DataRow(@"{ ""timestamp"":""2022-01-30T17:21:49Z"", ""event"":""LoadGame"", ""FID"":""0"", ""Commander"":""Dark Session"", ""Horizons"":true, ""Ship"":""Anaconda"", ""ShipID"":69, ""ShipName"":""[KED] JUMPACONDA"", ""ShipIdent"":""ISAR"", ""FuelLevel"":32.000000, ""FuelCapacity"":32.000000, ""GameMode"":""Solo"", ""Credits"":0, ""Loan"":0 }")]
        public async Task TestLoadGame(string json)
        {
            LoadGame? loadGame = JsonConvert.DeserializeObject<LoadGame>(json);
            Commander commander = new()
            {
                GameVersion = GameVersion.Odyssey,
                GameMode = GameMode.Open,
            };
            await (loadGame?.ProcessEntry(commander, ApplicationTestDbContext.DbContext) ?? ValueTask.CompletedTask);
            Assert.AreEqual(GameMode.Solo, commander.GameMode);
            Assert.AreEqual(GameVersion.Horizons, commander.GameVersion);
        }
    }
}
