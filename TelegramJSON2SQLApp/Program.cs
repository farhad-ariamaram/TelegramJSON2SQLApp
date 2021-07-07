using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace TelegramJSON2SQLApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            JsonSerializer serializer = new JsonSerializer();
            Model o;
            Message msg;
            long counter = 0;
            long counterFrom = Int64.Parse(ReadCounter());
            SqlConnection conn = new SqlConnection(ReadCS());
            SqlCommand cmd = new SqlCommand("AddNew", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            await conn.OpenAsync();

            using (FileStream fs = File.Open(ReadJA(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    if (line.StartsWith("{\"message\""))
                    {
                        if (counter <= counterFrom)
                        {
                            counter += 1;
                            continue;
                        }
                        using (StringReader sr2 = new StringReader(line))
                        using (JsonReader reader = new JsonTextReader(sr2))
                        {
                            while (await reader.ReadAsync())
                            {
                                if (reader.TokenType == JsonToken.StartObject)
                                {
                                    try
                                    {
                                        o = serializer.Deserialize<Model>(reader);
                                        msg = JsonConvert.DeserializeObject<Message>(o.Message);

                                        var username = o.Username == null ? "" : o.Username;

                                        cmd.Parameters.AddWithValue("@fname", msg.FirstName);
                                        cmd.Parameters.AddWithValue("@lname", msg.LastName);
                                        cmd.Parameters.AddWithValue("@phone", msg.Phone);
                                        cmd.Parameters.AddWithValue("@username", username);

                                        await cmd.ExecuteNonQueryAsync();
                                        cmd.Parameters.Clear();
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.ReadKey();
        }

        private static string ReadCounter()
        {
            string s = null;
            using (StreamReader sr = File.OpenText("c.txt"))
            {
                s = sr.ReadLine();
            }
            return s;
        }

        private static string ReadCS()
        {
            string s = null;
            using (StreamReader sr = File.OpenText("cs.txt"))
            {
                s = sr.ReadLine();
            }
            return s;
        }

        private static string ReadJA()
        {
            string s = null;
            using (StreamReader sr = File.OpenText("ja.txt"))
            {
                s = sr.ReadLine();
            }
            return s;
        }

    }

    public class Model
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("@version")]
        public string Version { get; set; }

        [JsonProperty("typ")]
        public string Typ { get; set; }

        [JsonProperty("access_hash")]
        public string AccessHash { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("@timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("flags")]
        public int Flags { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("imported_from")]
        public string ImportedFrom { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public class PFlags
    {
        [JsonProperty("contact")]
        public bool Contact { get; set; }
    }

    public class Status
    {
        [JsonProperty("was_online")]
        public int WasOnline { get; set; }
    }

    public class Message
    {
        [JsonProperty("pFlags")]
        public PFlags PFlags { get; set; }

        [JsonProperty("flags")]
        public int Flags { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("access_hash")]
        public string AccessHash { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }
    }

}