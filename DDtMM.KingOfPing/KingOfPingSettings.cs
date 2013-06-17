using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace DDtMM.KingOfPing
{
    public class KingOfPingSettings : ConfigurationSection
    {
        public static readonly KingOfPingSettings Instance;

        static KingOfPingSettings()
        {
            Instance = ConfigurationManager.GetSection("KingOfPingSettings") as KingOfPingSettings;
        }

        [ConfigurationProperty("settingsPath", IsRequired = true)]
        public string ConfigPath
        {
            get { return (string)this["settingsPath"]; }
            set { this["settingsPath"] = value; }
        }

        public string FullConfigPath
        {
            get 
            {
                return System.AppDomain.CurrentDomain.BaseDirectory + 
                    VirtualPathUtility.ToAbsolute(ConfigPath).Substring(1);

            }
        }
    }
}