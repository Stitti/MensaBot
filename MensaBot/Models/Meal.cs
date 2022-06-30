using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MensaBot.Models
{
    [DataContract]
    public class Meal
    {
        [DataMember(Name ="id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "notes")]
        public List<string> Notes { get; set; }
        [DataMember(Name = "prices")]
        public Prices Prices { get; set; }
        [DataMember(Name = "category")]
        public string Category { get; set; }
    }
}
