using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DDtMM.KingOfPing
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPingService" in both code and config file together.
    [ServiceContract]
    public interface IPingService
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
         ResponseFormat = WebMessageFormat.Json)]
        void RunNow();

        [OperationContract]
        [WebInvoke(Method = "GET",
         ResponseFormat = WebMessageFormat.Json)]
        List<Models.SiteModel> GetSites();

        [OperationContract]
        [WebInvoke(Method = "GET",
         ResponseFormat = WebMessageFormat.Json)]
        DateTime GetServerTime();

        [OperationContract]
        [WebInvoke(Method = "GET",
         ResponseFormat = WebMessageFormat.Json)]
        DateTime GetNextRunTime();
    }
}
