using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetTracking.Model
{
    public class Scan
    {
        [PrimaryKey]
        public int Id { get; set; }
        public DateTime ScanTime { get; set; }
        public string Station { get; set; }
        public int StationId { get; set; }
        public string Item { get; set; }
        public string Operator { get; set; }
        public int OperatorId { get; set; }
        public string Action { get; set; }
        public int ActionItemId { get; set; }
        public bool IsSaved { get; set; }
        public bool Processed { get; set; }
        public bool ActionIsLocked { get; set; }
        public bool StationIsLocked { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public override string ToString()
        {
            return this.ScanTime.ToShortTimeString() + " " + this.Station + " " + this.Action + " " + this.Item;
        }
    }


}
