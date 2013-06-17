using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace DDtMM.KingOfPing.Models
{
    [DataContract]
    public class SiteModel
    {

        [DataMember]
        public int SiteID;

        [DataMember]
        public string Url;

        [DataMember]
        public DateTime StartTime;

        [DataMember]
        public DateTime EndTime;

        /// <summary>
        /// Time between runs in seconds
        /// </summary>
        [DataMember]
        public int Interval;

        [DataMember]
        public DateTime NextRun;

        //[DataMember]
        //public List<LogModel> Logs;
    }
}