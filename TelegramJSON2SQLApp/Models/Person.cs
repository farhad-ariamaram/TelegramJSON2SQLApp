using System;
using System.Collections.Generic;

#nullable disable

namespace TelegramJSON2SQLApp.Models
{
    public partial class Person
    {
        public long Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
    }
}
