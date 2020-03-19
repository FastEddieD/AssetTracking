using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace AssetTrackingWebAPI
{
   public static class Helper
   {
       public static string CnnVal(string name)
       {
//           var connstring = IConfigurationManager<Configuration>.ConnectionStrings[name].ConnectionString;
           var connstring = "Server=avazwvzjob.database.windows.net,1433;Initial Catalog=AssetTrackingDB;Persist Security Info=False;User ID=edolikian;Password=S3ns1ble; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
           return connstring;

       }
   }

    
}