using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDtMM.KingOfPing.Managers
{
    public class DataManager : IDisposable
    {
        public static DataManager Instance { get; private set; }

        static DataManager()
        {
            Instance = new DataManager();
        }

        public websitePingConfig PingSettings;

        private DataManager()
        {
            LoadConfig();
        }

        public void LoadConfig()
        {
            if (PingSettings != null) PingSettings.Dispose();
            PingSettings = new websitePingConfig();
            PingSettings.ReadXml(KingOfPingSettings.Instance.FullConfigPath);
        }

        public void SaveChanges()
        {
            //PingSettings.AcceptChanges();
            PingSettings.WriteXml(KingOfPingSettings.Instance.FullConfigPath);
        }

        public void Dispose()
        {
            if (PingSettings != null) PingSettings.Dispose();
        }
    }
}