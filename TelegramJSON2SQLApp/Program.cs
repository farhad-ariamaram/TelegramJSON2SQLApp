using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using TelegramJSON2SQLApp.Models;

namespace TelegramJSON2SQLApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TelegramDBContext _db = new TelegramDBContext();
            JsonSerializer serializer = new JsonSerializer();
            Model o;
            Message msg;
            using (FileStream fs = File.Open(@"E:\telegramir\db.json", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    if (line.StartsWith("{\"message\""))
                    {
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
                                        await _db.People.AddAsync(new Person
                                        {
                                            Fname = msg.FirstName,
                                            Lname = msg.LastName,
                                            Phone = msg.Phone,
                                            Username = o.Username
                                        });
                                        await _db.SaveChangesAsync();
                                    }
                                    catch (Exception)
                                    {
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
