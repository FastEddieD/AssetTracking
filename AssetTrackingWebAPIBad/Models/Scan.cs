using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetTrackingWebAPI.Models
{
    public class Scan
    {
        public int Id { get; set; }
        public DateTime ScanTime { get; set; }
        public string Action { get; set; } = "";
        public string Station { get; set; } = "";
        public string Item { get; set; } = "";
        public string Operator { get; set; } = "";
        public bool Processed { get; set; }
    }
}