using DDtMM.KingOfPing.Controllers;
using DDtMM.KingOfPing.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DDtMM.KingOfPing
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PingService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PingService.svc or PingService.svc.cs at the Solution Explorer and start debugging.
    public class PingService : IPingService
    {
        public void RunNow()
        {
            PingManager.Instance.PingAllSites();
            
        }

        public DateTime GetServerTime()
        {
            return DateTime.Now;
        }

        
        public List<Models.SiteModel> GetSites()
        {
            List<Models.SiteModel> sites = SiteController.LoadSites();
            return sites;
        }

        public List<Models.LogModel> GetLogs(int siteID)
        {
            return LogController.LoadSiteLogs(siteID).ToList();
        }


        public DateTime GetNextRunTime()
        {
            return PingManager.Instance.NextRunTime();
        }
    }
}
