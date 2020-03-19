using AssetTrackingWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Configuration;
using Dapper;
namespace AssetTrackingWebAPI.Controllers
{
    public class ScanController : ApiController
    {
        string connectionString = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
           
      
        private List<Scan> scans;

        public ScanController()
        {
            scans = new List<Scan>();
            scans.Add(new Scan { Id = 1, Item = "123123", Station = "1", Action = "Load", ScanTime = DateTime.Now, Operator = "A", Processed = false });
            scans.Add(new Scan { Id = 2, Item = "321123", Station = "1", Action = "Received", ScanTime = DateTime.Now, Operator = "A", Processed = false });
            scans.Add(new Scan { Id = 3, Item = "321123", Station = "1", Action = "Load", ScanTime = DateTime.Now, Operator = "A", Processed = false });
            scans.Add(new Scan { Id = 4, Item = "123123", Station = "2", Action = "Load", ScanTime = DateTime.Now, Operator = "A", Processed = false });
        }


        // GET: api/Scan
        public List<Scan> Get()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
                // var output = connection.Query<Scan>($"Select * from Scans").ToList();
                var output = connection.Query<Scan>("dbo.spGetScansAll").ToList();
                return output;
            }
        }
        // GET/SCAN/GETBYDATE/
        public List<Scan> GetByDate(DateTime scanDate)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
                return connection.Query<Scan>($"Select * from Scans where ScanTime = '{scanDate}'").ToList();
            }
        }
        public List<Scan> GetByStationId(int stationId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
                return connection.Query<Scan>($"Select * from Scans where StationId = '{stationId}'").ToList();
            }
        }
        public List<Scan> GetByItem(string item)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
                return connection.Query<Scan>($"Select * from Scans where Item = '{item}'").ToList();
            }
        }






        [Route("api/Scan/GetByItem/{id}")]
      //  [HttpGet]
        public List<Scan> GetByItem(int id)
        {
            return scans.Where(x => x.Id == id).ToList();
        }


        // GET: api/Scan/5
        public Scan Get(int id)
        {
            return scans.Where(x => x.Id == id).FirstOrDefault();
        }

        // POST: api/Scan
        public void Post(Scan val)
        {
            scans.Add(val);

        }

        ////POST: api/scan/SaveScan
        //[HttpPost]
        //[Route("api/Scan/SaveScan/{scan:scan}")]
        //public void SaveScan(Scan val)
        //{
        //    // connect to database and save a new scan
        //    Scan newScan = new Scan();
        //    newScan.ScanTime = val.ScanTime;
        //    newScan.Item = val.Item;
        //    newScan.Operator = val.Operator;
        //    newScan.Processed = false;
        //    newScan.Station = val.Station;
            


        //}

        // DELETE: api/Scan/5
        public void Delete(int id)
        {
            // Make a database call to remove         
            // This is not right
            // scans.Remove.Where(x => x.Id == id);        }
        }
    }
}
      // string connectionString = "Data Source=avazwvzjob.database.windows.net;Initial Catalog=AssetTrackingDB;User ID=edolikian;Password=********;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                // DataTable dtblScans = new DataTable();
                // scans = new List<Scan>();
                // SqlConnection sqlCon = new SqlConnection(connectionString);
                //     sqlCon.Open();
                //     SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Scans",connectionString);
                //     sqlDa.Fill(dtblScans);
                //     for (int i = 0; i < dtblScans.Rows.Count; i++)
                //     {
                //         DataRow scan = dtblScans.Rows[i];
                //         scans.Add(new Scan
                //         {
                ////             Id = dtblScans.Rows[i],
                //             Item = dtblScans.Rows[i]["Item"].ToString(),
                //             Action = scan["Action"].ToString(),
                //             ScanTime = (DateTime)scan["ScanTime"],
                //             Operator = scan["Operator"].ToString(),
                //             Processed = (bool)scan["Processed"]
                //         });



                //     }

                //return scans;