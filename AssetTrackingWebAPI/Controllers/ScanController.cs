using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AssetTrackingWebAPI.Models;
using System.Diagnostics;
using System.Data;
using Dapper;
using AssetTracking.Model;

namespace AssetTrackingWebAPI.Controllers
{

    public class ScanController : Controller
    {
        private List<Scan> scans;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET api/values
        [HttpGet]
        [Route("Scan/GetTacos")]
        public ActionResult<IEnumerable<string>> GetTacos()
        {
            return new string[] { "taco1", "taco2" };
        }

        [HttpGet]
        [Route("Scan/Get")]
        public ActionResult<IEnumerable<Scan>> Get()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
                var output = connection.Query<Scan>($"Select * from Scans").ToList();
                //   var output = connection.Query<Scan>("dbo.spGetScansAll").ToList();
                return output;
            }
        }
        
        // GET/Scan/GetPlants
        [HttpGet]
        [Route("Scan/GetPlants")]
        public ActionResult<IEnumerable<Plant>> GetPlants()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
                var output = connection.Query<Plant>($"Select * from Plant").ToList();
                return output;
            }
        }
        // GET/Scan/GetDepts
        [HttpGet]
        [Route("Scan/GetDepts")]
        public ActionResult<IEnumerable<Dept>> GetDepts()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
         
                var output = connection.Query<Dept>($"Select * from Dept").ToList();
                return output;
            }
        }
        // GET/Scan/GetStations
        [HttpGet]
        [Route("Scan/GetStations")]
        public ActionResult<IEnumerable<Station>> GetStations()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
                var output = connection.Query<Station>($"Select * from Station").ToList();
                return output;
            }
        }

        // GET/Scan/GetStationStatus
        [HttpGet]
        [Route("Scan/GetStationStatus")]
        public ActionResult<IEnumerable<StationStatus>> GetStationStatus()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
                var output = connection.Query<StationStatus>($"Select * FROM vwStationList").ToList();
                return output;
            }
        }

        // GET/Scan/GetItemHistory
        [HttpGet]
        [Route("Scan/GetItemHistory/{item}")]
        public ActionResult<IEnumerable<ItemHistory>> GetItemHistory(string item)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
                var output = connection.Query<ItemHistory>($"Select * from vwItemHistory WHERE Item = @item ORDER BY ID DESC", new { item }).ToList();
                return output;
            }
        }

        // GET/Scan/GetActions
        [HttpGet]
        [Route("Scan/GetActions")]
        public ActionResult<IEnumerable<ActionItem>> GetActions()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
                var output = connection.Query<ActionItem>($"Select * from ActionItems").ToList();
                return output;
            }
        }

        // GET/Scan/GetOperators
        [HttpGet]
        [Route("Scan/GetOperators")]
        public ActionResult<IEnumerable<Operator>> GetOperators()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
                var output = connection.Query<Operator>($"Select * from Operator").ToList();
                return output;
            }
        }

        [HttpGet]
        [Route("Scan/GetByStationId/{StationId}")]
        public ActionResult<IEnumerable<Scan>> GetByStationId(int stationId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
                var selectString = "SELECT * FROM Scans where StationId = @stationId";
                var output = connection.Query<Scan>(selectString, new {stationId}).ToList();
                //   var output = connection.Query<Scan>("dbo.spGetScansAll").ToList();
                return output;
            }
        }

        [HttpGet]
        [Route("Scan/GetByItem/{Item}")]
        public ActionResult<IEnumerable<Scan>> GetByItem(string item)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
                var selectString = "SELECT * FROM Scans where Item = @item";
                var output = connection.Query<Scan>(selectString, new { item}).ToList();
                //   var output = connection.Query<Scan>("dbo.spGetScansAll").ToList();
                return output;
            }
        }

        [HttpPost]
        [Route("Scan/SaveScan")]
        public void SaveScan([FromBody]Scan val)
        {
            string sqlString = "INSERT INTO SCANS (ScanTime, StationId, Item, OperatorId, ActionItemId, Latitude, Longitude, Processed) VALUES (@ScanTime, @StationId, @Item, @OperatorId, @ActionItemId, @Latitude, @Longitude, @Processed)";
            string sqlUpdateStationStatus = "UPDATE [dbo].[Station] SET LastScanId = @Id WHERE Id = @StationId";
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
            {
                var output = connection.Execute(sqlString, val);
                string sqlgetId = "SELECT * FROM SCANS WHERE (ScanTime = @ScanTime and StationId = @StationId)";
                var newId = connection.ExecuteScalar(sqlgetId, val);
                val.Id = Int32.Parse(newId.ToString());
                var output2 = connection.Execute(sqlUpdateStationStatus, val);
     //           return output.OkResult("RecordSaved");
            }
        }


    }




    }



      //  string connectionString = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
           
      
    
        //public ScanController()
        //{
        //    scans = new List<Scan>();
        //    scans.Add(new Scan { Id = 1, Item = "123123", Station = "1", Action = "Load", ScanTime = DateTime.Now, Operator = "A", Processed = false });
        //    scans.Add(new Scan { Id = 2, Item = "321123", Station = "1", Action = "Received", ScanTime = DateTime.Now, Operator = "A", Processed = false });
        //    scans.Add(new Scan { Id = 3, Item = "321123", Station = "1", Action = "Load", ScanTime = DateTime.Now, Operator = "A", Processed = false });
        //    scans.Add(new Scan { Id = 4, Item = "123123", Station = "2", Action = "Load", ScanTime = DateTime.Now, Operator = "A", Processed = false });
        //}


        //// GET: api/Scan
        //public List<Scan> Get()
        //{
        //    using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
        //    {
        //        // var output = connection.Query<Scan>($"Select * from Scans").ToList();
        //        var output = connection.Query<Scan>("dbo.spGetScansAll").ToList();
        //        return output;
        //    }
        //}
        //// GET/SCAN/GETBYDATE/
        //public List<Scan> GetByDate(DateTime scanDate)
        //{
        //    using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
        //    {
        //        return connection.Query<Scan>($"Select * from Scans where ScanTime = '{scanDate}'").ToList();
        //    }
        //}
        //public List<Scan> GetByStationId(int stationId)
        //{
        //    using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
        //    {
        //        return connection.Query<Scan>($"Select * from Scans where StationId = '{stationId}'").ToList();
        //    }
        //}
        //public List<Scan> GetByItem(string item)
        //{
        //    using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("AssetTrackingDB")))
        //    {
        //        return connection.Query<Scan>($"Select * from Scans where Item = '{item}'").ToList();
        //    }
        //}






//        [Route("api/Scan/GetByItem/{id}")]
//      //  [HttpGet]
//        public List<Scan> GetByItem(int id)
//        {
//            return scans.Where(x => x.Id == id).ToList();
//        }


//        // GET: api/Scan/5
//        public Scan Get(int id)
//        {
//            return scans.Where(x => x.Id == id).FirstOrDefault();
//        }

//        // POST: api/Scan
//        public void Post(Scan val)
//        {
//            scans.Add(val);

//        }

//        ////POST: api/scan/SaveScan
//        //[HttpPost]
//        //[Route("api/Scan/SaveScan/{scan:scan}")]
//        //public void SaveScan(Scan val)
//        //{
//        //    // connect to database and save a new scan
//        //    Scan newScan = new Scan();
//        //    newScan.ScanTime = val.ScanTime;
//        //    newScan.Item = val.Item;
//        //    newScan.Operator = val.Operator;
//        //    newScan.Processed = false;
//        //    newScan.Station = val.Station;
            


//        //}

//        // DELETE: api/Scan/5
//        public void Delete(int id)
//        {
//            // Make a database call to remove         
//            // This is not right
//            // scans.Remove.Where(x => x.Id == id);        }
//        }
//    }
//}
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