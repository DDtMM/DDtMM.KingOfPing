using DDtMM.KingOfPing.Managers;
using DDtMM.KingOfPing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDtMM.KingOfPing.Controllers
{
    public static class SiteController
    {
        static Func<websitePingConfig.siteRow, SiteModel> rowToModel =
            delegate(websitePingConfig.siteRow s)
            {
                return new SiteModel()
                {
                    EndTime = Util.NextTimeDate(s.endTime.TimeOfDay),
                    Interval = s.interval,
                    NextRun = (s.IsnextRunNull()) ? DateTime.Now : s.nextRun,
                    SiteID = s.siteID,
                    StartTime = Util.NextTimeDate(s.startTime.TimeOfDay),
                    Url = s.url
                };
            };

        public static List<SiteModel> LoadSites()
        {
            return DataManager.Instance.PingSettings.site.Select(rowToModel).ToList();
        }
        public static SiteModel LoadSite(int siteID)
        {
            return DataManager.Instance.PingSettings.site
                .Where(s => s.siteID == siteID)
                .Select(rowToModel)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns all sites that need be pinged, based on ping interval and last run
        /// </summary>
        /// <returns></returns>
        public static List<SiteModel> LoadSitesToPing()
        {
            int sleepOffset = ServerController.LoadServer().MinSleep * -1;
            DateTime now = DateTime.Now;

            return (
                from s in DataManager.Instance.PingSettings.site
                where
                (s.IsnextRunNull() || s.nextRun.AddSeconds(sleepOffset) < now)
                && Util.TimeInRange(now, s.startTime, s.endTime)

                select rowToModel(s)).ToList();
        }

        public static void SaveSite(SiteModel site)
        {


            websitePingConfig.siteRow row;
            websitePingConfig.siteDataTable table = DataManager.Instance.PingSettings.site;

            if (site.SiteID >= 0)
            {
                row = table.First(s => s.siteID == site.SiteID);
                row.BeginEdit();
                CopyModelToRow(site, row);
                row.EndEdit();
                row.AcceptChanges();
            }
            else
            {
                lock (table)
                {
                    row = table.NewsiteRow();
                    row.BeginEdit();
                    row.siteID = (table.Count() > 0) ? table.Max(s => s.siteID) + 1 : 1;
                    CopyModelToRow(site, row);
                    row.EndEdit();
                    table.AddsiteRow(row);
                    table.AcceptChanges();
                }
            }


        }

        /// <summary>
        /// copies non-key values to row
        /// </summary>
        /// <param name="log"></param>
        /// <param name="row"></param>
        private static void CopyModelToRow(SiteModel site, websitePingConfig.siteRow row)
        {
            row.endTime = site.EndTime;
            row.interval = site.Interval;
            row.nextRun = site.NextRun;
            row.startTime = site.StartTime;
            row.url = site.Url;
        }
    }

}