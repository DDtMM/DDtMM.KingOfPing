using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using System.Net;

using System.Configuration;
using DDtMM.KingOfPing.Managers;
using DDtMM.KingOfPing.Controllers;
using DDtMM.KingOfPing.Models;

namespace DDtMM.KingOfPing
{
    public class Global : System.Web.HttpApplication
    {
        static Thread keepAliveThread = new Thread(KeepAlive);

        protected void Application_Start()
        {
            keepAliveThread.Start();
        }

        protected void Application_End()
        {
            keepAliveThread.Abort();
            ServerController.Current.Status = ServerModel.ServerState.Stopped;
            LogController.AddLog(true, "101", DateTime.Now, "Server Stopped");
        }

        static void KeepAlive()
        {
            int nextRun;

            ServerController.Current.Status = ServerModel.ServerState.Waiting;
            LogController.AddLog(true, "100", DateTime.Now, "Server Started");

            while (true)
            {

                try
                {
                    ServerController.Current.Status = ServerModel.ServerState.Pinging;
                    PingManager.Instance.PingSites(SiteController.LoadSitesToPing());
                }
                catch (Exception ex)
                {
                    LogController.AddLog(false, "400", DateTime.Now, ex.Message);
                }
                nextRun = Convert.ToInt32(PingManager.Instance.NextRunSeconds()) * 1000;
                ServerController.Current.Status = ServerModel.ServerState.Waiting;
 

                try
                {
                    Thread.Sleep(nextRun);
                }
                catch (ThreadAbortException)
                {
                    break;
                }
            }
            
        }
    }
}