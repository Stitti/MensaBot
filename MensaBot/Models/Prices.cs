using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MensaBot.Models
{
    [DataContract]
    public class Prices
    {
        [DataMember(Name = "students")]
        public double Students { get; set; }
        [DataMember(Name = "employees")]
        public double Employees { get; set; }
        [DataMember(Name = "others")]
        public double Others { get; set; }
    }
}
