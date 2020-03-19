using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetTracking.Model
{
    public class Station
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Code { get; set; }
        public int DeptId { get; set; }
        public int OperatorId { get; set; }
        public int ActionItemId { get; set; }
        public string Description { get; set; }
        public int LastScanId { get; set; }
        public override string ToString()
        {
            return $"({Id}) {Code} {Description}";
        }
        public string StationDescription
        {
            get
            {
                return $"({Id}) Station {Code} {Description}";
            }
        }
    }
   
}
