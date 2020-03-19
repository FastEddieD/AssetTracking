using System;
using System.Collections.Generic;
using System.Text;

namespace AssetTrackingDataService.Model
{
    public class Scan
    {
        public int Id { get; set; }
        public DateTime ScanTime { get; set; }
        public int StationId { get; set; }
        public string Item { get; set; }
        public int OperatorId { get; set; }
        public int ActionItemId { get; set; }
        public bool Processed { get; set; }
        public override string ToString()
        {
            return this.ScanTime.ToShortTimeString() + " " + this.Item + " Action=" + this.ActionItemId + " @ Station=" + this.Item;
        }
    }
    


}
