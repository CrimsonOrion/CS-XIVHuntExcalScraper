using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_XIVHuntExcalScraper
{
    class MobSpawnInfo
    {
        public int ID { get; set; }
        public string Mob { get; set; }
        public string Rank { get; set; }
        public string Zone { get; set; }
        public string TimeSeen { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public string SeenAlive { get; set; }

        public static List<MobSpawnInfo> CurrentMobInfo;
        public static List<MobSpawnInfo> AllMobInfo;
    }

    class ZoneInfo
    {
        public string Zone { get; set; }
        public int ID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public short Spawned { get; set; }

        public static List<ZoneInfo> AllZoneInfo;

    }

    class Zones
    {
        public int ID { get; set; }
        public string TableName { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }

        public static List<Zones> AllZones;
    }

    class WorldName
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public WorldName(string id, string name)
        {
            ID = id;
            Name = name;
        }

        public static List<WorldName> WorldNames = new List<WorldName>()
        {
            new WorldName("73","Adamantoise"),
            new WorldName("90","Aegis"),
            new WorldName("43","Alexander"),
            new WorldName("44","Anima"),
            new WorldName("23","Asura"),
            new WorldName("68","Atomos"),
            new WorldName("69","Bahamut"),
            new WorldName("91","Balmung"),
            new WorldName("78","Behemoth"),
            new WorldName("24","Belias"),
            new WorldName("34","Brynhildr"),
            new WorldName("79","Cactuar"),
            new WorldName("45","Carbuncle"),
            new WorldName("80","Cerberus"),
            new WorldName("70","Chocobo"),
            new WorldName("74","Coeurl"),
            new WorldName("62","Diabolos"),
            new WorldName("92","Durandal"),
            new WorldName("93","Excalibur"),
            new WorldName("53","Exodus"),
            new WorldName("54","Faerie"),
            new WorldName("35","Famfrit"),
            new WorldName("46","Fenrir"),
            new WorldName("58","Garuda"),
            new WorldName("63","Gilgamesh"),
            new WorldName("81","Goblin"),
            new WorldName("94","Gungnir"),
            new WorldName("47","Hades"),
            new WorldName("95","Hyperion"),
            new WorldName("59","Ifrit"),
            new WorldName("48","Ixion"),
            new WorldName("40","Jenova"),
            new WorldName("49","Kujata"),
            new WorldName("55","Lamia"),
            new WorldName("64","Leviathan"),
            new WorldName("36","Lich"),
            new WorldName("83","Louisoix"),
            new WorldName("75","Malboro"),
            new WorldName("82","Mandragora"),
            new WorldName("96","Masamune"),
            new WorldName("37","Mateus"),
            new WorldName("65","Midgardsormr"),
            new WorldName("71","Moogle"),
            new WorldName("66","Odin"),
            new WorldName("39","Omega"),
            new WorldName("28","Pandaemonium"),
            new WorldName("56","Phoenix"),
            new WorldName("97","Ragnarok"),
            new WorldName("60","Ramuh"),
            new WorldName("98","Ridill"),
            new WorldName("99","Sargatanas"),
            new WorldName("29","Shinryu"),
            new WorldName("67","Shiva"),
            new WorldName("57","Siren"),
            new WorldName("76","Tiamat"),
            new WorldName("61","Titan"),
            new WorldName("72","Tonberry"),
            new WorldName("50","Typhon"),
            new WorldName("51","Ultima"),
            new WorldName("77","Ultros"),
            new WorldName("30","Unicorn"),
            new WorldName("52","Valefor"),
            new WorldName("31","Yojimbo"),
            new WorldName("41","Zalera"),
            new WorldName("32","Zeromus"),
            new WorldName("42","Zodiark")
        };
    }
}
