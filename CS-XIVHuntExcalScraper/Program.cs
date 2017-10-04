using System.Collections.Generic;
using System.Threading;

namespace CS_XIVHuntExcalScraper
{
    class Program
    {
        static void Main(string[] args)
        {// Populate zone list
            DB.RunQueryGetAllZones();

            // Populate all mob info
            DB.RunQueryGetAllMobInfo();

            // Populate full Mob/Zone statuf
            ZoneInfo.AllZoneInfo = new List<ZoneInfo>();
            foreach (var zone in Zones.AllZones)
            {
                List<ZoneInfo> list = new List<ZoneInfo>(DB.RunQueryEachZoneInfo(zone.TableName));
                ZoneInfo.AllZoneInfo.AddRange(list);
            }

            while (true)
            {
                ScrapeAPIAsync(); //async, passes through immediately
                Thread.Sleep(10000); // wait 10 seconds to go again
            }

        }

        static async void ScrapeAPIAsync()
        {
            await Scraper.ScrapeXIVHuntAPIAsync();
        }
    }
}
