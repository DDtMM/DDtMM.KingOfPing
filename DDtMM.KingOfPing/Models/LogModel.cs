using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace DDtMM.KingOfPing.Models
{
    [DataContract]
    public class LogModel
    {
        [DataMember]
        public int LogID;

        [DataMember]
        public int SiteID;

        [DataMember]
        public DateTime RunTime;

        [DataMember]
        public bool Success;

        [DataMember]
        public string ResultCode;

        [DataMember]
        public string Message;
    }
}