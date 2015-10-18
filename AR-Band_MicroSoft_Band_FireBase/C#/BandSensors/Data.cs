using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BandSensors
{
    [DataContract]
    class Data
    {
        [DataMember]
        public long Calorie { get; set; }
        [DataMember]
        public int HeartRate { get; set; }
        [DataMember]
        public double SkinTemperature { get; set; }
        [DataMember]
        public string UV { get; set; }
        [DataMember]
        public long Steps { get; set; }
        [DataMember]
        public long Distance { get; set; }

    }
}
