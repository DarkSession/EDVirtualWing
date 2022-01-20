using ED_Virtual_Wing.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ED_Virtual_Wing.Models
{
    [Table("StarSystemBody")]
    public class StarSystemBody
    {
        [JsonIgnore]
        [Column]
        public long Id { get; set; }

        [JsonIgnore]
        [Column]
        public long StarSystemId { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonIgnore]
        [ForeignKey("StarSystemId")]
        public StarSystem StarSystem { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Column]
        public int BodyId { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; } = string.Empty;

        private static HttpClient HttpClient { get; } = new();

        public static async Task<StarSystemBody?> GetStarSystemBodyAsync(long systemAddress, int bodyId, ApplicationDbContext applicationDbContext)
        {
            StarSystemBody? systemBody = await applicationDbContext.StarSystemBodies.FirstOrDefaultAsync(s => s.StarSystem.SystemAddress == systemAddress && s.BodyId == bodyId);
            if (systemBody == null)
            {
                StarSystem? starSystem = await applicationDbContext.StarSystems.FirstOrDefaultAsync(s => s.SystemAddress == systemAddress);
                if (starSystem != null)
                {
                    try
                    {
                        // If the current instance doesn't know the body, we can query it from EDCT.
                        // This currently requires no authentication but will so in the future.
                        using HttpResponseMessage response = await HttpClient.GetAsync($"https://api.edct.dev/system/{systemAddress}/body/{bodyId}");
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            SystemBodyResponse? systemBodyResponse = JsonConvert.DeserializeObject<SystemBodyResponse>(responseBody);
                            if (systemBodyResponse != null)
                            {
                                systemBody = new()
                                {
                                    BodyId = bodyId,
                                    Name = systemBodyResponse.BodyName,
                                    StarSystem = starSystem,
                                };
                                applicationDbContext.StarSystemBodies.Add(systemBody);
                                await applicationDbContext.SaveChangesAsync();
                            }
                        }
                    }
                    catch (Exception  e)
                    {

                    }
                }
            }
            return systemBody;
        }

        public class SystemBodyResponse
        {
            public string SystemName { get; set; } = string.Empty;
            public string BodyName { get; set; } = string.Empty;
        }
    }
}
