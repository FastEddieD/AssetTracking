using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingDashboard.Model
{
    public class StationX
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int DeptId { get; set; }
        public string Description { get; set; }
        public int LastScanId { get; set; }
    }
}
