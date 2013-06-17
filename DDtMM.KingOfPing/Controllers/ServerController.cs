using DDtMM.KingOfPing.Models;
using DDtMM.KingOfPing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDtMM.KingOfPing.Managers;

namespace DDtMM.KingOfPing.Controllers
{
    public static class ServerController
    {
        /// <summary>
        /// The currently running server instance.
        /// </summary>
        public static ServerModel Current;

        static ServerController()
        {
            Current = LoadServer();
        }

        public static ServerModel LoadServer()
        {
            websitePingConfig.serverRow row = DataManager.Instance.PingSettings.server.First();
            return new ServerModel()
            {
                MaxLogSize = row.logSize,
                MaxSleep = row.maxSleep,
                MinSleep = row.minSleep
            };
        }

        public static void SaveServer(ServerModel server)
        {
            websitePingConfig.serverRow row = DataManager.Instance.PingSettings.server.First();
            row.BeginEdit();
            row.logSize = server.MaxLogSize;
            row.maxSleep = server.MaxSleep;
            row.minSleep = server.MinSleep;
            row.EndEdit();
            row.AcceptChanges();
        }
    }
}