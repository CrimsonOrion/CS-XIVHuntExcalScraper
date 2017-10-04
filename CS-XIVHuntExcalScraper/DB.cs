using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CS_XIVHuntExcalScraper
{
    class DB
    {
        private static readonly string CONNECTION_STRING = Stuff.ConnectionStringTest;

        public static string updateQuery = string.Empty;

        /// <summary>
        /// Tests the connection to the SQLite database.  Returns true if the connection succeeds.
        /// </summary>
        /// <returns>Boolean true if the connection works.</returns>
        public static bool TestConnection()
        {
            bool result = false;

            MySqlConnection dbConnection = new MySqlConnection(CONNECTION_STRING);

            dbConnection.Open();
            if (dbConnection.State == ConnectionState.Open)
            {
                result = true;
                dbConnection.Close();
            }

            return result;
        }

        public static string RunQuery(string query)
        {
            string mobName = string.Empty;
            string queryString = query;
            MySqlConnection dbConnection = new MySqlConnection(CONNECTION_STRING);

            try
            {
                // Open the database to work with it
                dbConnection.Open();

                // Prepare the query to run with dbCommand
                using (MySqlCommand dbCommand = new MySqlCommand(queryString, dbConnection))
                {
                    // Execute the query
                    using (MySqlDataReader queryReader = dbCommand.ExecuteReader())
                    {
                        // while the results are still being read...
                        while (queryReader.Read())
                        {
                            // Get the name of the mob
                            mobName = queryReader.GetString(0);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                File.AppendAllText("ErrorInfo.txt", $"{DateTime.Now} - {e.Message}\r\n");
                Console.WriteLine($"{DateTime.Now} - {e.Message}");
            }
            finally
            {
                dbConnection.Close();
            }
            return mobName;
        }

        public static void RunQueryGetAllMobInfo()
        {
            MobSpawnInfo.AllMobInfo = new List<MobSpawnInfo>();
            string queryString = $"SELECT id, mob, rank, zone FROM `MobSpawnInfo`";
            MySqlConnection dbConnection = new MySqlConnection(CONNECTION_STRING);

            try
            {
                // Open the database to work with it
                dbConnection.Open();

                // Prepare the query to run with dbCommand
                using (MySqlCommand dbCommand = new MySqlCommand(queryString, dbConnection))
                {
                    // Execute the query
                    using (MySqlDataReader queryReader = dbCommand.ExecuteReader())
                    {
                        // while the results are still being read...
                        while (queryReader.Read())
                        {
                            // Get the name of the mob
                            MobSpawnInfo.AllMobInfo.Add(new MobSpawnInfo() { ID = queryReader.GetInt32(0), Mob = queryReader.GetString(1), Rank = queryReader.GetString(2), Zone = queryReader.GetString(3) });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                File.AppendAllText("ErrorInfo.txt", $"{DateTime.Now} - {e.Message}\r\n");
                Console.WriteLine($"{DateTime.Now} - {e.Message}");
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public static void RunQueryGetAllZones()
        {
            Zones.AllZones = new List<Zones>();

            string queryString = $"SELECT id, tablename, name, region FROM `Zones`";
            MySqlConnection dbConnection = new MySqlConnection(CONNECTION_STRING);

            try
            {
                // Open the database to work with it
                dbConnection.Open();

                // Prepare the query to run with dbCommand
                using (MySqlCommand dbCommand = new MySqlCommand(queryString, dbConnection))
                {
                    // Execute the query
                    using (MySqlDataReader queryReader = dbCommand.ExecuteReader())
                    {
                        // while the results are still being read...
                        while (queryReader.Read())
                        {
                            // Get the name of the mob
                            Zones.AllZones.Add(new Zones { ID = queryReader.GetInt32(0), TableName = queryReader.GetString(1), Name = queryReader.GetString(2), Region = queryReader.GetString(3) });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                File.AppendAllText("ErrorInfo.txt", $"{DateTime.Now} - {e.Message}\r\n");
                Console.WriteLine($"{DateTime.Now} - {e.Message}");
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public static List<ZoneInfo> RunQueryEachZoneInfo(string zone)
        {
            List<ZoneInfo> list = new List<ZoneInfo>();
            string queryString = $"SELECT id, x, y, spawned FROM `{zone}`";
            MySqlConnection dbConnection = new MySqlConnection(CONNECTION_STRING);

            try
            {
                // Open the database to work with it
                dbConnection.Open();

                // Prepare the query to run with dbCommand
                using (MySqlCommand dbCommand = new MySqlCommand(queryString, dbConnection))
                {
                    // Execute the query
                    using (MySqlDataReader queryReader = dbCommand.ExecuteReader())
                    {
                        // while the results are still being read...
                        while (queryReader.Read())
                        {
                            // Get the name of the mob
                            list.Add(new ZoneInfo() { Zone = (zone), ID = queryReader.GetInt32(0), X = (float)queryReader.GetDouble(1), Y = (float)queryReader.GetDouble(2), Spawned = queryReader.GetInt16(3) });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                File.AppendAllText("ErrorInfo.txt", $"{DateTime.Now} - {e.Message}\r\n");
                Console.WriteLine($"{DateTime.Now} - {e.Message}");
            }
            finally
            {
                dbConnection.Close();
            }
            return list;

        }

        public static int UpdateMobSpawnHistory(string queryString)
        {
            int recordsUpdated = 0;

            MySqlConnection dbConnection = new MySqlConnection(CONNECTION_STRING);

            try
            {
                // Open the database to work with it
                dbConnection.Open();

                // Prepare the query to run with dbCommand
                using (MySqlCommand dbCommand = new MySqlCommand(queryString, dbConnection))
                {
                    // Execute the query
                    recordsUpdated = dbCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                File.AppendAllText("ErrorInfo.txt", $"{DateTime.Now} - {e.Message}\r\n");
                Console.WriteLine($"{DateTime.Now} - {e.Message}");
            }
            finally
            {
                dbConnection.Close();
            }

            return recordsUpdated;
        }
    }

}
