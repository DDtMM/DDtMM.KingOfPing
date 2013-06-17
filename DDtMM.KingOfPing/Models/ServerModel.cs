using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace DDtMM.KingOfPing.Models
{
    [DataContract]
    public class ServerModel
    {
        public enum ServerState
        {
            Stopped,
            Waiting,
            Pinging,
            Error
        }

        [DataMember]
        public int MinSleep;

        [DataMember]
        public int MaxSleep;

        [DataMember]
        public int MaxLogSize;

        [DataMember]
        public ServerState Status;
    }
}