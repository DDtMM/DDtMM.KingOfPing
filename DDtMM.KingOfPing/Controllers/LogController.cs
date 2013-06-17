using DDtMM.KingOfPing.Managers;
using DDtMM.KingOfPing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDtMM.KingOfPing.Controllers
{
    public static class LogController
    {
        static Func<websitePingConfig.logRow, LogModel> rowToModel =
            delegate(websitePingConfig.logRow l)
        {
            return new LogModel()
            {
                LogID = l.logID,
                SiteID = l.siteID,
                RunTime = l.runTime,
                Success = l.success,
                ResultCode = l.resultCode,
                Message = (l.Islog_textNull()) ? null : l.log_text,
            };
        };
   
        public static LogModel NewLogModel()
        {
            return new LogModel()
            {
                LogID = -1,
                SiteID = -1
            };
        }


        public static List<LogModel> LoadSiteLogs(int siteID)
        {
            return DataManager.Instance.PingSettings.log
                .Where(l => l.siteID == siteID).Select(rowToModel).ToList();
        }

        public static List<LogModel> LoadServerLogs()
        {
            return DataManager.Instance.PingSettings.log
                .Where(l => l.siteID == -1).Select(rowToModel).ToList();
        }

        /// <summary>
        /// Quickly adds a new log record.
        /// </summary>
        /// <param name="success"></param>
        /// <param name="resultCode"></param>
        /// <param name="runTime"></param>
        /// <param name="message"></param>
        /// <param name="siteID"></param>
        public static void AddLog(bool success, string resultCode, DateTime runTime, 
            string message = null, int siteID = -1)
        {
            LogModel log = NewLogModel();
            log.Success = success;
            log.ResultCode = resultCode;
            log.RunTime = runTime;
            log.Message = message;
            log.SiteID = siteID;
            SaveLog(log);
            DataManager.Instance.SaveChanges();
        }

        /// <summary>
        /// Save a log in memory.
        /// </summary>
        /// <param name="log"></param>
        public static void SaveLog(LogModel log)
        {
            websitePingConfig.logDataTable table = DataManager.Instance.PingSettings.log;
 
            websitePingConfig.logRow row;

            if (log.LogID >= 0)
            {
                row = table.First(l => l.logID == log.LogID);
                row.BeginEdit();
                CopyModelToRow(log, row);
                row.EndEdit();
                row.AcceptChanges();
            }
            else
            {
                lock (table)
                {
                    if (ServerController.LoadServer().MaxLogSize <=
                        table.Count(l => l.siteID == log.SiteID))
                    {
                        table.Where(l => l.siteID == log.SiteID)
                            .OrderByDescending(l => l.logID)
                            .Skip(ServerController.LoadServer().MaxLogSize).ToList()
                            .ForEach(l => table.Rows.Remove(l));
 
                    }

                    row = table.NewlogRow();
                    row.BeginEdit();
                    row.logID = (table.Count() > 0) ? table.Max(l => l.logID) + 1 : 1;
                    CopyModelToRow(log, row);
                    row.EndEdit();
                    table.AddlogRow(row);
                    table.AcceptChanges();
                }
            }


            table.AcceptChanges();

        }

        /// <summary>
        /// copies non-key values to row
        /// </summary>
        /// <param name="log"></param>
        /// <param name="row"></param>
        private static void CopyModelToRow(LogModel log, websitePingConfig.logRow row)
        {
            row.log_text = log.Message;
            row.resultCode = log.ResultCode;
            row.runTime = log.RunTime;
            row.siteID = log.SiteID;
            row.success = log.Success;
        }

    }
}