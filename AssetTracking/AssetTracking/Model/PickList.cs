using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetTracking.Model
{
    public class PickList
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public DateTime ScanTime { get; set; }
        public int OperatorId { get; set; }
        public int StationId { get; set; }
        public int ActionItemId { get; set; }
        public override string ToString()
        {
            return $"({Id}) {ItemCode} {Description} ({Qty})";
        }
        public string ItemDescription
        {
            get
            {
                return $"({Id}) {ItemCode} {Description} ({Qty})";
            }
        }

    }
}
