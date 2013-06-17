using DDtMM.KingOfPing.Controllers;
using DDtMM.KingOfPing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace DDtMM.KingOfPing.Managers
{
    public class PingManager 
    {
        public static PingManager Instance { get; private set; }

        static PingManager()
        {
            Instance = new PingManager();
        }

        public PingManager()
        {

        }

        public DateTime NextRunTime()
        {
            DateTime now = DateTime.Now;
            DateTime minNextRun = now.AddSeconds(ServerController.Current.MinSleep);
            DateTime maxNextRun = now.AddSeconds(ServerController.Current.MaxSleep);
            DateTime nextRun = DataManager.Instance.PingSettings.site
                .Min(s => (s.IsnextRunNull()) ? now :  s.nextRun);

            return (nextRun > minNextRun) ? (nextRun > maxNextRun) ? maxNextRun : nextRun : minNextRun;
        }

        public double NextRunSeconds()
        {
            return (NextRunTime() - DateTime.Now).TotalSeconds;
        }

        public void PingSites(List<SiteModel> sites)
        {
            sites.ForEach(s => PingSite(s));
            DataManager.Instance.SaveChanges();
        }

        public void PingAllSites()
        {
            PingSites(SiteController.LoadSites());
        }

        public void PingSite(SiteModel site)
        {
            DateTime pingBegan = DateTime.Now;
   
            site.NextRun = pingBegan.AddSeconds(site.Interval);

            if (!Util.TimeInRange(site.NextRun, site.StartTime, site.EndTime))
            {
                if (site.NextRun < site.EndTime)
                    site.NextRun = site.NextRun.Date.Add(site.StartTime.TimeOfDay).AddDays(1);
                else
                    site.NextRun = site.NextRun.Date.Add(site.StartTime.TimeOfDay);
            }
            SiteController.SaveSite(site);

            LogModel log = LogController.NewLogModel();
            log.SiteID = site.SiteID;
            log.RunTime = pingBegan;
            
            try
            {
                WebRequest req = WebRequest.Create(site.Url);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                log.ResultCode = res.StatusCode.ToString();
                log.Success = true;
            }
            catch (Exception ex)
            {
                log.ResultCode = "exception";
                log.Message = ex.Message;
                log.Success = false;
            }

            LogController.SaveLog(log);
        }

 
    }
}