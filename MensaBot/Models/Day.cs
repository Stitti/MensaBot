using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MensaBot.Models
{
    [DataContract]
    public class Day
    {
        [DataMember(Name = "date")]
        public DateTime Date { get; set; }
        [DataMember(Name = "closed")]
        public bool IsClosed { get; set; }
    }
}
