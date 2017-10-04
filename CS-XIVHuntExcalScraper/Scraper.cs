using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CS_XIVHuntExcalScraper
{
    class Scraper
    {
        private static HttpClient Http = new HttpClient();
        public static async Task ScrapeXIVHuntAPIAsync()
        {
            string urlAddress = "https://xivhunt.net/api/worlds";
            string response;
            MobSpawnInfo.CurrentMobInfo = new List<MobSpawnInfo>();
            string data = string.Empty;
            try
            {
                using (HttpResponseMessage r = await Http.GetAsync(urlAddress))
                {
                    if (r.IsSuccessStatusCode)
                        response = await r.Content.ReadAsStringAsync();
                    else
                        return;

                    List<World> worlds = JsonConvert.DeserializeObject<List<World>>(response);
                    DateTime twentySecondsAgo = DateTime.Now.AddSeconds(-20);                    
                    foreach (World excalHunts in worlds.Where(w => w.id == 93))
                    {
                        foreach (Hunt hunt in excalHunts.hunts)
                        {
                            DateTime lastReportedEST = hunt.lastReported.ToLocalTime();
                            if (lastReportedEST > twentySecondsAgo)
                            {
                                var /* MobSpawnInfo.IEnum*/ mobInfo = MobSpawnInfo.AllMobInfo.Where(mI => mI.ID == hunt.id);
                                string mobName = string.Empty;
                                string mobZone = string.Empty;
                                
                                foreach(MobSpawnInfo mSI in mobInfo)
                                {
                                    mobName = mSI.Mob;
                                    mobZone = mSI.Zone;
                                }
                                // Conversion time!
                                float x = 0f, y = 0f;
                                if (mobZone == "Coerthas Western Highlands" || mobZone == "The Sea Of Clouds" || mobZone == "The Dravanian Forelands" || mobZone == "The Dravanian Hinterlands" || mobZone == "The Churning Mists" || mobZone == "Azys Lla")
                                {
                                    x = (float)Math.Round( hunt.x / 50 + 22.5f, 1);
                                    y = (float)Math.Round( hunt.y / 50 + 22.5f, 1);
                                }
                                else
                                {
                                    x = (float)Math.Round( hunt.x / 50 + 21.5f, 1);
                                    y = (float)Math.Round( hunt.y / 50 + 21.5f, 1);
                                }
                                string sqlFormattedLastReported = lastReportedEST.ToString("yyyy-MM-dd HH:mm:ss");
                                data += $"ID: {hunt.id} - Mob: {mobName} - Rank: {hunt.Rank} - Zone: {mobZone} - Last Seen: {sqlFormattedLastReported} - XY: {x},{y} - Last Seen Alive: {hunt.lastAlive}\r\n";

                                // Only on Excal
                                if (hunt.Rank == Hunt.HuntRank.S) { DB.UpdateMobSpawnHistory($"UPDATE `{mobZone.Replace(" ", "")}` SET spawned = 0 where spawned = 1;"); }
                                MobSpawnInfo.CurrentMobInfo.Add(new MobSpawnInfo { ID = hunt.id, Mob = mobName, Rank = hunt.Rank.ToString(), Zone = mobZone.Replace(" ", "") /* Make Zone=ZoneTable */ , X = x, Y = y, TimeSeen = sqlFormattedLastReported, SeenAlive = hunt.lastAlive.ToString() });
                            }
                        }
                    }
                    int records = 0;
                    if (!data.Equals(string.Empty))
                    {
                        File.WriteAllText("HuntInfo.txt", $"{DateTime.Now}\r\n{data}");


                        // Verify and fill valid data
                        VerifySpawnPoints();



                        records = DB.UpdateMobSpawnHistory(DB.updateQuery);
                    }
                    Console.WriteLine($"Pulled information at {DateTime.Now}.  {records} inserted.");
                }                
            }
            catch (Exception e)
            {
                File.AppendAllText("ErrorInfo.txt", $"{DateTime.Now} - {e.Message}\r\n");
                Console.WriteLine($"{DateTime.Now} - {e.Message}");
            }
        }

        public static void VerifySpawnPoints()
        {
            bool found = false;
            float radius = 0.0f;
            DB.updateQuery = string.Empty;

            try
            {
                foreach (var info in MobSpawnInfo.CurrentMobInfo)
                {
                    var currentZoneInfo = ZoneInfo.AllZoneInfo.Where(z => z.Zone == info.Zone);
                    foreach (var cZI in currentZoneInfo)
                    {

                        // If a HW or SB zone, make it the search radius 1 square (50 malms).  If an ARR zone, make it 0.5 sq (25 malms) since they're spawn points are closer together.
                        if (info.Zone == "CoerthasWesternHighlands" || info.Zone == "AzysLla" || info.Zone == "TheDravanianForelands" || info.Zone == "TheDravanianHinterlands" || info.Zone == "TheChurningMists" || info.Zone == "TheSeaOfClouds" || info.Zone == "TheLochs" || info.Zone == "TheFringes" || info.Zone == "ThePeaks" || info.Zone == "TheAzimSteppe" || info.Zone == "TheRubySea" || info.Zone == "Yanxia")
                        {
                            radius = 1.0f;
                        }
                        else
                        {
                            radius = 0.5f;
                        }

                        // check if the spawn point is within range of a known spawn point
                        if (info.X < cZI.X + radius && info.X > cZI.X - radius && info.Y < cZI.Y + radius && info.Y > cZI.Y - radius)
                        {
                            // If so, add it to the update query
                            DB.updateQuery += $"UPDATE `{info.Zone}` SET spawned = 1 WHERE ID = {cZI.ID} AND spawned = 0;";
                            Console.WriteLine($"Found ID:{info.ID} Name:{info.Mob} Rank:{info.Rank} in {info.Zone} at {info.X},{info.Y}. Last Seen: {info.TimeSeen} - Alive? {info.SeenAlive}");
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        // if now, make a note of it to look at later.
                        File.AppendAllText("NoSpawnPointFound.txt", $"{DateTime.Now} - ID: {info.ID}, Name: {info.Mob}, Zone: {info.Zone}, Rank: {info.Rank}, Coord: {info.X},{info.Y}, Alive? {info.SeenAlive}\r\n");
                        Console.WriteLine($"NoSpawnPointFound: {DateTime.Now} - ID: {info.ID}, Name: {info.Mob}, Zone: {info.Zone}, Rank: {info.Rank}, Coord: {info.X},{info.Y}, Alive? {info.SeenAlive}");
                    }

                    // reset the 'found' variable
                    found = false;
                }
            }
            catch (Exception e)
            {
                File.AppendAllText("ErrorInfo.txt", $"{DateTime.Now} - {e.Message}\r\n");
                Console.WriteLine($"{DateTime.Now} - {e.Message}");
            }
        }
    }

    public class World
    {
#pragma warning disable IDE1006 // Naming Styles
        public int id { get; set; }
        public List<Hunt> hunts { get; set; }
    }

    public class Hunt
    {
        public int wId { get; set; }
        public int id { get; set; }
        //public HuntRank r { get; set; }
        [JsonProperty("r")]
        public HuntRank Rank { get; set; }
        public DateTime lastReported { get; set; }
        public int instance { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public bool lastAlive { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        public enum HuntRank
        {
            B,
            A,
            S
        }
    }

}
